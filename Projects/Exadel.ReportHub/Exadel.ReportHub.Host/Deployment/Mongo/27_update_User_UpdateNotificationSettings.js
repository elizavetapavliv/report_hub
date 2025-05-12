const scriptName = "27_update_User_AddClientIdAndName";
const version = NumberInt(1);

if (db.MigrationHistory.findOne({ ScriptName: scriptName, Version: version })) {
    print(`${scriptName} v${version} is already applied`);
    quit();
}
const allowedRoles = ["Owner", "ClientAdmin"];
const reportPeriods = ["Whole", "Month", "Week", "Custom"];

const assignments = db.UserAssignment.aggregate([
    { $match: { "Role": { $in: allowedRoles } } },
    {
        $lookup: {
            from: "Client",
            localField: "ClientId",
            foreignField: "_id",
            as: "ClientInfo",
            pipeline: [
                { $match: { "IsDeleted": false } }
            ]
        }
    },
    { $unwind: "$ClientInfo" }
]).toArray();

const updates = assignments.Map(assignment => {
    const reportPeriod = reportPeriods[Math.floor(Math.random() * reportPeriods.length)];

    const notificationSettings = null;
    if (assignment.ClientInfo) {
        notificationSettings =
        {
            ClientId = assignment.ClientInfo._id,
            ClientName = assignment.ClientInfo.Name,
            ReportPeriod = reportPeriod,
            DaysCount = reportPeriod === "Custom" ? Math.floor(Math.random() * 60) + 1 : null
        };
    }

    return {
        updateOne: {
            filter: { _id: assignment.UserId },
            update: {
                $set: {
                    NotificationSettings: notificationSettings
                }
            }
        }
    }
});

if (updates.length > 0) {
    db.User.bulkWrite(updates);
}

const usersWithAssignments = assignments.map(a => a.UserId);
db.User.updateMany(
    {
        _id: { $nin: usersWithAssignments }
    },
    {
        $set: {
            NotificationSettings: null
        }
    }
);

db.MigrationHistory.insertOne({
    ScriptName: scriptName,
    Version: version,
    ScriptRunTime: new Date()
});

print("Users Updated Successfully");