using System.ComponentModel.DataAnnotations;

namespace pawpals.Models
{

  public class Connection
  {
      [Key]
      public int ConnectionId { get; set; }

      public int FollowerId { get; set; }

      public Member? Follower { get; set; }

      public int FollowingId { get; set; }

      public Member? Following { get; set; }
        
  }

}


