using System.ComponentModel.DataAnnotations;

namespace pawpals.Models
{

  public class User
  {
      [Key]
      public int UserId { get; set; }

      public string? Username { get; set; }

      public string? Email { get; set; }

      public string? PasswordHash { get; set; }

      public string? Bio { get; set; }

      public string? Location { get; set; }
        
  }

}


