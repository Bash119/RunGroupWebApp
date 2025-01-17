﻿using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.Repository;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class DashboardController : Controller
    {
       
        private readonly IDashboardRepository _dasboardRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPhotoService _photoService;

        
        public DashboardController(IDashboardRepository dashboardRepository
            ,IHttpContextAccessor httpContextAccessor,IPhotoService photoService) 
        {
            _dasboardRepository= dashboardRepository;
            _httpContextAccessor= httpContextAccessor;
            _photoService= photoService;

        }

        private void MapUserEdit(AppUser user,EditUserDashboardViewModel editVM,ImageUploadResult photoResult)
        {
            user.Id= editVM.Id;
            user.Pace = editVM.Pace;
            user.Mileage =editVM.Mileage;
            user.ImageProfileUrl = photoResult.Url.ToString();
            user.City = editVM.City;
            user.State=editVM.State;
        }
        public async Task<IActionResult> Index()
        {
            var userRaces= await _dasboardRepository.GetAllUserRaces();
            var userClubs=await _dasboardRepository.GetAllUserClubs();

            var dashboardViewModel = new DashboardViewModel()
            {
                Races = userRaces,
                Clubs = userClubs
            };
            return View(dashboardViewModel);
        }

        public async Task<IActionResult> EditUserProfile()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _dasboardRepository.GetUserById(curUserId);
            if (user == null) return View("Error");

            var editUserVM = new EditUserDashboardViewModel
            {
                Id= curUserId,
                Pace=user.Pace,
                Mileage=user.Mileage,
                ImageProfileUrl=user.ImageProfileUrl,
                City=user.City,
                State=user.State
            };
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserDashboardViewModel editVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit Profile");
                return View("EditUserProfile", editVM);
            }

            var user=await _dasboardRepository.GetUserByIdNoTracking(editVM.Id);

            if(user.ImageProfileUrl == "" || user.ImageProfileUrl == null)
            {
                var photoResult = await _photoService.AddPhotoAsync(editVM.Image);
                MapUserEdit(user,editVM, photoResult);
                _dasboardRepository.Update(user);
                return RedirectToAction("Index");
            }
            else
            {
                try
                {
                    await _photoService.DeletePhotoAsync(user.ImageProfileUrl);
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(editVM);
                }
                var photoResult = await _photoService.AddPhotoAsync(editVM.Image);
                MapUserEdit(user, editVM, photoResult);
                _dasboardRepository.Update(user);
                return RedirectToAction("Index");
            }


        }
    }
}
