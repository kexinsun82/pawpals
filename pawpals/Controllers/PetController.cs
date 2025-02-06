using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pawpals.Data;
using pawpals.Models;
using pawpals.Models.DTOs;

namespace pawpals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PetController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Pet/FindPet/5
        [HttpGet("FindPet/{id}")]
        public async Task<ActionResult<PetDTO>> GetPet(int id)
        {
            var pet = await _context.Pets
                .Include(p => p.Owner) // 加载 Owner 数据
                .FirstOrDefaultAsync(p => p.PetId == id);

            if (pet == null)
            {
                return NotFound();
            }

            // 映射到 PetDTO
            var petDto = new PetDTO
            {
                PetId = pet.PetId,
                Name = pet.Name,
                Type = pet.Type,
                Breed = pet.Breed,
                DOB = pet.DOB,
                OwnerId = pet.OwnerId // 只返回 OwnerId
            };

            return petDto;
        }

        // GET: api/Pet/ListPets
        [HttpGet("ListPets")]
        public async Task<ActionResult<IEnumerable<PetDTO>>> GetPets()
        {
            var pets = await _context.Pets
                .Include(p => p.Owner) // 加载 Owner 数据
                .Select(p => new PetDTO
                {
                    PetId = p.PetId,
                    Name = p.Name,
                    Type = p.Type,
                    Breed = p.Breed,
                    DOB = p.DOB,
                    OwnerId = p.OwnerId // 只返回 OwnerId
                })
                .ToListAsync();

            return pets;
        }

        [HttpPost("AddPet")]
        public async Task<ActionResult<Pet>> PostPet(PetDTO petDto)
        {
            var pet = new Pet
            {
                Name = petDto.Name,
                Type = petDto.Type,
                Breed = petDto.Breed,
                DOB = petDto.DOB,
                OwnerId = petDto.OwnerId // 只存 OwnerId
            };

            _context.Pets.Add(pet);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPet), new { id = pet.PetId }, pet);
        }

        // PUT: api/Pet/UpdatePet/5
        [HttpPut("UpdatePet/{id}")]
        public async Task<IActionResult> PutPet(int id, PetDTO petDto)
        {
            var pet = await _context.Pets.FindAsync(id);
            if (pet == null)
            {
                return NotFound();
            }

            // 更新宠物信息
            pet.Name = petDto.Name;
            pet.Type = petDto.Type;
            pet.Breed = petDto.Breed;
            pet.DOB = petDto.DOB;
            pet.OwnerId = petDto.OwnerId; // 只存 OwnerId

            _context.Entry(pet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PetExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Pet/DeletePet/5
        [HttpDelete("DeletePet/{id}")]
        public async Task<IActionResult> DeletePet(int id)
        {
            var pet = await _context.Pets.FindAsync(id);
            if (pet == null)
            {
                return NotFound();
            }

            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PetExists(int id)
        {
            return _context.Pets.Any(e => e.PetId == id);
        }
    }
}
