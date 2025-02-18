using System;
using pawpals.Models.DTOs;

namespace pawpals.Models
{
    public class HomeViewModel
    {
        public MemberDTO Member { get; set; } = new MemberDTO();
        public List<PetDTO> Pets { get; set; } = new List<PetDTO>();
        public List<BasicMemberDTO> Friends { get; set; } = new List<BasicMemberDTO>();
        public List<BasicMemberDTO> RecommendedFriends { get; set; } = new List<BasicMemberDTO>();
        public List<ConnectionDTO> Connections { get; set; } = new List<ConnectionDTO>();
    }

}
