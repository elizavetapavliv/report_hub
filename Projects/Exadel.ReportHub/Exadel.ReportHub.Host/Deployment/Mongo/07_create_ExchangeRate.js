﻿const scriptName = "02_create_ExchangeRateCollection";
const version = NumberInt(2);

if (db.MigrationHistory.findOne({ ScriptName: scriptName, Version: version })) {
    print(`${scriptName} v${version} is already applied`);
    quit();
}

db.createCollection("ExchangeRate", {
    collation: { locale: "en" }
});

db.ExchangeRate.createIndex(
    { Currency: 1 },
    {
        unique: true,
        background: true
    }
);

db.ExchangeRate.createIndex(
    { RateDate: 1 },
    { expireAfterSeconds: 24 * 60 * 60 }
);

db.MigrationHistory.insertOne({
    ScriptName: scriptName,
    Version: version,
    ScriptRunTime: new Date()
});

print("ExchangeRate collection created.");