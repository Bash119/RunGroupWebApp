using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.Repository;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class RaceController : Controller
    {
        
        private readonly IRaceRepository _raceRepository;
        private readonly IPhotoService _photoservice;
        public RaceController(IRaceRepository raceRepository,IPhotoService photoService) 
        {
           
            _raceRepository=raceRepository;
            _photoservice=photoService;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Race> races = await _raceRepository.GetAllRacesAsync();
            return View(races);
        }

        public async Task<IActionResult> Detail( int id) 
        {
            Race race = await _raceRepository.GetRaceByIdAsync(id);   
            return View(race);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceViewModel raceVM) {

            if (ModelState.IsValid)
            {
                var result = await _photoservice.AddPhotoAsync(raceVM.Image);
                var race = new Race
                {
                    Title = raceVM.Title,
                    Description = raceVM.Description,
                    Image = result.Url.ToString(),
                    Address = new Address
                    {
                        City = raceVM.Address.City,
                        State = raceVM.Address.State,
                        Street = raceVM.Address.Street
                    }
                };
                _raceRepository.Add(race);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo Upload Failed");
            }
            return View(raceVM);
            
        }

        public async Task<IActionResult> Edit(int id)
        {
            var race = await _raceRepository.GetRaceByIdAsync(id);
            if (race == null) return View("Error");

            var raceVM = new EditRaceViewModel
            {
                Title = race.Title,
                Description = race.Description,
                Address = race.Address,
                AddressId = race.AddressId,
                URL = race.Image,
                RaceCategory = race.RaceCategory
            };

            return View(raceVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRaceViewModel raceVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit Club.");
                return View("Edit", raceVM);
            }
            var userRace = await _raceRepository.GetRaceByIdAsyncNoTracking(id);
            if (userRace != null)
            {
                try
                {
                    await _photoservice.DeletePhotoAsync(userRace.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(raceVM);
                }
                var photoResult = await _photoservice.AddPhotoAsync(raceVM.Image);
                var race = new Race
                {
                    Id = id,
                    Title = raceVM.Title,
                    Description = raceVM.Description,
                    Address = raceVM.Address,
                    AddressId = raceVM.AddressId,
                    Image = photoResult.Url.ToString()
                };
                _raceRepository.Update(race);
                return RedirectToAction("Index");

            }
            else return View(raceVM);


        }

        public async Task<IActionResult> Delete(int id)
        {
            var race=await _raceRepository.GetRaceByIdAsync(id);
            if (race == null) return View("Error");
            return View(race);
        }

        [HttpPost,ActionName("Delete")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var race=await _raceRepository.GetRaceByIdAsync(id);
            if (race == null) return View("Error");
            _raceRepository.Delete(race);
            return RedirectToAction("Index");
        }

    }
}
