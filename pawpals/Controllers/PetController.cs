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

        /// <summary>
        /// Gets a list of all pets in the system
        /// </summary>
        /// <example>
        /// GET api/Pet/ListPets -> [{"petId":1,"name":"Max","type":"Dog","breed":"Labrador","dob":"2020-01-01T00:00:00","ownerId":1}]
        /// </example>
        /// <returns>
        /// List of pets with their basic information
        /// </returns>
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

        /// <summary>
        /// Gets the owners information for a specific pet
        /// </summary>
        /// <param name="petId">The ID of the pet</param>
        /// <example>
        /// GET api/Pets/Owners/1 -> [{"memberId":1,"memberName":"John Doe","email":"john@example.com"}]
        /// </example>
        /// <returns>
        /// List of member information who own the specified pet
        /// </returns>
        /// <response code="404">If pet with given ID is not found</response>
        [HttpGet("/api/Pet/Owners/{petId}")]
        public async Task<ActionResult<List<MemberDTO>>> GetPetOwners(int petId)
        {
            var owners = await _context.Pets
                .Where(p => p.PetId == petId)
                .Include(p => p.Owner)
                .Select(p => new MemberDTO
                {
                    MemberId = p.Owner.MemberId,
                    MemberName = p.Owner.MemberName,
                    Email = p.Owner.Email
                })
                .ToListAsync();

            return Ok(owners);
        }

        /// <summary>
        /// Adds a new pet to the system
        /// </summary>
        /// <param name="petDto">The pet information to add</param>
        /// <example>
        /// POST api/Pet/AddPet 
        /// Request body: {"name":"Max","type":"Dog","breed":"Labrador","dob":"2020-01-01T00:00:00","ownerId":1}
        /// </example>
        /// <returns>
        /// The newly created pet information
        /// </returns>
        /// <response code="201">Returns the newly created pet</response>
        /// <response code="400">If the pet data is invalid</response>
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

        /// <summary>
        /// Updates an existing pet's information
        /// </summary>
        /// <param name="id">The ID of the pet to update</param>
        /// <param name="petDto">The updated pet information</param>
        /// <example>
        /// PUT api/Pet/UpdatePet/5
        /// Request body: {"name":"Max Updated","type":"Dog","breed":"Labrador","dob":"2020-01-01T00:00:00","ownerId":1}
        /// </example>
        /// <returns>
        /// No content if the update is successful
        /// </returns>
        /// <response code="204">Update successful</response>
        /// <response code="404">If pet with given ID is not found</response>
        /// <response code="400">If the update data is invalid</response>
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

        /// <summary>
        /// Deletes a specific pet from the system
        /// </summary>
        /// <param name="id">The ID of the pet to delete</param>
        /// <example>
        /// DELETE api/Pet/DeletePet/5
        /// </example>
        /// <returns>
        /// No content if the deletion is successful
        /// </returns>
        /// <response code="204">Delete successful</response>
        /// <response code="404">If pet with given ID is not found</response>
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
