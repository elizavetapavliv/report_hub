using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Exadel.ReportHub.Data.Models
{
    public class Invoices
    {
        [BsonId]
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public Guid InvoiceId { get; set; } = Guid.NewGuid();

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime IssueDate { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime DueDate { get; set; }

        [BsonRepresentation(BsonType.Double)]
        public decimal Amount { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string Currency { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string PaymentStatus { get; set; }
    }
}
