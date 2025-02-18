using pawpals.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pawpals.Services
{
    public interface IMemberService
    {
        Task<List<MemberDTO>> GetAllMembersAsync();
        Task<MemberDTO?> GetMemberByIdAsync(int id);
        Task<bool> AddMemberAsync(MemberDTO memberDto);
        Task<bool> UpdateMemberAsync(MemberDTO memberDto);
        Task<bool> DeleteMemberAsync(int id);
    }
}
