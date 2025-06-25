using DPKS.Common.Result;
using System.ComponentModel.DataAnnotations;

namespace DPKS.Model.Role.Request
{
    public class RoleCreateRequest : RequestBase
    {
        [Required(ErrorMessage ="Vui lòng nhập tên vai trò: ")]
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
