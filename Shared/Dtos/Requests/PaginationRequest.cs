namespace Shared.Dtos.Requests
{
    public class PaginationRequest
    {
        public SearchFilterRequest Filter { get; set; }
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; }
    }
}
