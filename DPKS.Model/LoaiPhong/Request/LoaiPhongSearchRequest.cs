using DPKS.Common.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Model.LoaiPhong.Request
{
    public class LoaiPhongSearchRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}
