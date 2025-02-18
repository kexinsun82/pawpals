using Microsoft.EntityFrameworkCore;
using pawpals.Data;
using pawpals.Models;
using pawpals.Models.DTOs;
using System.Linq;
using System.Threading.Tasks;

namespace pawpals.Services
{
    public class HomeService : IHomeService
    {
        private readonly ApplicationDbContext _context;

        public HomeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<HomeViewModel> GetHomePageDataAsync(int userId)
        {
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
                .Where(m => m.MemberId != userId)
                .Select(m => new BasicMemberDTO
                {
                    MemberId = m.MemberId,
                    MemberName = m.MemberName
                })
                .ToListAsync();

            return new HomeViewModel
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
                    OwnerIds = _context.PetOwners
                        .Where(po => po.PetId == p.PetId)
                        .Select(po => po.OwnerId)
                        .ToList()
                }).ToList(),
                Friends = friends,
                RecommendedFriends = recommendedFriends
            };
        }
    }
}
