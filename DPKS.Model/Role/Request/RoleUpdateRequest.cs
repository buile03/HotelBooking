using DPKS.Common.Result;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Model.Role.Request
{
    public class RoleUpdateRequest : UpdateRequestBase
    {
        [Required(ErrorMessage ="Vui lòng nhập tên vai trò: ")]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
