using Microsoft.AspNetCore.Mvc;
using pawpals.Services;
using System.Threading.Tasks;

namespace pawpals.Controllers
{
    public class ConnectionPageController : Controller
    {
        private readonly IConnectionService _connectionService;

        public ConnectionPageController(IConnectionService connectionService)
        {
            _connectionService = connectionService;
        }

        public async Task<IActionResult> Index(int memberId)
        {
            var connections = await _connectionService.GetConnectionsAsync(memberId);
            return View(connections);
        }

        public async Task<IActionResult> Followers(int memberId)
        {
            var followers = await _connectionService.GetFollowersAsync(memberId);
            return View(followers);
        }

        public async Task<IActionResult> Following(int memberId)
        {
            var following = await _connectionService.GetFollowingAsync(memberId);
            return View(following);
        }

        [HttpPost]
        public async Task<IActionResult> Follow(int memberId, int followingId)
        {
            await _connectionService.FollowUserAsync(memberId, followingId);
            return RedirectToAction(nameof(Following), new { memberId });
        }

        [HttpPost]
        public async Task<IActionResult> Unfollow(int memberId, int followingId)
        {
            await _connectionService.UnfollowUserAsync(memberId, followingId);
            return RedirectToAction(nameof(Following), new { memberId });
        }
    }
}
