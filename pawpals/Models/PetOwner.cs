using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pawpals.Models
{
    public class PetOwner
    {
        [Key]
        public int PetOwnerId { get; set; }

        public int PetId { get; set; }
        public int OwnerId { get; set; }

        [ForeignKey("PetId")]
        public required Pet Pet { get; set; }

        [ForeignKey("OwnerId")]
        public required Member Owner { get; set; }
    }
}