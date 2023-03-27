using FluentAssertions;
using Microsoft.Playwright;
using PlaywrightTests.APITesting.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlaywrightTests.APITesting.Swagger
{
    public class GetTokenClass
    {
        public static async Task<string> GetToken()
        {
            var playwright = await Playwright.CreateAsync();

            var requestContext = await playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions()
            {
                BaseURL = "https://localhost:5001",
                IgnoreHTTPSErrors = true,

            });

            var response = await requestContext.PostAsync("/api/Authenticate/Login", new APIRequestContextOptions()
            {
                DataObject = new
                {
                    userName = "Shubham",
                    password = "123456"
                }

            });
            var data = await response.JsonAsync();

            Console.WriteLine(data);
            Console.WriteLine(response.StatusText);

            var deserializeAuth = data.Value.Deserialize<Authenticate>(new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Console.WriteLine("Token = " + deserializeAuth.Token);
            deserializeAuth.Should().NotBeNull(string.Empty);

            return deserializeAuth.Token;
        }

    }
}
