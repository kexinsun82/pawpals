using System.ComponentModel.DataAnnotations;

namespace pawpals.Models
{

  public class Pet
  {
      [Key]
      public int PetId { get; set; }

      public string? Name { get; set; }

      public string? Type { get; set; }
      public string? Breed { get; set; }

      public DateTime DOB { get; set; }

      public int OwnerId { get; set; }

      public Member? Owner { get; set; }
        
  }

}


