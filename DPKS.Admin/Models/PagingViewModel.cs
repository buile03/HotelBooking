namespace DPKS.Admin.Models
{
    public class PagingViewModel
    {
        public int Total { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Keyword { get; set; }
    }
}
