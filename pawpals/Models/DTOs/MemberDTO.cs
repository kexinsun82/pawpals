using System;

namespace pawpals.Models.DTOs
{
    public class MemberDTO
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public string Location { get; set; }

        // 使用 connectionId 代替完整的 Member 对象
        public List<int> FollowerConnectionIds { get; set; } = new List<int>();
        public List<int> FollowingConnectionIds { get; set; } = new List<int>();
    }
}