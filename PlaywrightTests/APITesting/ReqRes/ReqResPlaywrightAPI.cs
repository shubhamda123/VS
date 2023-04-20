using FluentAssertions;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using Newtonsoft.Json;
using PlaywrightTests.APITesting.Property;
using System;
using System.Collections.Generic;

using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlaywrightTests.APITesting.ReqRes
{
    public class ReqResPlaywrightAPI
    {
        [Test]
        public async Task GetMethod()
        {
            Console.WriteLine("shubham Daa");
            var playwright = await Playwright.CreateAsync();

            var headers = new Dictionary<string, string>();
            headers.Add("Accept", "application/json");

            var requestContext = await playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions()
            {
                BaseURL = "https://reqres.in",
                IgnoreHTTPSErrors = true,
                ExtraHTTPHeaders = headers
            });

            //response
            var response = await requestContext.GetAsync("api/users?page=2");
            //json data
            var data = await response.JsonAsync();

            Console.WriteLine(data);

            Console.WriteLine(response.StatusText);
            Console.WriteLine(HttpStatusCode.OK);
            response.Status.Should().Be(200);

            //Page  Check expression for null
            var page = "";
            if (data != null)
            {
                page = data.Value.GetProperty("page").ToString();
                Console.WriteLine(page);
                page.Should().NotBe(string.Empty);  // assertion
            }

            //OR  USe conditional access (?)
            var page1 = data?.GetProperty("page").ToString();
            Console.WriteLine(page1);
            page.Should().NotBeNull();
            page.Should().NotBe(string.Empty);  // assertion

            //Email

            //json Deserializer
            var deserialize = data?.Deserialize<GetPropertyReqRes>();
            //OR -->  var deserialize =  data.Value.Deserialize<GetPropertyReqRes>();
            //OR --> // property name casesensitive
            /* var deserialize = data?.Deserialize<GetPropertyReqRes>(new JsonSerializerOptions
             {
                 PropertyNameCaseInsensitive = true, 
             });*/

            string fn = deserialize.data[2].first_name.ToString();
            Console.WriteLine(fn);
            Assert.That(fn, Is.EqualTo("Tobias"));  // assertion

            deserialize.data[2].first_name.Should().NotBeNull();

            deserialize.data[2].first_name.Should().Be("Tobias");
            Console.WriteLine("passed test");

        }


        [Test]
        public async Task PostMethod()
        {
            var playwright = await Playwright.CreateAsync();

            var headers = new Dictionary<string, string>();
            headers.Add("Accept", "application/json");

            var requestContext = await playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions()
            {
                BaseURL = "https://reqres.in",
                IgnoreHTTPSErrors = true,
                ExtraHTTPHeaders = headers
            });

            var response = await requestContext.PostAsync("/api/users?page=2", new APIRequestContextOptions()
            {
                DataObject = new
                {
                    name = "Shubham",
                    job = "Tester"
                }

            });
            var data = await response.JsonAsync();

            Console.WriteLine(data);
            Console.WriteLine(response.StatusText);
        }


        [Test]
        public async Task PostMethodJsonFile()
        {
            var playwright = await Playwright.CreateAsync();

            var headers = new Dictionary<string, string>();
            headers.Add("Accept", "application/json");

           // dynamic path = "C:\\Users\\HP\\source\\repos\\PlaywrightTests\\PlaywrightTests\\APITesting\\ReqRes\\JsonPost.json";
           dynamic postData= JsonConvert.DeserializeObject(File.ReadAllText("C:\\Users\\HP\\source\\repos\\PlaywrightTests\\PlaywrightTests\\APITesting\\ReqRes\\JsonPost.json"));
            
            dynamic jsonData = JsonConvert.SerializeObject(postData, Formatting.Indented);

            var requestContext = await playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions()
            {
                BaseURL = "https://reqres.in",
                IgnoreHTTPSErrors = true,
                //ExtraHTTPHeaders = headers
            });

            var response = await requestContext.PostAsync("/api/users?page=2", new APIRequestContextOptions()
            {
                DataObject = jsonData,
                Headers = headers

            });
            var data1 = await response.JsonAsync();

            Console.WriteLine(data1);
            Console.WriteLine(response.StatusText);


            //Without Deserialize
           var nam= data1?.GetProperty("name").ToString();
            Console.WriteLine(nam);
            nam.Should().NotBeNull();
            nam.Should().NotBe(string.Empty);

            //Deserialize
            var deserialize = data1?.Deserialize<PostProperty>();
            string name= deserialize.name.ToString();
            Assert.That(name, Is.EqualTo("Shubham"));
            Console.WriteLine("Post json File Passed");

        }
    }
}
