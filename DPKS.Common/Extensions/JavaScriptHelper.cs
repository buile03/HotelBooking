using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Common.Extensions
{
    public static class JavaScriptHelper
    {
        public static string GetDecodedId(string encodedId)
        {
            return encodedId.DecodeId().ToString();
        }
    }
}
