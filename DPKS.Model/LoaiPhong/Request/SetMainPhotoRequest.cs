using DPKS.Common.Result;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Model.LoaiPhong.Request
{
    public class SetMainPhotoRequest : RequestBase
    {
        [Required(ErrorMessage = "Vui lòng chọn loại phòng")]
        public int LoaiPhongId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ảnh")]
        public int PhotoId { get; set; }
    }
}
