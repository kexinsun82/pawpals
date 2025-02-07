using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pawpals.Models
{

  public class Connection
  {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int ConnectionId { get; set; }

      [Required]
      public int FollowerId { get; set; }

      [Required]
      public int FollowingId { get; set; }

      [ForeignKey("FollowerId")]
      public Member? Follower { get; set; } = null!;

      [ForeignKey("FollowingId")]
      public Member? Following { get; set; } = null!;
        
  }

}


