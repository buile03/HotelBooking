using DPKS.Common.Result;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Model.User.Request
{
    public class UserUpdateRequest : UpdateRequestBase
    {
        [Required]
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string PhoneNum { get; set; }

        public string GioiTinh { get; set; }
        public DateTime? Ngaysinh {  get; set; }
        public int QuocGiaId { get; set; }
        public int TinhId { get; set; }
        public IFormFile Avatar { get; set; }
    }
}
