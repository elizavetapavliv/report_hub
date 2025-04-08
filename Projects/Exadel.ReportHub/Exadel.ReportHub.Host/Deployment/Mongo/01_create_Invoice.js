const scriptName = "01_create_Invoice";
const version = NumberInt(2);

if (db.MigrationHistory.findOne({ ScriptName: scriptName, Version: version })) {
    print(`${scriptName} v${version} is already applied`);
    quit();
}

db.createCollection("Invoice", {
    collation: {
        locale: "en"
    }
});

const clientIds = [
    UUID("ea94747b-3d45-46d6-8775-bf27eb5da02b"),
    UUID("866eb606-d074-4237-bcf2-aa7798002f7f"),
    UUID("5cb0b8ed-45f4-4432-9ff7-3a9f896362f9"),
    UUID("15de1dcc-98c2-4463-85ed-b36a6a31445a"),
    UUID("e1e39dd5-1ec0-4f9a-b765-d6dc25f0d9a7")
]

const customerIds = [
    UUID("f89e1e75-d61c-4c51-b0be-c285500988cf"),
    UUID("e1509ec2-2b05-406f-befa-149f051586a9"),
    UUID("6d024627-568b-4d57-b477-2274c9d807b9"),
    UUID("ba045076-4837-47ab-80d5-546192851bab"),
    UUID("ba18cc29-c7ff-48c4-9b7b-456bcef231d0")
]

const paymentStatuses = [
    "Unpaid",
    "Pending",
    "Overdue",
    "PartiallyPaid",
    "Paid"
]

const currencies = ["USD", "EUR", "JPY", "INR", "GBP", "BYN", "PLN"]

function getRandomInt(max) {
    return Math.floor(Math.random() * max);
}

function randomDate(start, end) {
    return new Date(start.getTime() + Math.random() * (end.getTime() - start.getTime()));
}

function generateInvoiceNumber(index) {
    var currentDate = new Date();
    var year = currentDate.getFullYear() - getRandomInt(12);
    return "INV" + year + getRandomInt(30) + ("0000000" + (index + 1)).slice(-7);
}

function generateIssueDate() {
    return ISODate(randomDate(new Date("2010-01-01T00:00:00Z"), new Date()).toISOString());
}

function generateDueDate(issueDate) {
    return ISODate(new Date(issueDate.getTime() + (getRandomInt(80) + 10) * 86400000).toISOString());
}

function generateRandomItem(clientId, currency) {
    const itemNames = [
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

    const index = getRandomInt(itemNames.length);
    return {
        _id: UUID(), 
        ClientId: clientId,
        Name: itemNames[index],
        Description: descriptions[index],
        Price: NumberDecimal((Math.random() * 2000 + 100).toFixed(2)),
        Currency: currency
    };
}

function generateItems(clientId, currency) {
    const items = [];
    const count = getRandomInt(4) + 1;

    for (let i = 0; i < count; i++) {
        items.push(generateRandomItem(clientId, currency));
    }
    return items;
}

const invoices = [];
const invoiceCount = 20;

for (let i = 0; i < invoiceCount; i++) {
    const newClientId = clientIds[getRandomInt(clientIds.length)];
    const newCustomerId = customerIds[getRandomInt(customerIds.length)];
    const issueDate = generateIssueDate();
    const dueDate = generateDueDate(issueDate);
    const currency = currencies[getRandomInt(currencies.length)];

    const items = generateItems(newClientId, currency);

    let totalAmount = 0;
    items.forEach(function (item) {
        totalAmount += parseFloat(item.Price.toString());
    });

    invoices.push({
        _id: UUID(),
        ClientId: newClientId,
        CustomerId: newCustomerId,
        InvoiceNumber: generateInvoiceNumber(i),
        IssueDate: issueDate,
        DueDate: dueDate,
        Amount: totalAmount.toFixed(2),
        Currency: currency,
        PaymentStatus: paymentStatuses[getRandomInt(currencies.length)],
        BankAccountNumber: "5555555555555555555555",
        Items: items
    });
}

db.Invoice.insertMany(invoices);

db.MigrationHistory.insertOne({
    ScriptName: scriptName,
    Version: version,
    ScriptRunTime: new Date()
});

print("All invoices are inserted successfully!");