using DPKS.Common.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Model.Feedback.Request
{
    public class SearchFeedbackRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
        public int? DanhGia { get; set; }
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }

    }
}
