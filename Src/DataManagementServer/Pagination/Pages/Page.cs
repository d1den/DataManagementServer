

using DataManagementServer.Pagination.Resources;

namespace Pagination
{
    public class Page<T>
    {
        public IList<T> Values { get; }
        public int PageSize { get; }
        public int PageNumber { get; }
        public int TotalElementsCount { get; }
        public int TotalPageCount { get { return (int) Math.Ceiling(TotalElementsCount / ((double) PageSize)); } }

        public bool IsOutOfRange { get { return PageNumber > TotalPageCount; } }

        public Page(IList<T> values, int pageSize, int pageNumber, int totalElementsCount)
        {

            if (pageNumber < 1)
            {
                throw new ArgumentException(ErrorMessages.IncorrectPageNumber);
            }
            if (pageSize < 1)
            {
                throw new ArgumentException(ErrorMessages.IncorrectPageSize);
            }
            if (totalElementsCount < 0)
            {
                throw new ArgumentException(ErrorMessages.IncorrectTotalElementsCount);
            }
            if (values.Count > pageSize)
            {
                throw new ArgumentException(ErrorMessages.ElementsMoreThanPageSize);
            }
            Values = values;
            PageSize = pageSize;
            PageNumber = pageNumber;
            TotalElementsCount = totalElementsCount;
        }
    }
}
