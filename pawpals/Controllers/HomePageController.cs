using Microsoft.AspNetCore.Mvc;
using pawpals.Services;
using System.Threading.Tasks;

namespace pawpals.Controllers
{
    public class HomePageController : Controller
    {
        private readonly IHomeService _homeService;

        public HomePageController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = await _homeService.GetHomePageDataAsync(1); // 假设当前用户ID = 1
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
