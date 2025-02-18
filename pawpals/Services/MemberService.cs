using Microsoft.EntityFrameworkCore;
using pawpals.Data;
using pawpals.Models;
using pawpals.Models.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pawpals.Services
{
    public class MemberService : IMemberService
    {
        private readonly ApplicationDbContext _context;

        public MemberService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MemberDTO>> GetAllMembersAsync()
        {
            return await _context.Members
                .Select(m => new MemberDTO
                {
                    MemberId = m.MemberId,
                    MemberName = m.MemberName,
                    Email = m.Email,
                    Bio = m.Bio,
                    Location = m.Location
                })
                .ToListAsync();
        }

        public async Task<MemberDTO?> GetMemberByIdAsync(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null) return null;

            return new MemberDTO
            {
                MemberId = member.MemberId,
                MemberName = member.MemberName,
                Email = member.Email,
                Bio = member.Bio,
                Location = member.Location
            };
        }

        public async Task<bool> AddMemberAsync(MemberDTO memberDto)
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

            return true;
        }

        public async Task<bool> UpdateMemberAsync(MemberDTO memberDto)
        {
            var member = await _context.Members.FindAsync(memberDto.MemberId);
            if (member == null) return false;

            member.MemberName = memberDto.MemberName;
            member.Email = memberDto.Email;
            member.Bio = memberDto.Bio;
            member.Location = memberDto.Location;

            _context.Entry(member).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMemberAsync(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null) return false;

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
