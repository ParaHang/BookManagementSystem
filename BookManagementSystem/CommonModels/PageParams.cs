namespace BookManagementSystem.CommonModels
{
    public class PageParams
    {
        const int MaxPageSize = 20;
        public int PageNumber { get; set; } = 1;
        private int _pageSize { get; set; } = 5;
        public int PageSize 
        {
            get
            {
                return _pageSize;
            }
            set 
            {
                _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
            } 
        }
    }
}
