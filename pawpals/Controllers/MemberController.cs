using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pawpals.Data;
using pawpals.Models;
using Microsoft.EntityFrameworkCore;
using pawpals.Models.DTOs;

namespace pawpals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MemberController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Member/FindMember/{id}
        [HttpGet("FindMember/{id}")]
        public async Task<ActionResult<MemberDTO>> GetMember(int id)
        {
            var member = await _context.Members
                .Include(m => m.Followers)
                .Include(m => m.Following)
                .FirstOrDefaultAsync(m => m.MemberId == id);

            if (member == null)
            {
                return NotFound();
            }

            var memberDto = new MemberDTO
            {
                MemberId = member.MemberId,
                MemberName = member.MemberName,
                Email = member.Email,
                Bio = member.Bio,
                Location = member.Location,

                // 只返回 connectionId
                FollowerConnectionIds = member.Followers.Select(f => f.ConnectionId).ToList(),
                FollowingConnectionIds = member.Following.Select(f => f.ConnectionId).ToList()
            };

            return memberDto;
        }

        // GET: api/Member/ListMembers
        [HttpGet("ListMembers")]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetMembers()
        {
            var members = await _context.Members
                .Include(m => m.Followers)
                .Include(m => m.Following)
                .Select(m => new MemberDTO
                {
                    MemberId = m.MemberId,
                    MemberName = m.MemberName,
                    Email = m.Email,
                    Bio = m.Bio,
                    Location = m.Location,

                    // 获取所有 connectionId，而不是完整的 Member 对象
                    FollowerConnectionIds = m.Followers.Select(f => f.ConnectionId).ToList(),
                    FollowingConnectionIds = m.Following.Select(f => f.ConnectionId).ToList()
                })
                .ToListAsync();

            return members;
        }

        // POST: api/Member/AddMember
        [HttpPost("AddMember")]
        public async Task<ActionResult<MemberDTO>> PostMember(MemberDTO memberDto)
        {
            var member = new Member
            {
                MemberName = memberDto.MemberName,
                Email = memberDto.Email,
                Bio = memberDto.Bio,
                Location = memberDto.Location
            };

            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMember), new { id = member.MemberId }, memberDto);
        }

        // PUT: api/Member/UpdateMember/{id}
        [HttpPut("UpdateMember/{id}")]
        public async Task<IActionResult> PutMember(int id, MemberDTO memberDto)
        {
            if (id != memberDto.MemberId)
            {
                return BadRequest();
            }

            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            member.MemberName = memberDto.MemberName;
            member.Email = memberDto.Email;
            member.Bio = memberDto.Bio;
            member.Location = memberDto.Location;

            _context.Entry(member).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.MemberId == id);
        }

        // DELETE: api/Member/DeleteMember/{id}
        [HttpDelete("DeleteMember/{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}