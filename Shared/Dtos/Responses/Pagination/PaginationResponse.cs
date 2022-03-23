using Shared.Dtos.Contracts;

namespace Shared.Dtos.Responses.Pagination
{
    public class PaginationResponse<T> where T : IEntityResponse
    {
        public IEnumerable<T> Entities { get; set; }
        public int Count { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }

        public PaginationResponse(IEnumerable<T> entity, int count, int totalCount, int pageSize, int pageIndex)
        {
            Entities = entity;
            Count = count;
            TotalCount = totalCount;
            PageSize = pageSize;
            PageIndex = pageIndex;
        }
    }
}
