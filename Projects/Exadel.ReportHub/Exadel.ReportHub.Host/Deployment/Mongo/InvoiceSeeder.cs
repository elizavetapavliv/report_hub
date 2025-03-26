using MongoDB.Bson;
using MongoDB.Driver;

namespace Exadel.ReportHub.Host.Deployment.Mongo;

public static class InvoiceSeeder
{
    private const string ScriptName = "SeedInvoices";
    private const string Version = "1.0";

    public static async Task ExecuteSeedScriptAsync(IMongoClient client, string nameDb)
    {
        var db = client.GetDatabase(nameDb);

        var migrationHistory = db.GetCollection<BsonDocument>("MigrationHistory");

        var filter = Builders<BsonDocument>.Filter.Eq("ScriptName", ScriptName)
                   & Builders<BsonDocument>.Filter.Eq("Version", Version);

        var existingMigration = await migrationHistory.Find(filter).FirstOrDefaultAsync();
        if (existingMigration is not null)
        {
            return;
        }

        var invoices = db.GetCollection<BsonDocument>("Invoices");

        var indexKeys = Builders<BsonDocument>.IndexKeys.Ascending("invoiceId");
        await invoices.Indexes.CreateOneAsync(
            new CreateIndexModel<BsonDocument>(indexKeys, new CreateIndexOptions { Unique = true }));

        var seedData = new List<BsonDocument>
        {
            new BsonDocument
            {
                { "invoiceId", "INV2025002" },
                { "issueDate", new DateTime(2025, 1, 20, 0, 0, 0, DateTimeKind.Utc) },
                { "dueDate", new DateTime(2025, 2, 20, 0, 0, 0, DateTimeKind.Utc) },
                { "amount", 500.00m },
                { "currency", "USD" },
                { "paymentStatus", "Unpaid" }
            },
            new BsonDocument
            {
                { "invoiceId", "INV2024001" },
                { "issueDate", new DateTime(2024, 4, 28, 0, 0, 0, DateTimeKind.Utc) },
                { "dueDate", new DateTime(2024, 6, 19, 0, 0, 0, DateTimeKind.Utc) },
                { "amount", 98917.28m },
                { "currency", "JPY" },
                { "paymentStatus", "Overdue" }
            },
            new BsonDocument
            {
                { "invoiceId", "INV2025005" },
                { "issueDate", new DateTime(2025, 8, 9, 0, 0, 0, DateTimeKind.Utc) },
                { "dueDate", new DateTime(2025, 9, 17, 0, 0, 0, DateTimeKind.Utc) },
                { "amount", 47418.14m },
                { "currency", "USD" },
                { "paymentStatus", "Unpaid" }
            },
            new BsonDocument
            {
                { "invoiceId", "INV2024002" },
                { "issueDate", new DateTime(2024, 8, 12, 0, 0, 0, DateTimeKind.Utc) },
                { "dueDate", new DateTime(2024, 9, 5, 0, 0, 0, DateTimeKind.Utc) },
                { "amount", 38510.38m },
                { "currency", "CAD" },
                { "paymentStatus", "Cancelled" }
            },
            new BsonDocument
            {
                { "invoiceId", "INV2024003" },
                { "issueDate", new DateTime(2024, 8, 20, 0, 0, 0, DateTimeKind.Utc) },
                { "dueDate", new DateTime(2024, 9, 17, 0, 0, 0, DateTimeKind.Utc) },
                { "amount", 17775.52m },
                { "currency", "INR" },
                { "paymentStatus", "Partially Paid" }
            },
            new BsonDocument
            {
                { "invoiceId", "INV2024004" },
                { "issueDate", new DateTime(2024, 10, 24, 0, 0, 0, DateTimeKind.Utc) },
                { "dueDate", new DateTime(2024, 12, 10, 0, 0, 0, DateTimeKind.Utc) },
                { "amount", 35625.06m },
                { "currency", "GBP" },
                { "paymentStatus", "Paid" }
            },
            new BsonDocument
            {
                { "invoiceId", "INV2024005" },
                { "issueDate", new DateTime(2024, 10, 25, 0, 0, 0, DateTimeKind.Utc) },
                { "dueDate", new DateTime(2024, 12, 18, 0, 0, 0, DateTimeKind.Utc) },
                { "amount", 80875.27m },
                { "currency", "CNY" },
                { "paymentStatus", "Pending" }
            },
            new BsonDocument
            {
                { "invoiceId", "INV2025010" },
                { "issueDate", new DateTime(2025, 9, 21, 0, 0, 0, DateTimeKind.Utc) },
                { "dueDate", new DateTime(2025, 10, 15, 0, 0, 0, DateTimeKind.Utc) },
                { "amount", 7024.28m },
                { "currency", "AUD" },
                { "paymentStatus", "Pending" }
            },
            new BsonDocument
            {
                { "invoiceId", "INV2024006" },
                { "issueDate", new DateTime(2024, 4, 3, 0, 0, 0, DateTimeKind.Utc) },
                { "dueDate", new DateTime(2024, 4, 19, 0, 0, 0, DateTimeKind.Utc) },
                { "amount", 30632.44m },
                { "currency", "EUR" },
                { "paymentStatus", "Pending" }
            },
            new BsonDocument
            {
                { "invoiceId", "INV2025012" },
                { "issueDate", new DateTime(2025, 10, 24, 0, 0, 0, DateTimeKind.Utc) },
                { "dueDate", new DateTime(2025, 11, 4, 0, 0, 0, DateTimeKind.Utc) },
                { "amount", 61696.31m },
                { "currency", "INR" },
                { "paymentStatus", "Cancelled" }
            },
            new BsonDocument
            {
                { "invoiceId", "INV2025013" },
                { "issueDate", new DateTime(2025, 1, 7, 0, 0, 0, DateTimeKind.Utc) },
                { "dueDate", new DateTime(2025, 1, 26, 0, 0, 0, DateTimeKind.Utc) },
                { "amount", 3216.18m },
                { "currency", "AUD" },
                { "paymentStatus", "Partially Paid" }
            },
            new BsonDocument
            {
                { "invoiceId", "INV2025015" },
                { "issueDate", new DateTime(2025, 12, 20, 0, 0, 0, DateTimeKind.Utc) },
                { "dueDate", new DateTime(2025, 12, 31, 0, 0, 0, DateTimeKind.Utc) },
                { "amount", 100000.00m },
                { "currency", "USD" },
                { "paymentStatus", "Paid" }
            },
            new BsonDocument
            {
                { "invoiceId", "INV2025014" },
                { "issueDate", new DateTime(2025, 7, 11, 0, 0, 0, DateTimeKind.Utc) },
                { "dueDate", new DateTime(2025, 7, 18, 0, 0, 0, DateTimeKind.Utc) },
                { "amount", 40805.19m },
                { "currency", "JPY" },
                { "paymentStatus", "Pending" }
            },
            new BsonDocument
            {
                { "invoiceId", "INV2025016" },
                { "issueDate", new DateTime(2025, 11, 10, 0, 0, 0, DateTimeKind.Utc) },
                { "dueDate", new DateTime(2025, 12, 31, 0, 0, 0, DateTimeKind.Utc) },
                { "amount", 44972.66m },
                { "currency", "AUD" },
                { "paymentStatus", "Cancelled" }
            },
            new BsonDocument
            {
                { "invoiceId", "INV2024007" },
                { "issueDate", new DateTime(2024, 7, 20, 0, 0, 0, DateTimeKind.Utc) },
                { "dueDate", new DateTime(2024, 8, 28, 0, 0, 0, DateTimeKind.Utc) },
                { "amount", 42622.83m },
                { "currency", "GBP" },
                { "paymentStatus", "Overdue" }
            },
            new BsonDocument
            {
                { "invoiceId", "INV2025018" },
                { "issueDate", new DateTime(2025, 8, 14, 0, 0, 0, DateTimeKind.Utc) },
                { "dueDate", new DateTime(2025, 8, 28, 0, 0, 0, DateTimeKind.Utc) },
                { "amount", 50.00m },
                { "currency", "USD" },
                { "paymentStatus", "Unpaid" }
            },
            new BsonDocument
            {
                { "invoiceId", "INV2025019" },
                { "issueDate", new DateTime(2025, 10, 5, 0, 0, 0, DateTimeKind.Utc) },
                { "dueDate", new DateTime(2025, 10, 26, 0, 0, 0, DateTimeKind.Utc) },
                { "amount", 79897.20m },
                { "currency", "CNY" },
                { "paymentStatus", "Paid" }
            },
            new BsonDocument
            {
                { "invoiceId", "INV2025020" },
                { "issueDate", new DateTime(2025, 6, 29, 0, 0, 0, DateTimeKind.Utc) },
                { "dueDate", new DateTime(2025, 7, 22, 0, 0, 0, DateTimeKind.Utc) },
                { "amount", 89556.04m },
                { "currency", "CNY" },
                { "paymentStatus", "Paid" }
            }
        };

        foreach (var invoice in seedData)
        {
            var invoiceId = invoice["invoiceId"].AsString;
            var invoiceFilter = Builders<BsonDocument>.Filter.Eq("invoiceId", invoiceId);

            var replaceOptions = new ReplaceOptions { IsUpsert = true };
            await invoices.ReplaceOneAsync(invoiceFilter, invoice, replaceOptions);
        }

        var migrationRecord = new BsonDocument
        {
            { "ScriptName", ScriptName },
            { "Version", Version },
            { "ScriptRunTime", DateTime.UtcNow }
        };

        await migrationHistory.InsertOneAsync(migrationRecord);
    }
}
