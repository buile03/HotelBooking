using DPKS.Common.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Model.User.Request
{
    public class UserSearchRequest : PagingRequestBase
    {
        public string Keyword {  get; set; }

    }
}
