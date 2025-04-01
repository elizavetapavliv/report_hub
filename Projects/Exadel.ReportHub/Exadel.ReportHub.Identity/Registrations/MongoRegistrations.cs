using Exadel.ReportHub.RA;
using Exadel.ReportHub.RA.Abstract;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;

namespace Exadel.ReportHub.Identity.Registrations;

public static class MongoRegistrations
{
    public static void AddMongo(this IServiceCollection services)
    {
        services.AddSingleton<MongoDbContext>();
        ConventionRegistry.Register("IgnoreExtraElements", new ConventionPack 
        { 
            new IgnoreExtraElementsConvention(true) 
        }, _ => true);
        services.AddSingleton(typeof(IIdentityRepository), typeof(IdentityRepository));
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
    }
}