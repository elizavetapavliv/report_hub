const scriptName = "03_create_User";
const version = NumberInt(1);
const demoPassword = "Pa$$word"

if (db.MigrationHistory.findOne({ scriptName, version })) {
    print(`${scriptName} v${version} is already applied`);
    quit();
}

db.createCollection("User", {
    collation: {
        locale: "en"
    }
});

db.User.InsertMany([
    {
        _id: UUID(),
        Email: "demo.user1@gmail.com",
        FullName: "Dmitry Rohau",
        Password: demoPassword,
        IsActive: false
    },
    {
        _id: UUID(),
        Email: "demo.user2@gmail.com",
        FullName: "Makar Kniazeu",
        Password: demoPassword,
        IsActive: false
    },
    {
        _id: UUID(),
        Email: "demo.user3@gmail.com",
        FullName: "Giorgi Chekurishvili",
        Password: demoPassword,
        IsActive: true
    },
    {
        _id: UUID(),
        Email: "demo.user4@gmail.com",
        FullName: "Tengo Giorgadze",
        Password: demoPassword,
        IsActive: true
    }
]);

db.MigrationHistory.insertOne({
    SriptName: scriptName,
    Version: version,
    ScriptRunTime: new Date()
});

print("All users are inserted successfully!");