using pawpals.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pawpals.Services
{
    public interface IPetService
    {
        Task<List<PetDTO>> GetAllPetsAsync();
        Task<PetDTO?> GetPetByIdAsync(int id);
        Task<bool> AddPetAsync(PetDTO petDto);
        Task<bool> UpdatePetAsync(PetDTO petDto);
        Task<bool> DeletePetAsync(int id);
    }
}
