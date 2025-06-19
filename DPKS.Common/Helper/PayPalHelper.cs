using Microsoft.Extensions.Configuration;
using PayPalCheckoutSdk.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Common.Helper
{
    public class PayPalHelper
    {
        private static string _clientId;
        private static string _secret;
        private static string _mode;

        public static void Configure(IConfiguration configuration)
        {
            _clientId = configuration["PayPal:ClientId"];
            _secret = configuration["PayPal:Secret"];
            _mode = configuration["PayPal:Mode"];
        }

        public static PayPalEnvironment Environment =>
            _mode == "live"
                ? new LiveEnvironment(_clientId, _secret)
                : new SandboxEnvironment(_clientId, _secret);

        public static PayPalHttpClient Client => new PayPalHttpClient(Environment);
    }
}
