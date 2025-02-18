using Microsoft.AspNetCore.Mvc;
using pawpals.Models.DTOs;
using pawpals.Services;
using System.Threading.Tasks;

namespace pawpals.Controllers
{
    public class MemberPageController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberPageController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        public async Task<IActionResult> Index()
        {
            var members = await _memberService.GetAllMembersAsync();
            return View(members);
        }

        public async Task<IActionResult> Details(int id)
        {
            var member = await _memberService.GetMemberByIdAsync(id);
            if (member == null) return NotFound();
            return View(member);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MemberDTO memberDto)
        {
            if (!ModelState.IsValid) return View(memberDto);

            await _memberService.AddMemberAsync(memberDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var member = await _memberService.GetMemberByIdAsync(id);
            if (member == null) return NotFound();
            return View(member);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, MemberDTO memberDto)
        {
            if (id != memberDto.MemberId) return BadRequest();

            if (!ModelState.IsValid) return View(memberDto);

            await _memberService.UpdateMemberAsync(memberDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var member = await _memberService.GetMemberByIdAsync(id);
            if (member == null) return NotFound();
            return View(member);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _memberService.DeleteMemberAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
