using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DPKS.Common.Helper
{
    public class MoMoHelper
    {
        private readonly IConfiguration _configuration;
        public MoMoHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> CreatePaymentUrl(decimal amount, string orderId)
        {
            var endpoint = _configuration["MoMo:Endpoint"];
            var partnerCode = _configuration["MoMo:PartnerCode"];
            var accessKey = _configuration["MoMo:AccessKey"];
            var secretKey = _configuration["MoMo:SecretKey"];
            var returnUrl = _configuration["MoMo:ReturnUrl"];
            var notifyUrl = _configuration["MoMo:NotifyUrl"];

            string orderInfo = $"Thanh toán đơn đặt phòng #{orderId}";
            string requestId = Guid.NewGuid().ToString();
            string requestType = "captureWallet";
            string extraData = "";

            string rawHash = $"accessKey={accessKey}&amount={amount}&extraData={extraData}&ipnUrl={notifyUrl}&orderId={orderId}&orderInfo={orderInfo}&partnerCode={partnerCode}&redirectUrl={returnUrl}&requestId={requestId}&requestType={requestType}";
            string signature = SignSHA256(rawHash, secretKey);

            var body = new
            {
                partnerCode,
                accessKey,
                requestId,
                amount = amount.ToString(),
                orderId,
                orderInfo,
                redirectUrl = returnUrl,
                ipnUrl = notifyUrl,
                extraData,
                requestType,
                signature,
                lang = "vi"
            };

            var client = new HttpClient();
            var response = await client.PostAsync(endpoint, new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json"));
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine("MoMo response: " + responseBody);

            using var doc = JsonDocument.Parse(responseBody);
            if (doc.RootElement.TryGetProperty("payUrl", out JsonElement payUrlElement))
            {
                return payUrlElement.GetString();
            }
            else
            {
                throw new Exception($"Không tìm thấy 'payUrl' trong phản hồi MoMo. Nội dung phản hồi: {responseBody}");
            }
            //var payUrl = doc.RootElement.GetProperty("payUrl").GetString();
            //return payUrl;
        }

        private string SignSHA256(string rawData, string key)
        {
            var encoding = new UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(key);
            byte[] messageBytes = encoding.GetBytes(rawData);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return BitConverter.ToString(hashmessage).Replace("-", "").ToLower();
            }
        }
    }
}
