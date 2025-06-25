using DPKS.Common.Result;

namespace DPKS.Model.TienNghi.Request
{
    public class TienNghiCreateRequest : RequestBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string Icon { get; set; }
    }
}
