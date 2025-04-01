using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.RA;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;

namespace Exadel.ReportHub.Identity.Registrations;

public static class MongoRegistrations
{
    public static IServiceCollection AddMongo(this IServiceCollection services)
    {
        services.AddSingleton<MongoDbContext>();
        ConventionRegistry.Register("EnumStringConvention", new ConventionPack
        {
            new EnumRepresentationConvention(BsonType.String)
        }, _ => true);
        services.AddSingleton(typeof(IIdentityRepository), typeof(IdentityRepository));
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        return services;
    }
}