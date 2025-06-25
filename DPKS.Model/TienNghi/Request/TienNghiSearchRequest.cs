using DPKS.Common.Result;

namespace DPKS.Model.TienNghi.Request
{
    public class TienNghiSearchRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}
