const scriptName = "01_create_Invoice.js";
const version = NumberInt(1);

if (db.MigrationHistory.findOne({ scriptName, version })) {
    print(`${scriptName} v${version} is already applied`);
    quit();
}

db.createCollection("Invoice", {
    collation: {
        locale: "en"
    }
});

db.Invoice.insertMany([
    {
        _id: UUID(),
        invoiceId: "INV2025002",
        issueDate: ISODate("2025-01-20T00:00:00Z"),
        dueDate: ISODate("2025-02-20T00:00:00Z"),
        amount: NumberDecimal("500.00"),
        currency: "USD",
        paymentStatus: "Unpaid"
    },
    {
        _id: UUID(),
        invoiceId: "INV2024001",
        issueDate: ISODate("2024-04-28T00:00:00Z"),
        dueDate: ISODate("2024-06-19T00:00:00Z"),
        amount: NumberDecimal("98917.28"),
        currency: "JPY",
        paymentStatus: "Overdue"
    },
    {
        _id: UUID(),
        invoiceId: "INV2025005",
        issueDate: ISODate("2025-08-09T00:00:00Z"),
        dueDate: ISODate("2025-09-17T00:00:00Z"),
        amount: NumberDecimal("47418.14"),
        currency: "USD",
        paymentStatus: "Unpaid"
    },
    {
        _id: UUID(),
        invoiceId: "INV2024002",
        issueDate: ISODate("2024-08-12T00:00:00Z"),
        dueDate: ISODate("2024-09-05T00:00:00Z"),
        amount: NumberDecimal("38510.38"),
        currency: "CAD",
        paymentStatus: "Cancelled"
    },
    {
        _id: UUID(),
        invoiceId: "INV2024003",
        issueDate: ISODate("2024-08-20T00:00:00Z"),
        dueDate: ISODate("2024-09-17T00:00:00Z"),
        amount: NumberDecimal("17775.52"),
        currency: "INR",
        paymentStatus: "Partially Paid"
    },
    {
        _id: UUID(),
        invoiceId: "INV2024004",
        issueDate: ISODate("2024-10-24T00:00:00Z"),
        dueDate: ISODate("2024-12-10T00:00:00Z"),
        amount: NumberDecimal("35625.06"),
        currency: "GBP",
        paymentStatus: "Paid"
    },
    {
        _id: UUID(),
        invoiceId: "INV2024005",
        issueDate: ISODate("2024-10-25T00:00:00Z"),
        dueDate: ISODate("2024-12-18T00:00:00Z"),
        amount: NumberDecimal("80875.27"),
        currency: "CNY",
        paymentStatus: "Pending"
    },
    {
        _id: UUID(),
        invoiceId: "INV2025010",
        issueDate: ISODate("2025-09-21T00:00:00Z"),
        dueDate: ISODate("2025-10-15T00:00:00Z"),
        amount: NumberDecimal("7024.28"),
        currency: "AUD",
        paymentStatus: "Pending"
    },
    {
        _id: UUID(),
        invoiceId: "INV2024006",
        issueDate: ISODate("2024-04-03T00:00:00Z"),
        dueDate: ISODate("2024-04-19T00:00:00Z"),
        amount: NumberDecimal("30632.44"),
        currency: "EUR",
        paymentStatus: "Pending"
    },
    {
        _id: UUID(),
        invoiceId: "INV2025012",
        issueDate: ISODate("2025-10-24T00:00:00Z"),
        dueDate: ISODate("2025-11-04T00:00:00Z"),
        amount: NumberDecimal("61696.31"),
        currency: "INR",
        paymentStatus: "Cancelled"
    },
    {
        _id: UUID(),
        invoiceId: "INV2025013",
        issueDate: ISODate("2025-01-07T00:00:00Z"),
        dueDate: ISODate("2025-01-26T00:00:00Z"),
        amount: NumberDecimal("3216.18"),
        currency: "AUD",
        paymentStatus: "Partially Paid"
    },
    {
        _id: UUID(),
        invoiceId: "INV2025015",
        issueDate: ISODate("2025-12-20T00:00:00Z"),
        dueDate: ISODate("2025-12-31T00:00:00Z"),
        amount: NumberDecimal("100000.00"),
        currency: "USD",
        paymentStatus: "Paid"
    },
    {
        _id: UUID(),
        invoiceId: "INV2025014",
        issueDate: ISODate("2025-07-11T00:00:00Z"),
        dueDate: ISODate("2025-07-18T00:00:00Z"),
        amount: NumberDecimal("40805.19"),
        currency: "JPY",
        paymentStatus: "Pending"
    },
    {
        _id: UUID(),
        invoiceId: "INV2025016",
        issueDate: ISODate("2025-11-10T00:00:00Z"),
        dueDate: ISODate("2025-12-31T00:00:00Z"),
        amount: NumberDecimal("44972.66"),
        currency: "AUD",
        paymentStatus: "Cancelled"
    },
    {
        _id: UUID(),
        invoiceId: "INV2024007",
        issueDate: ISODate("2024-07-20T00:00:00Z"),
        dueDate: ISODate("2024-08-28T00:00:00Z"),
        amount: NumberDecimal("42622.83"),
        currency: "GBP",
        paymentStatus: "Overdue"
    },
    {
        _id: UUID(),
        invoiceId: "INV2025018",
        issueDate: ISODate("2025-08-14T00:00:00Z"),
        dueDate: ISODate("2025-08-28T00:00:00Z"),
        amount: NumberDecimal("50.00"),
        currency: "USD",
        paymentStatus: "Unpaid"
    },
    {
        _id: UUID(),
        invoiceId: "INV2025019",
        issueDate: ISODate("2025-10-05T00:00:00Z"),
        dueDate: ISODate("2025-10-26T00:00:00Z"),
        amount: NumberDecimal("79897.20"),
        currency: "CNY",
        paymentStatus: "Paid"
    },
    {
        _id: UUID(),
        invoiceId: "INV2025020",
        issueDate: ISODate("2025-06-29T00:00:00Z"),
        dueDate: ISODate("2025-07-22T00:00:00Z"),
        amount: NumberDecimal("89556.04"),
        currency: "CNY",
        paymentStatus: "Paid"
    }
]);

db.MigrationHistory.insertOne({
    scriptName,
    version,
    scriptRunTime: new Date()
});

print("All invoices are inserted successfully!");