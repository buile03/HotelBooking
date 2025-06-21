using System.ComponentModel;

namespace DPKS.Common.Enum
{
    public enum enLoaiThanhToan
    {
        [Description("Tiền mặt")]
        TienMat = 1,

        [Description("Thanh toán bằng Stripe")]
        Stripe = 2,

        [Description("Thanh toán bằng Paypal")]
        PayPal = 3,

        [Description("Thanh toán bằng Momo")]
        Momo = 4,

        [Description("Thanh toán bằng VNPay")]
        VNPay = 5
    }
}
