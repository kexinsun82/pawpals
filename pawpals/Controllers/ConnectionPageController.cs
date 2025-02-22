using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pawpals.Data;
using pawpals.Models;
using System.Threading.Tasks;

namespace pawpals.Controllers
{
    public class ConnectionPageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConnectionPageController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var connections = await _context.Connections
                .Include(c => c.Follower)
                .Include(c => c.Following)
                .ToListAsync();

            return View(connections);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var connection = await _context.Connections
                .Include(c => c.Follower)
                .Include(c => c.Following)
                .FirstOrDefaultAsync(m => m.ConnectionId == id);

            if (connection == null)
            {
                return NotFound();
            }

            return View(connection);
        }
    }
}