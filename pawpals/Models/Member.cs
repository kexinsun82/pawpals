using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pawpals.Models
{

  public class Member
  {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int MemberId { get; set; }

      public string? MemberName { get; set; }

      public string? Email { get; set; }

      // public string? Password { get; set; }

      public string? Bio { get; set; }

      public string? Location { get; set; }

      // Many to Many Relationship: Followers and Following
      public ICollection<Connection> Followers { get; set; } = new List<Connection>();
      public ICollection<Connection> Following { get; set; } = new List<Connection>();

      public ICollection<PetOwner> PetOwners { get; set; } = new List<PetOwner>();
        
  }

}


