using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Common.Extensions
{
    public static class StringExtensions
    {
        // Method để decode ID từ encoded string
        public static int DecodeId(this string encodedId)
        {
            if (string.IsNullOrWhiteSpace(encodedId))
                return 0;

            try
            {
                // Implement your decode logic here
                // This is just an example - adjust based on your EncodeId1() method
                var bytes = Convert.FromBase64String(encodedId);
                var decodedString = Encoding.UTF8.GetString(bytes);
                return int.Parse(decodedString);
            }
            catch
            {
                return 0;
            }
        }
    }
}
