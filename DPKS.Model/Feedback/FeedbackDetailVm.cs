using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Model.Feedback
{
    public class FeedbackDetailVm
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DatPhongId { get; set; }
        public int DanhGia { get; set; }
        public string BinhLuan { get; set; }
        public DateTime CreateAt { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }
        public DateTime NgayNhanPhong { get; set; }
        public DateTime NgayTraPhong { get; set; }
        public decimal TongTien { get; set; }
    }
}
