const scriptName = "05_create_Client";
const version = NumberInt(6);

if (db.MigrationHistory.findOne({ ScriptName: scriptName, Version: version })) {
    print(`${scriptName} v${version} is already applied`);
    quit();
}

db.createCollection("Client", {
    collation: {
        locale: "en"
    }
});

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

const customerIds = [
    UUID("f89e1e75-d61c-4c51-b0be-c285500988cf"),
    UUID("e1509ec2-2b05-406f-befa-149f051586a9"),
    UUID("6d024627-568b-4d57-b477-2274c9d807b9"),
    UUID("ba045076-4837-47ab-80d5-546192851bab"),
    UUID("ba18cc29-c7ff-48c4-9b7b-456bcef231d0"),
    UUID("1fddb42d-0436-4123-9448-2821ed38c158"),
    UUID("30f41dcc-fbde-4321-b929-dd332ec27f8d"),
    UUID("ac0fe3cf-9ba1-4bd7-a22c-554972b44ed0"),
    UUID("5223d592-7e90-46b6-939d-9f715d4b5058"),
    UUID("43867fc9-ab6e-43ae-8c2c-97f2575969df"),
    UUID("2b00cf70-2ce9-4787-b4da-5baf62365c16"),
    UUID("802c97bd-2828-42bd-a640-1114eed702e8"),
    UUID("f3e0620c-5bd5-44ba-9ce3-77a6ef5be83d"),
    UUID("1f59b609-adf5-4fd0-94f7-b9c74e6e9572"),
]

const bankAccountNumbers = [
    "US64CHASE123456789012345678",
    "CZ1208000000001923412345",
    "PL441050009976031234567891",
    "IT60X0542811101000000123456",
    "AU042000000123456789",
    "CA1122334455667788991234",
    "FR1420041010050500013M02606",
    "BG80BNBG96611020345678",
    "GB29NWBK60161331926819",
    "DE89370400440532013000"
]

const clientNames = [
    "Acme Corp",
    "Globex Corp",
    "Umbrella Corp",
    "Apple Corp",
    "Pfizer",
    "Novavax Comp",
    "Exadel",
    "Umbrella Corp",
    "Unitex",
    "Maxwell"
]

countries = [
    {
        _id: UUID("5e8b6266-c097-468d-97c1-026457ab22a0"),
        Name: "USA",
        CurrencyId: UUID("c1ce0c2a-6701-4d66-95d3-812fa9b2ca08"),
        CurrencyCode: "USD"
    },
    {
        _id: UUID("10accbc3-4c41-4840-aa9a-11b23f0e0d99"),
        Name: "Czech",
        CurrencyId: UUID("c3a29e5d-8421-4b78-9b88-abb692709441"),
        CurrencyCode: "CZK"
    },
    {
        _id: UUID("5b182ba0-39bf-49c3-812d-cd3ce44bde10"),
        Name: "Poland",
        CurrencyId: UUID("f3cc7604-0d40-446e-86fe-e55b103d35b5"),
        CurrencyCode: "PLN"
    },
    {
        _id: UUID("a4cb75df-6cd1-4a77-a6ed-4e5aa7da85fd"),
        Name: "Italy",
        CurrencyId: UUID("04d123f0-dc7e-4b92-829c-dffd1ef0b89a"),
        CurrencyCode: "EUR"
    },
    {
        _id: UUID("e1a58289-5abf-4333-af5c-4589159fb48a"),
        Name: "Australia",
        CurrencyId: UUID("6acc354b-4ad4-4507-8b94-d27c43a06778"),
        CurrencyCode: "AUD"
    },
    {
        _id: UUID("97d10e39-b9b7-49e5-a27d-d819c016e50e"),
        Name: "Canada",
        CurrencyId: UUID("1bb21106-7b4f-4fd5-9651-adce4c249306"),
        CurrencyCode: "CAD"
    },
    {
        _id: UUID("cae4bb82-26f5-4820-b5aa-1343879fb43a"),
        Name: "France",
        CurrencyId: UUID("04d123f0-dc7e-4b92-829c-dffd1ef0b89a"),
        CurrencyCode: "EUR"
    },
    {
        _id: UUID("b436342d-4b4a-406a-bdde-40d4fe77a381"),
        Name: "Bulgaria",
        CurrencyId: UUID("fd76eaab-194a-4e44-a4f8-3eed74c729c8"),
        CurrencyCode: "BGN"
    },
    {
        _id: UUID("eda3aa62-0a91-467e-b86e-99d7fc2b9342"),
        Name: "UK",
        CurrencyId: UUID("23cc2b9f-55cc-47f9-b7bd-29e818517ccc"),
        CurrencyCode: "GBP"
    },
    {
        _id: UUID("6aa7177a-0768-4e24-a5fd-a3f8446bf6b9"),
        Name: "Germany",
        CurrencyId: UUID("04d123f0-dc7e-4b92-829c-dffd1ef0b89a"),
        CurrencyCode: "EUR"
    },
    ]
const globalClient = {
    _id: UUID("e47501a8-547b-4dc4-ba97-e65ccfc39477"),
    Name: "Global client"
}

function getRandomInt(max) {
    return Math.floor(Math.random() * max);
}

function randomCustomerIds(index) {
    const count = getRandomInt(4) + 1;
    const customers = [];

    for (let i = 0; i < count; i++) {
        customers.push(customerIds[index + i]);
    }
    return customers;
}

const clients = [];
const clientCount = 10;

for (let i = 0; i < clientCount; i++) {
    clients.push({
        _id: clientIds[i],
        Name: clientNames[i],
        BankAccountNumber: bankAccountNumbers[i],
        CustomerIds: randomCustomerIds(i),
        CountryId: countries[i]._id,
        Country: countries[i].Name,
        CurrencyId: countries[i].CurrencyId,
        CurrencyCode: countries[i].CurrencyCode,
        IsDeleted: false
    });
}
clients.push(globalClient);

const opt = clients.map(client => ({
    replaceOne: {
        filter: { _id: client._id },
        replacement: client,
        upsert: true
    }
}));
db.Client.bulkWrite(opt);

db.MigrationHistory.insertOne({
    ScriptName: scriptName,
    Version: version,
    ScriptRunTime: new Date()
});

print("All clients are inserted successfully!");
