﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class ClubController : Controller
    {
       

        private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContectAccessor;

        public ClubController(IClubRepository clubRepository, IPhotoService photoService,IHttpContextAccessor httpContextAccessor) 
        {
            
            _clubRepository = clubRepository;
            _photoService = photoService;
            _httpContectAccessor = httpContextAccessor;
        }
        public async  Task<IActionResult> Index()
        {
            IEnumerable<Club> clubs= await _clubRepository.GetAllClubsAsync();
            return View(clubs);
        }

        public async Task<IActionResult> Detail(int id) 
        {
            Club club = await _clubRepository.GetClubByIdAsync(id);
            return View(club);
        }

        public IActionResult Create()
        {
            var curUserId = _httpContectAccessor.HttpContext.User.GetUserId();
            var createClubViewModel = new CreateClubViewModel
            {
                AppUserId = curUserId,
            };
            return View(createClubViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubVM)
        {
            if(ModelState.IsValid)
            {

                var result = await _photoService.AddPhotoAsync(clubVM.Image);
                var club = new Club
                {
                    Title = clubVM.Title,
                    Description=clubVM.Description,
                    Image=result.Url.ToString(),
                    ClubCategory=clubVM.ClubCategory,
                    AppUserId=clubVM.AppUserId,
                    Address= new Address
                    {
                        City = clubVM.Address.City,
                        Street= clubVM.Address.Street,
                        State = clubVM.Address.State,
                    }

                };
                _clubRepository.Add(club);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }
            return View(clubVM);
            
        }

        public async Task<IActionResult> Edit(int id)
        {
            var club=await _clubRepository.GetClubByIdAsync(id);
            if (club == null) return View("Error");

            var clubVM = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                Address = club.Address,
                AddressId = club.AddressId,
                URL = club.Image,
                ClubCategory = club.ClubCategory
            };

            return View(clubVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id,EditClubViewModel clubVM)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit Club.");
                return View("Edit",clubVM);
            }
            var userClub = await _clubRepository.GetClubByIdAsyncNoTracking(id);
            if (userClub != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(userClub.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(clubVM);
                }
                var photoResult = await _photoService.AddPhotoAsync(clubVM.Image);
                var club = new Club
                {
                    Id = id,
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Address = clubVM.Address,
                    AddressId = clubVM.AddressId,
                    Image = photoResult.Url.ToString()
                };
                _clubRepository.Update(club);
                return RedirectToAction("Index");

            }
            else return View(clubVM);
            

        }


        public async Task<IActionResult> Delete(int id)
        {
            var clubDetails=await _clubRepository.GetClubByIdAsync(id);
            if (clubDetails == null) return View("Error");
            return View(clubDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var clubDetails = await _clubRepository.GetClubByIdAsync(id);
            if (clubDetails == null) return View("Error");
            _clubRepository.Delete(clubDetails);
            return RedirectToAction("Index");
        }
    }
}
