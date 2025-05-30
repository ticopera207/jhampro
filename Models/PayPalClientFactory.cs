using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PayPalCheckoutSdk.Core;
using PayPalHttp;
using Microsoft.Extensions.Configuration;

namespace jhampro.Models
{
    public class PayPalClientFactory
    {
        private readonly IConfiguration _configuration;

        public PayPalClientFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public PayPalHttpClient CreateClient()
        {
            var environment = new SandboxEnvironment(
                _configuration["PayPal:ClientId"],
                _configuration["PayPal:ClientSecret"]
            );
            return new PayPalHttpClient(environment);
        }

    }
}