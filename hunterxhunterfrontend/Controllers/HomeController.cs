using hunterxhunterfrontend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Diagnostics;

namespace hunterxhunterfrontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _hxhApiUrl;


        public HomeController(ILogger<HomeController> logger, IOptions<hxhApiOptions> hxhoptions, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _hxhApiUrl = hxhoptions.Value.Url;
        }
        public async Task<IActionResult> Index()
        {
            List<hxhmodel> hunterlist = new List<hxhmodel>();


            try
            {
                var client = _clientFactory.CreateClient();
                var response = await client.GetAsync(_hxhApiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    hunterlist = JsonConvert.DeserializeObject<List<hxhmodel>>(apiResponse);
                }
                else
                {
                    _logger.LogError($"Received non-success status code {response.StatusCode} from API.");
                    _logger.LogError("error", _hxhApiUrl);
                }
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e, $"Error fetching data from API: {e.Message}");
                _logger.LogError(e, _hxhApiUrl);

                // Consider returning a user-friendly error page/view to the user
                // return View("Error");
            }

            return View(hunterlist);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}