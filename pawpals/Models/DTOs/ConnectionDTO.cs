using System;

namespace pawpals.Models.DTOs
{
    public class ConnectionDTO
    {
        public int ConnectionId { get; set; }
        public int FollowerId { get; set; }
        public int FollowingId { get; set; }
    }
}