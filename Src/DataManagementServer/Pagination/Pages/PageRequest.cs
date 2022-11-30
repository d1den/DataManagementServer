namespace Pagination.Pages
{
    public class PageRequest
    {
        public int PageSize { get; set; } = int.MaxValue;
        public int PageNumber { get; set; } = 1;

        public PageRequest() { }
        public PageRequest(int pageSize, int pageNumber) {
            PageSize = pageSize;
            PageNumber = pageNumber;
        }
    }
}