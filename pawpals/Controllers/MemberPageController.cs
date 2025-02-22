using Microsoft.AspNetCore.Mvc;
using pawpals.Models.DTOs;
using pawpals.Data;
using System.Threading.Tasks;
using pawpals.Models;

namespace pawpals.Controllers
{
    public class MemberPageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MemberPageController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var members = _context.Members.ToList();
            return View(members);
        }

        public IActionResult Details(int id)
        {
            var member = _context.Members.Find(id);
            if (member == null) return NotFound();
            return View(member);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(MemberDTO memberDto)
        {
            if (!ModelState.IsValid) return View(memberDto);

            var member = new Member
            {
                // Map properties from memberDto to member
            };
            _context.Members.Add(member);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var member = _context.Members.Find(id);
            if (member == null) return NotFound();
            return View(member);
        }

        [HttpPost]
        public IActionResult Edit(int id, MemberDTO memberDto)
        {
            if (id != memberDto.MemberId) return BadRequest();

            if (!ModelState.IsValid) return View(memberDto);

            var member = _context.Members.Find(id);
            if (member == null) return NotFound();

            // Map properties from memberDto to member

            _context.Members.Update(member);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var member = _context.Members.Find(id);
            if (member == null) return NotFound();
            return View(member);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var member = _context.Members.Find(id);
            if (member == null) return NotFound();

            _context.Members.Remove(member);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
