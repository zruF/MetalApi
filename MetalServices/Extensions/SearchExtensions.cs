using MetalModels.Contracts;
using MetalModels.Models;
using Shared.Dtos.Requests;

namespace MetalServices.Extensions
{
    public static class SearchExtensions
    {
        public static IQueryable<IEntity> SetQueryFilter(this IQueryable<IEntity> query, SearchFilterRequest request)
        {
            if(request.Name != null)
            {
                query = query.Where(q => q.Name.Contains(request.Name));
            }

            return query;
        }
    }
}
