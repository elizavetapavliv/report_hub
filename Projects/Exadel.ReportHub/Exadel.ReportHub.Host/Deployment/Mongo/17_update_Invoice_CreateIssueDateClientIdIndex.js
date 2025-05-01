﻿const scriptName = "17_update_Invoice_CreateIssueDateClientIdIndex";
const version = NumberInt(1);

if (db.MigrationHistory.findOne({ ScriptName: scriptName, Version: version })) {
    print(`${scriptName} v${version} is already applied`);
    quit();
}

db.Invoice.createIndex(
    { IssueDate: 1, ClientId: 1 },
    { partialFilterExpression: { IsDeleted: false } }
);

db.MigrationHistory.insertOne({
    ScriptName: scriptName,
    Version: version,
    ScriptRunTime: new Date()
});

print("index on IssueDate and ClientId created successfully!");