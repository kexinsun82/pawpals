using Microsoft.AspNetCore.Mvc;
using pawpals.Data;
using pawpals.Models;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using pawpals.Models.DTOs;

namespace pawpals.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            int userId = 1; // 假设当前用户 ID 为 1，实际应用中应从身份验证获取

            var member = await _context.Members.FindAsync(userId);
            var pets = await _context.Pets
                .Where(p => _context.PetOwners.Any(po => po.OwnerId == userId && po.PetId == p.PetId))
                .ToListAsync();
            var friends = await _context.Connections
                .Where(c => c.FollowerId == userId)
                .Include(c => c.Following)
                .Select(c => new BasicMemberDTO
                {
                    MemberId = c.Following.MemberId,
                    MemberName = c.Following.MemberName
                })
                .ToListAsync();
            var recommendedFriends = await _context.Members
                .Where(m => m.MemberId != userId) // 排除当前用户
                .Select(m => new BasicMemberDTO
                {
                    MemberId = m.MemberId,
                    MemberName = m.MemberName
                })
                .ToListAsync();

            var viewModel = new HomeViewModel
            {
                Member = member != null ? new MemberDTO
                {
                    MemberId = member.MemberId,
                    MemberName = member.MemberName,
                    Email = member.Email,
                    Bio = member.Bio
                } : new MemberDTO(),
                Pets = pets.Select(p => new PetDTO
                {
                    PetId = p.PetId,
                    Name = p.Name,
                    Type = p.Type,
                    Breed = p.Breed,
                    DOB = p.DOB,
                    OwnerIds = _context.PetOwners.Where(po => po.PetId == p.PetId).Select(po => po.OwnerId).ToList()
                }).ToList(),
                Friends = friends,
                RecommendedFriends = recommendedFriends
            };

            return View(viewModel);
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
