const scriptName = "07_create_Item";
const version = NumberInt(1);

if (db.MigrationHistory.findOne({ ScriptName: scriptName, Version: version })) {
    print(`${scriptName} v${version} is already applied`);
    quit();
}

db.createCollection("Item", {
    collation: {
        locale: "en"
    }
});

const itemIds = [
    UUID("8892462c-df8f-4a6f-b5ce-95e89db220bf"),
    UUID("f50ce8fc-9b5e-47e9-aeb9-7eab03677b8d"),
    UUID("c96844cf-5c1d-4672-b339-4a3eb6a9db8b"),
    UUID("76fb1a23-2f77-4c26-bf45-fc655f7432e6"),
    UUID("f55ce9a9-db00-4634-9759-2f369c4d0df1"),
    UUID("351e5e82-b200-4e5d-8b1c-9e0d2e52ceaa"),
    UUID("5c98227f-e9b7-45dd-bfdb-22dddf384598"),
    UUID("3b9ff6f2-d612-481c-b2a5-17c7c1c1ffb3"),
    UUID("e2b72b14-f334-4ef9-81b5-a86045e39c12"),
    UUID("aacf3867-90bf-422c-b271-540f2d7a157a")
];

const names = [
    "Car",
    "Development",
    "Consulting Service",
    "Wholesale purchase",
    "Financial service"
];

const descriptions = [
    "A high-quality vehicle equipped with modern technology for comfort and safety.",
    "Custom software development services tailored to meet specific business needs and foster innovation.",
    "Professional consulting services aimed at strategic growth and effective process optimization.",
    "Bulk purchasing options offering competitive pricing and reliable supply chain management.",
    "Comprehensive financial services including investment advisory, capital management, and risk assessment."
];

const clientIds = [
    UUID("ea94747b-3d45-46d6-8775-bf27eb5da02b"),
    UUID("866eb606-d074-4237-bcf2-aa7798002f7f"),
    UUID("5cb0b8ed-45f4-4432-9ff7-3a9f896362f9"),
    UUID("15de1dcc-98c2-4463-85ed-b36a6a31445a"),
    UUID("e1e39dd5-1ec0-4f9a-b765-d6dc25f0d9a7"),
    UUID("d728b231-0b5d-4c90-a2d4-675cbcb64ff2"),
    UUID("4e1f0ed6-0915-48cd-9bf0-eb804e7a919e"),
    UUID("b40ef306-6ac2-4fa8-b703-df291799feef"),
    UUID("00c1df50-320e-447b-8b94-7b2fab0fcf58"),
    UUID("31e52122-ea93-448a-8827-fb5f079cbd1a")
]

const currencyIds = [
    UUID("c1ce0c2a-6701-4d66-95d3-812fa9b2ca08"),
    UUID("04d123f0-dc7e-4b92-829c-dffd1ef0b89a"),
    UUID("45d6d081-e362-4a9d-996f-c144d944635d"),
    UUID("fd76eaab-194a-4e44-a4f8-3eed74c729c8"),
    UUID("f3cc7604-0d40-446e-86fe-e55b103d35b5"),
    UUID("5b8f21fe-f7f4-40a4-866a-6d53fe2be04e"),
    UUID("c9449281-83e3-447d-988d-4b4c9ff99cdc"),
    UUID("f3df2c57-c380-4ca0-b9d7-b165b82491f0"),
    UUID("c555cb42-4bdc-42d6-b12a-5ac03adc7858"),
    UUID("91c1fc7a-7cc6-45a3-b55f-8accd17daa78"),
    UUID("9c9a623c-505a-42a7-b72f-2ccca7bd80f0"),
    UUID("8a85572d-a02f-43dc-8d53-b70283d97f19"),
    UUID("4f994031-6a4a-4dbf-87d4-07f7273c4583"),
    UUID("bd5ce43f-130f-464f-82ce-faaf7149eec3"),
    UUID("968c4214-0ffc-4021-a386-dd8a66cefbe9"),
    UUID("c23007e8-a586-4b95-9af6-dfb21373d7cc"),
    UUID("b28a0fe8-196f-4583-9721-f255125b0678"),
    UUID("eecf1120-3bc5-4e59-bfa2-a907763ea1c7"),
    UUID("20276776-a4e7-4eae-af23-24b7cfcf12aa"),
    UUID("b84af585-c2ea-4ea3-ba7c-f3e0f2ac7573"),
    UUID("458c5c94-5934-4d93-9573-1e804f2897c9"),
    UUID("70f27608-6a95-421e-9df3-878fdf487175"),
    UUID("3ffa8766-db5f-47f6-b64c-7073ea126774"),
    UUID("ccbfd304-0d7d-47a7-aa07-dc5ed2f55f61"),
    UUID("14883dfa-74ad-491e-88ac-90bd98c7d672"),
    UUID("4481cfef-cfcd-4787-8708-074df2d6fabe"),
    UUID("878bb678-59fb-43af-aa1c-f175ef90a43a"),
    UUID("92f6c59f-220d-4cb5-9bef-fef89b7ce599"),
    UUID("6566f8ce-32e1-4426-aeed-b7fe55bdfdbb"),
    UUID("0eb90e6e-032b-4eb1-a33e-af0c7f581465"),
];

const currencyCodes = [
    "USD", "JPY", "BGN", "CZK", "DKK", "GBP", "HUF", "PLN", "RON", "SEK",
    "CHF", "ISK", "NOK", "TRY", "AUD", "BRL", "CAD", "CNY", "HKD", "IDR",
    "ILS", "INR", "KRW", "MXN", "MYR", "NZD", "PHP", "SGD", "THB", "ZAR"
];
    
function getRandomInt(max) {
    return Math.floor(Math.random() * max);
}

const items = [];
const itemCount = 10;

for (let i = 0; i < itemCount; i++) {
    var nameIndex = getRandomInt(names.length);
    var currencyIndex = getRandomInt(currencyIds.length);

    items.push({
        _id: itemIds[i],
        ClientId: clientIds[i],
        Name: names[nameIndex],
        Description: descriptions[nameIndex],
        Price: NumberDecimal((Math.random() * 2000 + 100).toFixed(2)),
        CurrencyId: currencyIds[currencyIndex],
        currencyCode: currencyCodes[currencyIndex],
        IsDeleted: false
    });
}

const opt = items.map(item => ({
    replaceOne: {
        filter: { _id: item._id },
        replacement: item,
        upsert: true
    }
}));
db.Item.bulkWrite(opt);

db.MigrationHistory.insertOne({
    ScriptName: scriptName,
    Version: version,
    ScriptRunTime: new Date()
});

print("All items are inserted successfully!");