using Microsoft.AspNetCore.Mvc;
using pawpals.Models.DTOs;
using pawpals.Services;
using System.Threading.Tasks;

namespace pawpals.Controllers
{
    public class PetPageController : Controller
    {
        private readonly IPetService _petService;

        public PetPageController(IPetService petService)
        {
            _petService = petService;
        }

        public async Task<IActionResult> Index()
        {
            var pets = await _petService.GetAllPetsAsync();
            return View(pets);
        }

        public async Task<IActionResult> Details(int id)
        {
            var pet = await _petService.GetPetByIdAsync(id);
            if (pet == null) return NotFound();
            return View(pet);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PetDTO petDto)
        {
            if (!ModelState.IsValid) return View(petDto);

            await _petService.AddPetAsync(petDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var pet = await _petService.GetPetByIdAsync(id);
            if (pet == null) return NotFound();
            return View(pet);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PetDTO petDto)
        {
            if (id != petDto.PetId) return BadRequest();

            if (!ModelState.IsValid) return View(petDto);

            await _petService.UpdatePetAsync(petDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var pet = await _petService.GetPetByIdAsync(id);
            if (pet == null) return NotFound();
            return View(pet);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _petService.DeletePetAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
