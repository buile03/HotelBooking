using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Common.System
{
    public class SystemConstants
    {
        public const string Alerts = "Alerts";
        public const string UrlBack = "UrlBack";
        public const string UrlIndex = "UrlIndex";
        public const string loginFailed = "loginFailed";

        public const int pageSize = 20;
        public const int pageIndex = 1;
        public class AppSettings
        {
            public const string Key = "App";
            public const string Token = "Token";
            public const string UniqueCode = "00.08.H57";
            public const string MainConnectionString = "ConnectionString";

            public const int ExpiresUtcMinutes = 30;
            public const string JWTSecurityKey = "xt7oidxpIRs9uDVnZEu9kZKqmumiF1e1RINb3UMlwCGA3O3Xywc8OZkOs4dmwf";
        }
        public class ProductSettings
        {
            public const int NumberOfFeaturedProducts = 4;
            public const int NumberOfLatestProducts = 6;
        }

        public class ProductConstants
        {
            public const string NA = "N/A";
        }
    }
}
