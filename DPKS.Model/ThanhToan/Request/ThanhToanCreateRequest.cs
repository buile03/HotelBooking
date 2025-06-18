using System;
using System.ComponentModel.DataAnnotations;

namespace DPKS.Model.ThanhToan.Request
{
    public class ThanhToanCreateRequest
    {
        public int DatPhongId { get; set; }

        [Display(Name = "Phương thức thanh toán")]
        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán")]
        public int PhuongThucThanhToanId { get; set; }

        [Display(Name = "Tổng tiền")]
        [Required]
        public decimal Gia { get; set; }

        public DateTime ThoiDiemThanhToan { get; set; }
    }
}
