using Exadel.ReportHub.Data.Abstract;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Exadel.ReportHub.RA.Abstract.Extensions;

public static class FilterDefinitionExtension
{
    public static FilterDefinition<TDocument> NotDeleted<TDocument>(this FilterDefinition<TDocument> filter)
    {
        if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TDocument)))
        {
            filter &= Builders<TDocument>.Filter.Eq(nameof(ISoftDeletable.IsDeleted), BsonValue.Create(false));
        }

        return filter;
    }

    public static FilterDefinition<TDocument> Active<TDocument>(this FilterDefinition<TDocument> filter)
    {
        if (typeof(IActivatable).IsAssignableFrom(typeof(TDocument)))
        {
            filter &= Builders<TDocument>.Filter.Eq(nameof(IActivatable.IsActive), BsonValue.Create(true));
        }

        return filter;
    }

    public static FilterDefinition<TDocument> WithSoftDeleteAndActive<TDocument>(
            this FilterDefinition<TDocument> filter)
    {
        return filter.NotDeleted().Active();
    }
}
