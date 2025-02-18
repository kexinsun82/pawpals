using pawpals.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pawpals.Services
{
    public interface IConnectionService
    {
        Task<List<BasicMemberDTO>> GetConnectionsAsync(int memberId);
        Task<List<BasicMemberDTO>> GetFollowersAsync(int memberId);
        Task<List<BasicMemberDTO>> GetFollowingAsync(int memberId);
        Task<bool> FollowUserAsync(int memberId, int followingId);
        Task<bool> UnfollowUserAsync(int memberId, int followingId);
    }
}
