using Microsoft.EntityFrameworkCore;
using pawpals.Data;
using pawpals.Models;
using pawpals.Models.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pawpals.Services
{
    public class PetService : IPetService
    {
        private readonly ApplicationDbContext _context;

        public PetService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PetDTO>> GetAllPetsAsync()
        {
            return await _context.Pets
                .Include(p => p.PetOwners)
                .Select(p => new PetDTO
                {
                    PetId = p.PetId,
                    Name = p.Name,
                    Type = p.Type,
                    Breed = p.Breed,
                    DOB = p.DOB,
                    OwnerIds = p.PetOwners.Select(po => po.OwnerId).ToList()
                })
                .ToListAsync();
        }

        public async Task<PetDTO?> GetPetByIdAsync(int id)
        {
            var pet = await _context.Pets.Include(p => p.PetOwners).FirstOrDefaultAsync(p => p.PetId == id);
            if (pet == null) return null;

            return new PetDTO
            {
                PetId = pet.PetId,
                Name = pet.Name,
                Type = pet.Type,
                Breed = pet.Breed,
                DOB = pet.DOB,
                OwnerIds = pet.PetOwners.Select(po => po.OwnerId).ToList()
            };
        }

        public async Task<bool> AddPetAsync(PetDTO petDto)
        {
            var pet = new Pet
            {
                Name = petDto.Name,
                Type = petDto.Type,
                Breed = petDto.Breed,
                DOB = petDto.DOB
            };

            _context.Pets.Add(pet);
            await _context.SaveChangesAsync();

            foreach (var ownerId in petDto.OwnerIds)
            {
                var owner = await _context.Members.FindAsync(ownerId);
                if (owner == null) return false;

                _context.PetOwners.Add(new PetOwner { PetId = pet.PetId, OwnerId = ownerId, Pet = pet, Owner = owner });
            }
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdatePetAsync(PetDTO petDto)
        {
            var pet = await _context.Pets.Include(p => p.PetOwners).FirstOrDefaultAsync(p => p.PetId == petDto.PetId);
            if (pet == null) return false;

            pet.Name = petDto.Name;
            pet.Type = petDto.Type;
            pet.Breed = petDto.Breed;
            pet.DOB = petDto.DOB;

            var currentOwners = pet.PetOwners.Select(po => po.OwnerId).ToList();
            var newOwners = petDto.OwnerIds;

            _context.PetOwners.RemoveRange(pet.PetOwners.Where(po => !newOwners.Contains(po.OwnerId)));
            foreach (var ownerId in newOwners.Except(currentOwners))
            {
                _context.PetOwners.Add(new PetOwner { PetId = pet.PetId, OwnerId = ownerId, Pet = pet, Owner = await _context.Members.FindAsync(ownerId) });
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePetAsync(int id)
        {
            var pet = await _context.Pets.Include(p => p.PetOwners).FirstOrDefaultAsync(p => p.PetId == id);
            if (pet == null) return false;

            _context.PetOwners.RemoveRange(pet.PetOwners);
            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
