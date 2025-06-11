using DPKS.Common.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Common.Result
{
    public class PagingRequestBase : RequestBase
    {
        public int PageIndex { get; set; } = SystemConstants.pageIndex;

        public int PageSize { get; set; } = SystemConstants.pageSize;
    }
    public class PagedResultBase
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }

        public int PageCount
        {
            get
            {
                var pageCount = (double)TotalRecords / PageSize;
                return (int)Math.Ceiling(pageCount);
            }
        }
    }
    public class PagedResult<T> : PagedResultBase
    {
        public string Keyword { get; set; } = String.Empty;
        public List<T> Items { set; get; }
    }
    public class GetPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; } = string.Empty;
    }
    public class PagingBase
    {
        public int PageIndex { get; set; } = SystemConstants.pageIndex;

        public int PageSize { get; set; } = SystemConstants.pageSize;
    }
    public class RequestBase
    {
        public Guid UserId { get; set; }

    }
    public class UpdateRequestBase
    {
        public string Id { get; set; }
        public Guid UserId { get; set; }

    }
    public class DeleteRequest : UpdateRequestBase
    {
        public string Title { get; set; }
        public string Caption { get; set; }
        public string Action { get; set; }
        public string ViewCallBack { get; set; }
    }
    public class UpdateOrderRequest : UpdateRequestBase
    {
        public int Value { get; set; }
    }
    public class UpdateStatusRequest : UpdateRequestBase
    {
    }

    public class ApiModel
    {
        public bool isSuccessed { get; set; }
        public string message { get; set; }
        public string validationErrors { get; set; }
    }
}
