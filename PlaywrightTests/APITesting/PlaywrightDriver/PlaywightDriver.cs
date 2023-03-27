using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightTests.APITesting.PlaywrightDriver
{
    public class PlaywightDriver : IDisposable
    {
        private readonly Task<IAPIRequestContext?>? _requestContext = null;
        public PlaywightDriver() 
        {
            _requestContext = CreateAPIContext();
        }

        public IAPIRequestContext? ApiRequestContext => _requestContext?.GetAwaiter().GetResult();

        private  async Task<IAPIRequestContext?> CreateAPIContext()
        {
            var playwright = await Playwright.CreateAsync();

            var headers = new Dictionary<string, string>();
            headers.Add("Accept", "alication/json");

            return await playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions()
            {
                BaseURL = "https://reqres.in",
                IgnoreHTTPSErrors = true,
                ExtraHTTPHeaders = headers
            });
        }

        public void Dispose()
        {
            _requestContext?.Dispose();
        }
    }
}
