namespace BookInfo.API.Services
{
    public class MetaData
    {
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public MetaData(int totalItems,int pageSize
            ,int currentPage) {
            TotalItems = totalItems;
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        }
    }
}
