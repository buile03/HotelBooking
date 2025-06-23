using DPKS.Common.System;

namespace DPKS.Common.Helper
{
    public static class EncodeIdExtensions
    {
        public static string EncodeId1(this int id, string key = SystemConstants.AppSettings.Key)
        {
            return SystemHashUtil.EncodeID(id.ToString(), key);
        }

        public static int DecodeId1(this string encodedId, string key = SystemConstants.AppSettings.Key)
        {
            return Convert.ToInt32(SystemHashUtil.DecodeID(encodedId, key));
        }
    }
}
