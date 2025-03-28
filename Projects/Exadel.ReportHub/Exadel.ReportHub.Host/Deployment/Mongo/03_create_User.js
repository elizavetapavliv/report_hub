const scriptName = "03_create_User";
const version = NumberInt(1);
const demoPasswordHash = "J4jyN3Qpyto3ioa0/UVri74uz8PW0aDAnTV4NwNBdWEs0KhV4kEW2KMpG/OUYbj8uZqJbnJmhCx1MDuwleovHw==";
const demoPasswordSalt = "bQSPPP6fkMp1aKLIZY4Tdw==";

if (db.MigrationHistory.findOne({ ScriptName: scriptName, Version: version })) {
    print(`${scriptName} v${version} is already applied`);
    quit();
}

db.createCollection("User", {
    collation: {
        locale: "en"
    }
});

db.User.insertMany([
    {
        _id: UUID(),
        Email: "demo.user1@gmail.com",
        FullName: "Tony Stark",
        PasswordHash: demoPasswordHash,
        PasswordSalt: demoPasswordSalt,
        IsActive: false
    },
    {
        _id: UUID(),
        Email: "demo.user2@gmail.com",
        FullName: "Jim Carrey",
        PasswordHash: demoPasswordHash,
        PasswordSalt: demoPasswordSalt,
        IsActive: false
    },
    {
        _id: UUID(),
        Email: "demo.user3@gmail.com",
        FullName: "Benedict Cumberbatch",
        PasswordHash: demoPasswordHash,
        PasswordSalt: demoPasswordSalt,
        IsActive: true
    },
    {
        _id: UUID(),
        Email: "demo.user4@gmail.com",
        FullName: "Hugh Jackman",
        PasswordHash: demoPasswordHash,
        PasswordSalt: demoPasswordSalt,
        IsActive: true
    }
]);

db.MigrationHistory.insertOne({
    ScriptName: scriptName,
    Version: version,
    ScriptRunTime: new Date()
});

print("All users are inserted successfully!");