using System.Text;

namespace DPKS.Common.Helper
{
    using global::System.Security.Cryptography;
    using global::System.Web;
    using Microsoft.Extensions.Configuration;

    namespace DPKS.Common.Helper
    {
        public class VnPayHelper
        {
            private readonly IConfiguration _configuration;
            public VnPayHelper(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public string CreatePaymentUrl(string orderId, decimal amount)
            {
                var vnp_Url = _configuration["VnPay:Url"];
                var vnp_ReturnUrl = _configuration["VnPay:ReturnUrl"];
                var vnp_TmnCode = _configuration["VnPay:TmnCode"];
                var vnp_HashSecret = _configuration["VnPay:HashSecret"];

                var vnpay = new SortedDictionary<string, string>();

                vnpay.Add("vnp_Version", "2.1.0");
                vnpay.Add("vnp_Command", "pay");
                vnpay.Add("vnp_TmnCode", vnp_TmnCode);
                vnpay.Add("vnp_Amount", ((long)(amount * 100)).ToString()); // nhân 100 để chuyển sang đơn vị nhỏ nhất
                vnpay.Add("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
                vnpay.Add("vnp_CurrCode", "VND");
                vnpay.Add("vnp_IpAddr", "127.0.0.1");
                vnpay.Add("vnp_Locale", "vn");
                vnpay.Add("vnp_OrderInfo", $"Thanh toán đơn đặt phòng #{orderId}");
                vnpay.Add("vnp_OrderType", "billpayment");
                vnpay.Add("vnp_ReturnUrl", vnp_ReturnUrl);
                vnpay.Add("vnp_TxnRef", orderId);

                string queryString = BuildQueryString(vnpay);
                string signData = BuildDataToSign(vnpay);
                string hash = ComputeHmacSHA512(signData, vnp_HashSecret);

                string paymentUrl = $"{vnp_Url}?{queryString}&vnp_SecureHash={hash}";

                return paymentUrl;
            }

            private string BuildQueryString(SortedDictionary<string, string> data)
            {
                var query = HttpUtility.ParseQueryString(string.Empty);
                foreach (var kv in data)
                {
                    query[kv.Key] = kv.Value;
                }
                return query.ToString();
            }

            private string BuildDataToSign(SortedDictionary<string, string> data)
            {
                var sb = new StringBuilder();
                foreach (var kv in data)
                {
                    sb.Append($"{kv.Key}={kv.Value}&");
                }
                sb.Length--; // remove last '&'
                return sb.ToString();
            }

            private string ComputeHmacSHA512(string data, string key)
            {
                var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key));
                var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }

}
