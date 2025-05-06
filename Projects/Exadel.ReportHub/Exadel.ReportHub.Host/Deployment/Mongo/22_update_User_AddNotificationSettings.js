const scriptName = "22_update_User_AddNotificationSettings";
const version = NumberInt(1);

if (db.MigrationHistory.findOne({ ScriptName: scriptName, Version: version })) {
    print(`${scriptName} v${version} is already applied`);
    quit();
}

const notificationFrequencies = ["Daily", "Weekly", "Monthly"];
const daysOfWeek = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
const reportFormats = ["Excel", "CSV"];
const notificationTimes = [8, 9, 10, 11, 14, 15, 16, 17];

const users = db.User.find({}, { _id: 1 }).toArray();


var updates = users.map(user => {
    const notificationSettings =
    {
        NotificationDayOfWeek: null,
        NotificationDayOfMonth: null,
    };

    const randomFrequency = notificationFrequencies[Math.floor(Math.random() * notificationFrequencies.length)];
    notificationSettings.NotificationFrequency = randomFrequency;


    notificationSettings.NotificationTime = notificationTimes[Math.floor(Math.random() * notificationTimes.length)];

    if (randomFrequency === "Weekly") {
        notificationSettings.NotificationDayOfWeek = daysOfWeek[Math.floor(Math.random() * daysOfWeek.length)];
    }
    else if (randomFrequency === "Monthly") {
        notificationSettings.NotificationDayOfMonth = Math.floor(Math.random() * 28) + 1;
    }

    notificationSettings.ReportFormat = reportFormats[Math.floor(Math.random() * reportFormats.length)];

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