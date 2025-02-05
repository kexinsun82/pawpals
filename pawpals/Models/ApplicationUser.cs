// using Microsoft.AspNetCore.Identity;
// using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;
// using System.Collections.Generic;

// namespace pawpals.Models
// {
//     public class ApplicationUser : IdentityUser
//     {
//         [Key]
//         [Column(TypeName = "VARCHAR(255)")]
//         public override string Id { get; set; } = Guid.NewGuid().ToString();
//         public required string Bio { get; set; }
//         public required string Location { get; set; }
//         public required ICollection<Connection> Followers { get; set; }
//         public required ICollection<Connection> Following { get; set; }
//     }
// }