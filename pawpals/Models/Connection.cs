using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pawpals.Models
{

  public class Connection
  {
      [Key]
      public int ConnectionId { get; set; }

      public int FollowerId { get; set; }

      public int FollowingId { get; set; }

      [ForeignKey("FollowerId")]
      public Member? Follower { get; set; }

      [ForeignKey("FollowingId")]
      public Member? Following { get; set; }
        
  }

}


