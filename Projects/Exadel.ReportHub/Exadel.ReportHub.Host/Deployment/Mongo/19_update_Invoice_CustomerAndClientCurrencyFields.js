const scriptName = "19_update_Invoice_CustomerAndClientCurrencyFields";
const version = NumberInt(2);

if (db.MigrationHistory.findOne({ ScriptName: scriptName, Version: version })) {
    print(`${scriptName} v${version} is already applied`);;
    quit();
}

const clientIds = db.Invoice.distinct("ClientId");
const clients = db.Client.find({ _id: { $in: clientIds } }, { _id: 1, CurrencyId: 1, CurrencyCode: 1 }).toArray();

const dates = db.Invoice.aggregate([
    {
        $group: {
            _id: null,
            minDate: { $min: "$IssueDate" },
            maxDate: { $max: "$IssueDate" }
        }
    }
]).toArray()[0];

const exchangeRates = db.ExchangeRate.find({
    RateDate: {
        $gte: dates.minDate,
        $lte: dates.maxDate
    }
}).toArray();

db.ExchangeRate.createIndex({ RateDate: 1, Currency: 1 });

const invoices = db.Invoice.find({}).toArray();
const updates = invoices.map(invoice => {

    const client = clients.find(x => x._id.equals(invoice.ClientId));

    const clientCurrencyRates = exchangeRates.filter(x => x.Currency === client.CurrencyCode &&
        new Date(x.RateDate) <= new Date(invoice.IssueDate));

    const update = {
        CustomerCurrencyAmount: invoice.Amount,
        CustomerCurrencyId: invoice.CurrencyId,
        CustomerCurrencyCode: invoice.CurrencyCode,

        ClientCurrencyAmount: 0,
        ClientCurrencyId: client.CurrencyId,
        ClientCurrencyCode: client.CurrencyCode,
    };
    if (invoice.CurrencyCode === client.CurrencyCode) {
        update.ClientCurrencyAmount = invoice.Amount;
    }
    else {

        if (clientCurrencyRates.length > 0) {
            const closestRate = clientCurrencyRates.reduce((prev, curr) => {
                const prevDiff = Math.abs(new Date(invoice.IssueDate).getTime() - new Date(prev.RateDate).getTime());
                const currDiff = Math.abs(new Date(invoice.IssueDate).getTime() - new Date(curr.RateDate).getTime());
                return currDiff < prevDiff ? curr : prev;
            });

            let clientCurrencyAmount = invoice.Amount;
            const clientCurrencyCode = client.CurrencyCode;

            if (invoice.CurrencyCode !== clientCurrencyCode) {
                const clientRate = closestRate.Rate;
                clientCurrencyAmount = invoice.Amount * clientRate;
            }

            update.ClientCurrencyAmount = NumberDecimal(clientCurrencyAmount.toString());
        }
    }

    return {
        updateOne: {
            filter: { _id: invoice._id },
            update: {
                $set: update,
                $unset: {
                    Amount: "",
                    CurrencyId: "",
                    CurrencyCode: ""
                }
            }
        }
    };
});

if (updates.length > 0) {
    db.Invoice.bulkWrite(updates);
}

db.ExchangeRate.dropIndex({ RateDate: 1, Currency: 1 });

db.MigrationHistory.insertOne({
    ScriptName: scriptName,
    Version: version,
    ScriptRunTime: new Date()
});

print("Invoices updated with Client Currency and Customer Currency fields");