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

        /// <summary>
        /// Gets a specific pet by ID
        /// </summary>
        /// <param name="id">The ID of the pet</param>
        /// <example>
        /// GET /api/Pet/FindPet/1
        /// </example>
        /// <returns>
        /// The pet information
        /// </returns>
        /// <response code="200">Returns the pet information</response>
        /// <response code="404">If the pet is not found</response>
        [HttpGet("FindPet/{id}")]
        public async Task<ActionResult<PetDTO>> GetPet(int id)
        {
            var pet = await _context.Pets
                .Include(p => p.PetOwners) // Load the PetOwners data
                .FirstOrDefaultAsync(p => p.PetId == id);

            if (pet == null)
            {
                return NotFound();
            }

            var petDto = new PetDTO
            {
                PetId = pet.PetId,
                Name = pet.Name,
                Type = pet.Type,
                Breed = pet.Breed,
                DOB = pet.DOB,
                OwnerIds = pet.PetOwners.Select(po => po.OwnerId).ToList()
            };

            return petDto;
        }

        /// <summary>
        /// Gets a list of all pets in the system
        /// </summary>
        /// <example>
        /// GET /api/Pet/ListPets
        /// </example>
        /// <returns>
        /// A list of pets
        /// </returns>
        /// <response code="200">Returns the list of pets</response>
        [HttpGet("ListPets")]
        public async Task<ActionResult<IEnumerable<PetDTO>>> GetPets()
        {
            var pets = await _context.Pets
                .Include(p => p.PetOwners) // Load the PetOwners data
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

            return pets;
        }

        /// <summary>
        /// Adds a new pet to the system and associates it with one or more owners
        /// </summary>
        /// <param name="petDto">The pet information to add, including the owner IDs</param>
        /// <example>
        /// POST api/Pet/AddPet 
        /// Request body: {"name":"Max","type":"Dog","breed":"Labrador","dob":"2020-01-01","ownerIds":[1, 2]}
        /// </example>
        /// <returns>
        /// The newly created pet information
        /// </returns>
        /// <response code="201">Returns the newly created pet</response>
        /// <response code="400">If the pet data is invalid or any owner does not exist</response>
        [HttpPost("AddPet")]
        public async Task<ActionResult<Pet>> PostPet(PetDTO petDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check the owner ids existing or not
            foreach (var ownerId in petDto.OwnerIds)
            {
                var owner = await _context.Members.FindAsync(ownerId);
                if (owner == null)
                {
                    return BadRequest($"Owner with ID {ownerId} not found");
                }
            }

            // Create/Add new pet
            var pet = new Pet
            {
                Name = petDto.Name,
                Type = petDto.Type,
                Breed = petDto.Breed,
                DOB = petDto.DOB
            };

            _context.Pets.Add(pet);
            await _context.SaveChangesAsync();

            // Link all owners
            foreach (var ownerId in petDto.OwnerIds)
            {
                var owner = await _context.Members.FindAsync(ownerId);
                if (owner == null)
                {
                    return BadRequest($"Owner with ID {ownerId} not found");
                }

                var petOwner = new PetOwner
                {
                    PetId = pet.PetId,
                    OwnerId = ownerId,
                    Pet = pet,
                    Owner = owner
                };
                _context.PetOwners.Add(petOwner);
            }

            // Return PetDTO
            var resultDto = new PetDTO
            {
                PetId = pet.PetId,
                Name = pet.Name,
                Type = pet.Type,
                Breed = pet.Breed,
                DOB = pet.DOB,
                OwnerIds = petDto.OwnerIds
            };

            return CreatedAtAction(nameof(GetPet), new { id = pet.PetId }, resultDto);
        }

        /// <summary>
        /// Updates an existing pet's information
        /// </summary>
        /// <param name="id">The ID of the pet to update</param>
        /// <param name="petDto">The updated pet information</param>
        /// <example>
        /// PUT /api/Pet/UpdatePet/1
        /// Request body: {"name":"Max Updated","type":"Dog","breed":"Labrador","dob":"2020-01-01"}
        /// </example>
        /// <returns>
        /// No content if the update is successful
        /// </returns>
        /// <response code="204">Update successful</response>
        /// <response code="400">If the update data is invalid</response>
        /// <response code="404">If the pet is not found</response>
        [HttpPut("UpdatePet/{id}")]
        public async Task<IActionResult> PutPet(int id, PetDTO petDto)
        {
            if (id != petDto.PetId)
            {
                return BadRequest("ID mismatch");
            }

            var pet = await _context.Pets
                .Include(p => p.PetOwners) // Load the PetOwners data
                .FirstOrDefaultAsync(p => p.PetId == id);

            if (pet == null)
            {
                return NotFound();
            }

            // Update Pet's information
            pet.Name = petDto.Name;
            pet.Type = petDto.Type;
            pet.Breed = petDto.Breed;
            pet.DOB = petDto.DOB;

            // Update Owner's information
            var currentOwnerIds = pet.PetOwners.Select(po => po.OwnerId).ToList();
            var newOwnerIds = petDto.OwnerIds;

            // Remove Owner
            var ownersToRemove = pet.PetOwners
                .Where(po => !newOwnerIds.Contains(po.OwnerId))
                .ToList();
            _context.PetOwners.RemoveRange(ownersToRemove);

            // Add new Owner
            var ownersToAdd = new List<PetOwner>();
            foreach (var ownerId in newOwnerIds.Where(id => !currentOwnerIds.Contains(id)))
            {
                var owner = await _context.Members.FindAsync(ownerId);
                if (owner == null)
                {
                    return BadRequest($"Owner with ID {ownerId} not found");
                }

                var petOwner = new PetOwner
                {
                    PetId = pet.PetId,
                    OwnerId = ownerId,
                    Pet = pet,
                    Owner = owner
                };
                ownersToAdd.Add(petOwner);
            }
            _context.PetOwners.AddRange(ownersToAdd);

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
            var pet = await _context.Pets
                .Include(p => p.PetOwners) 
                .FirstOrDefaultAsync(p => p.PetId == id);

            if (pet == null)
            {
                return NotFound();
            }

            // Delete PetOwner 
            _context.PetOwners.RemoveRange(pet.PetOwners);

            // Delete Pet
            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PetExists(int id)
        {
            return _context.Pets.Any(e => e.PetId == id);
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
            var owners = await _context.PetOwners
            .Where(po => po.PetId == petId)
            .Include(po => po.Owner)
            .Select(po => new MemberDTO
            {
                MemberId = po.Owner.MemberId,
                MemberName = po.Owner.MemberName,
                Email = po.Owner.Email,
                Bio = po.Owner.Bio, 
                Location = po.Owner.Location 
            })
            .ToListAsync();

            return Ok(owners);
        }
        
        /// <summary>
        /// Gets all pets owned by a specific member
        /// </summary>
        /// <param name="memberId">The ID of the member</param>
        /// <example>
        /// GET api/Pet/MemberPets/1 -> [{"petId":1,"name":"Max","type":"Dog","breed":"Labrador","dob":"2020-01-01"}]
        /// </example>
        /// <returns>
        /// List of pets owned by the specified member
        /// </returns>
        /// <response code="404">If member with given ID is not found</response>
        [HttpGet("MemberPets/{memberId}")]
        public async Task<ActionResult<List<PetDTO>>> GetMemberPets(int memberId)
        {
            var member = await _context.Members.FindAsync(memberId);
            if (member == null)
            {
                return NotFound("Member not found");
            }

            var pets = await _context.PetOwners
                .Where(po => po.OwnerId == memberId)
                .Include(po => po.Pet)
                .Select(po => new PetDTO
                {
                    PetId = po.Pet.PetId,
                    Name = po.Pet.Name,
                    Type = po.Pet.Type,
                    Breed = po.Pet.Breed,
                    DOB = po.Pet.DOB,
                    OwnerIds = po.Pet.PetOwners.Select(p => p.OwnerId).ToList()
                })
                .ToListAsync();

            return Ok(pets);
        }

        /// <summary>
        /// Adds an owner to a pet
        /// </summary>
        /// <param name="petId">The ID of the pet</param>
        /// <param name="memberId">The ID of the member to add as owner</param>
        /// <example>
        /// POST api/Pet/AddOwner/1/2
        /// </example>
        /// <returns>
        /// No content if the operation is successful
        /// </returns>
        /// <response code="204">Operation successful</response>
        /// <response code="404">If pet or member with given ID is not found</response>
        [HttpPost("AddOwner/{petId}/{memberId}")]
        public async Task<IActionResult> AddOwner(int petId, int memberId)
        {
            var pet = await _context.Pets.FindAsync(petId);
            if (pet == null)
            {
                return NotFound("Pet not found");
            }

            var member = await _context.Members.FindAsync(memberId);
            if (member == null)
            {
                return NotFound("Member not found");
            }

            // Check if the relationship already exists
            var existingRelation = await _context.PetOwners
                .AnyAsync(po => po.PetId == petId && po.OwnerId == memberId);

            if (existingRelation)
            {
                return BadRequest("This member is already an owner of the pet");
            }

            // Add new Owner
            var petOwner = new PetOwner
            {
                PetId = petId,
                OwnerId = memberId,
                Pet = pet,
                Owner = member
            };

            _context.PetOwners.Add(petOwner);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Removes an owner from a pet
        /// </summary>
        /// <param name="petId">The ID of the pet</param>
        /// <param name="memberId">The ID of the member to remove as owner</param>
        /// <example>
        /// DELETE api/Pet/RemoveOwner/1/2
        /// </example>
        /// <returns>
        /// No content if the operation is successful
        /// </returns>
        /// <response code="204">Operation successful</response>
        /// <response code="404">If pet or member with given ID is not found</response>
        [HttpDelete("RemoveOwner/{petId}/{memberId}")]
        public async Task<IActionResult> RemoveOwner(int petId, int memberId)
        {
            var petOwner = await _context.PetOwners
                .FirstOrDefaultAsync(po => po.PetId == petId && po.OwnerId == memberId);

            if (petOwner == null)
            {
                return NotFound("The specified owner is not associated with the pet");
            }

            _context.PetOwners.Remove(petOwner);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
