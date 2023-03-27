using FluentAssertions;
using Microsoft.Playwright;
using PlaywrightTests.APITesting.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlaywrightTests.APITesting.Swagger
{
    public class Swagger5001
    {
        [Test]
        public async Task GetSwaggerKK()
        {
            var playwright = await Playwright.CreateAsync();

            var headers = new Dictionary<string, string>();
            headers.Add("Accept", "application/json");

            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE2NzkyNDQ5MTcsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjQ0MzcxIiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNzEifQ.gI_zCuVTdJIj1jfHrCpbPGYFZ4udMrypz8WGMVM3YI8";
            headers.Add("Authorization", "Bearer " + token);

            var requestContext = await playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions()
            {
                BaseURL = "https://localhost:5001",
                IgnoreHTTPSErrors = true,
                ExtraHTTPHeaders = headers
            });

            var response = await requestContext.GetAsync("/Product/GetProductById/1");
            var data = await response.JsonAsync();

            Console.WriteLine(data);

            Console.WriteLine(response.StatusText);
            Console.WriteLine(HttpStatusCode.OK);
            // Assert.That(response.StatusText, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task PostSwagger()
        {
            var playwright = await Playwright.CreateAsync();

            var headers = new Dictionary<string, string>();
            headers.Add("Accept", "application/json");

            var requestContext = await playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions()
            {
                BaseURL = "https://localhost:5001",
                IgnoreHTTPSErrors = true,
                ExtraHTTPHeaders = headers
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
        }

        [Test]
        public async Task PostAuthenticate()
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

            var authenticateResponse = data.Value.Deserialize<Authenticate>(new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Console.WriteLine("Token = " + authenticateResponse.Token);
            authenticateResponse.Token.Should().NotBe(string.Empty);

        }

        [Test]
        public async Task GetProductSwagger()
        {
            var token = await GetTokenClass.GetToken();
            var playwright = await Playwright.CreateAsync();

            var requestContext = await playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions()
            {
                BaseURL = "https://localhost:5001",
                IgnoreHTTPSErrors = true,
            });

            var response = await requestContext.GetAsync("/Product/GetProductById/1", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>
                {
                    {"Authorization", $"Bearer {token}" }
                }

            }) ;
            var data = await response.JsonAsync();

            Console.WriteLine(data);

            Console.WriteLine(response.StatusText);
            Console.WriteLine(HttpStatusCode.OK);
            // Assert.That(response.StatusText, Is.EqualTo(HttpStatusCode.OK));
        }


    }
}
