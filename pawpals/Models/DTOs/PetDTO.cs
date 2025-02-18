using System;

namespace pawpals.Models.DTOs{
  public class PetDTO
  {
      public int PetId { get; set; }
      public string? Name { get; set; }
      public string? Type { get; set; }
      public string? Breed { get; set; }
      public DateTime DOB { get; set; }
      // public int OwnerId { get; set; } // return ownerid
      public List<int> OwnerIds { get; set; } = new List<int>();
  }
}