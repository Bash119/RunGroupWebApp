﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Controllers
{
    public class RaceController : Controller
    {
        
        private readonly IRaceRepository _raceRepository;
        public RaceController(ApplicationDbContext context,IRaceRepository raceRepository) 
        {
           
            _raceRepository=raceRepository;
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
    }
}
