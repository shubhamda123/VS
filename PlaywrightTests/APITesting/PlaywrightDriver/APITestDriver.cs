using FluentAssertions;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightTests.APITesting.PlaywrightDriver
{
    public class APITestDriver : PlaywightDriver
    {
        private readonly PlaywightDriver _playwightDriver;
        public APITestDriver(PlaywightDriver playwightDriver) 
        {
            _playwightDriver = playwightDriver;
        }

        [Test]
        public async Task GetMethod()
        {
            //response
            var response = await _playwightDriver.ApiRequestContext?.GetAsync("api/users?page=2")!;
            //json data
            var data = await response.JsonAsync();

            Console.WriteLine(data);

            Console.WriteLine(response.StatusText);
            Console.WriteLine(HttpStatusCode.OK);
            response.Status.Should().Be(200);
    
        }   
    }

}
