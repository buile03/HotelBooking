using DPKS.Common.Result;

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Model.LoaiPhong.Request
{
    public class AddAnhLoaiPhongRequest : RequestBase
    {
        [Required(ErrorMessage = "Vui lòng chọn loại phòng")]
        public int LoaiPhongId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn file ảnh")]
        [ImageFileValidation]
        public List<IFormFile> ImageFile{ get; set; }

    }

}
