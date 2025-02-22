using Microsoft.AspNetCore.Mvc;
using pawpals.Data;
using pawpals.Models;
using pawpals.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace pawpals.Controllers
{
    public class PetPageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PetPageController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var pets = await _context.Pets.ToListAsync();
            return RedirectToAction("List");
        }

        public async Task<IActionResult> List()
        {
            var petDTOs = await _context.Pets
                .Include(p => p.PetOwners) // Include the PetOwners for owner data
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

            return View(petDTOs);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets
                .Include(p => p.PetOwners) // Load related owners
                .FirstOrDefaultAsync(m => m.PetId == id);

            if (pet == null)
            {
                return NotFound();
            }

            // Convert Pet entity to PetDTO
            var petDetails = new PetDetails
            {
                Pet = new PetDTO
                {
                    PetId = pet.PetId,
                    Name = pet.Name ?? string.Empty,
                    Type = pet.Type ?? string.Empty,
                    Breed = pet.Breed ?? string.Empty,
                    DOB = pet.DOB,
                    OwnerIds = pet.PetOwners.Select(po => po.OwnerId).ToList() // Map PetOwners to OwnerIds
                }
            };

            return View(petDetails);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PetDTO petDto)
        {
            if (ModelState.IsValid)
            {
                var pet = new Pet
                {
                    Name = petDto.Name,
                    Type = petDto.Type,
                    Breed = petDto.Breed,
                    DOB = petDto.DOB
                };

                _context.Add(pet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(petDto);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets.FindAsync(id);
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
                DOB = pet.DOB
            };

            return View(petDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PetDTO petDto)
        {
            if (id != petDto.PetId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var pet = new Pet
                {
                    PetId = petDto.PetId,
                    Name = petDto.Name,
                    Type = petDto.Type,
                    Breed = petDto.Breed,
                    DOB = petDto.DOB
                };

                try
                {
                    _context.Update(pet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PetExists(pet.PetId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(petDto);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets
                .Include(p => p.PetOwners) // Include relationships to avoid FK constraint issues
                .FirstOrDefaultAsync(m => m.PetId == id);

            if (pet == null)
            {
                return NotFound();
            }

            var petDetails = new PetDetails
            {
                Pet = new pawpals.Models.DTOs.PetDTO
                {
                    PetId = pet.PetId,
                    Name = pet.Name ?? string.Empty,
                    Type = pet.Type ?? string.Empty,
                    Breed = pet.Breed ?? string.Empty,
                    DOB = pet.DOB,
                    OwnerIds = pet.PetOwners.Select(po => po.OwnerId).ToList()
                }
            };

            return View(petDetails);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pet = await _context.Pets
                .Include(p => p.PetOwners) // Ensure related entities are loaded
                .FirstOrDefaultAsync(p => p.PetId == id);

            if (pet == null)
            {
                return NotFound();
            }

            // Remove related PetOwners first to avoid FK constraint errors
            _context.PetOwners.RemoveRange(pet.PetOwners);

            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool PetExists(int id)
        {
            return _context.Pets.Any(e => e.PetId == id);
        }

    }
}