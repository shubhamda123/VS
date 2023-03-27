using Microsoft.Playwright;

namespace PlaywrightTests
{
    public class Tests
    {
        public IPage page;
        public IBrowserContext context;

        [SetUp]
        public async Task Setup()
        {
            //playwright pachage
            var playwright = await Playwright.CreateAsync();

            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Channel = "chrome",
                Headless = false,
                Args = new[] { "--start-maximized" }
            });

             context = await browser.NewContextAsync(new BrowserNewContextOptions

            {
                IgnoreHTTPSErrors = true,
                ViewportSize = ViewportSize.NoViewport
            });

             page = await context.NewPageAsync();
            await page.GotoAsync("https://www.google.com/");
        }

        [Test]
        public async Task Test1()
        {
           
            await page.FillAsync("//input[@title='Search']","playwright");
            Thread.Sleep(2000);
            await page.ScreenshotAsync(new PageScreenshotOptions
            {
                Path = "googlescreshot.jpg"
            }); 

        }
    }
}