using Microsoft.EntityFrameworkCore;
using pawpals.Data;
using pawpals.Models;
using pawpals.Models.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pawpals.Services
{
    public class ConnectionService : IConnectionService
    {
        private readonly ApplicationDbContext _context;

        public ConnectionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<BasicMemberDTO>> GetConnectionsAsync(int memberId)
        {
            return await _context.Connections
                .Where(c => c.FollowerId == memberId || c.FollowingId == memberId)
                .Include(c => c.Follower)
                .Include(c => c.Following)
                .Select(c => new BasicMemberDTO
                {
                    MemberId = c.Following.MemberId,
                    MemberName = c.Following.MemberName
                })
                .ToListAsync();
        }

        public async Task<List<BasicMemberDTO>> GetFollowersAsync(int memberId)
        {
            return await _context.Connections
                .Where(c => c.FollowingId == memberId)
                .Include(c => c.Follower)
                .Select(c => new BasicMemberDTO
                {
                    MemberId = c.Follower.MemberId,
                    MemberName = c.Follower.MemberName
                })
                .ToListAsync();
        }

        public async Task<List<BasicMemberDTO>> GetFollowingAsync(int memberId)
        {
            return await _context.Connections
                .Where(c => c.FollowerId == memberId)
                .Include(c => c.Following)
                .Select(c => new BasicMemberDTO
                {
                    MemberId = c.Following.MemberId,
                    MemberName = c.Following.MemberName
                })
                .ToListAsync();
        }

        public async Task<bool> FollowUserAsync(int memberId, int followingId)
        {
            if (memberId == followingId || _context.Connections.Any(c => c.FollowerId == memberId && c.FollowingId == followingId))
                return false;

            _context.Connections.Add(new Connection { FollowerId = memberId, FollowingId = followingId });
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnfollowUserAsync(int memberId, int followingId)
        {
            var connection = await _context.Connections
                .FirstOrDefaultAsync(c => c.FollowerId == memberId && c.FollowingId == followingId);

            if (connection == null) return false;

            _context.Connections.Remove(connection);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
