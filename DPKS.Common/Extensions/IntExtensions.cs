using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Common.Extensions
{
    public static class IntExtensions
    {
        public static string EncodeId(this int id)
        {
            var bytes = Encoding.UTF8.GetBytes(id.ToString());
            return Convert.ToBase64String(bytes);
        }
    }
}
