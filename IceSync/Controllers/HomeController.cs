using IceCreamCompanySync.HttpHandler;
using Microsoft.AspNetCore.Mvc;

namespace IceSync.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRequestHandler _requestHandler;

        public HomeController(IRequestHandler requestHandler, ILogger<HomeController> logger)
        {
            _logger = logger;
            _requestHandler = requestHandler;
        }

        public async Task<IActionResult> Index()
        {
            var result  = await _requestHandler.GetWorkflows();
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> RunWorkFlow(int id)
        {
            var result = await _requestHandler.RunWorkflow(id);
            return Ok(new { resposne = result });
        }
    }
}