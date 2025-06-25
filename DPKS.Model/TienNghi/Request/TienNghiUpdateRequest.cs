using DPKS.Common.Result;
using System.ComponentModel.DataAnnotations;

namespace DPKS.Model.TienNghi.Request
{
    public class TienNghiUpdateRequest : UpdateRequestBase
    {

        [Required(ErrorMessage = "Vui lòng nhập tên tiện nghi")]
        [Display(Name = "Tên tiện nghi")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mô tả")]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Icon")]
        public string Icon { get; set; }

        
    }
}
