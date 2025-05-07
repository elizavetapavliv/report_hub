const scriptName = "22_update_User_AddNotificationSettings";
const version = NumberInt(1);

if (db.MigrationHistory.findOne({ ScriptName: scriptName, Version: version })) {
    print(`${scriptName} v${version} is already applied`);
    quit();
}

const frequencies = ["Daily", "Weekly", "Monthly"];
const daysOfWeek = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
const exportFormats = ["Excel", "CSV"];
const hours = [8, 9, 10, 11, 14, 15, 16, 17];

const users = db.User.find({}, { _id: 1 }).toArray();

var updates = users.map(user => {
    const notificationSettings =
    {
        DayOfWeek: null,
        DayOfMonth: null,
    };

    const randomFrequency = frequencies[Math.floor(Math.random() * frequencies.length)];
    notificationSettings.Frequency = randomFrequency;


    notificationSettings.Hour = hours[Math.floor(Math.random() * hours.length)];

    if (randomFrequency === "Weekly") {
        notificationSettings.DayOfWeek = daysOfWeek[Math.floor(Math.random() * daysOfWeek.length)]
    }
    else if (randomFrequency === "Monthly") {
        notificationSettings.DayOfMonth = Math.floor(Math.random() * 28) + 1
    }

    notificationSettings.ExportFormat = exportFormats[Math.floor(Math.random() * exportFormats.length)];
    notificationSettings.ReportStartDate = null;
    notificationSettings.ReportEndDate = null;

    return {
        updateOne: {
            filter: { _id: user._id },
            update: {
                $set: {
                    NotificationSettings: notificationSettings
                }
            }
        }
    };
});

if (updates.length > 0) {
    db.User.bulkWrite(updates);
}

db.MigrationHistory.insertOne({
    ScriptName: scriptName,
    Version: version,
    ScriptRunTime: new Date()
});

print("Users Updated Successfully");