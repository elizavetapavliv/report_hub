const scriptName = "15_update_ExchangeRate_TTLIndexUpdate";
const version = NumberInt(1);

if (db.MigrationHistory.findOne({ ScriptName: scriptName, Version: version })) {
    print(`${scriptName} v${version} is already applied`);
    quit();
}

db.collection.dropIndex("RateDate_1");

db.ExchangeRate.createIndex(
    { RateDate: 1 },
    { expireAfterSeconds: 7 * 24 * 60 * 60 }
);

print("Index created successfully!");