using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class DashboardController : Controller
    {
       
        private readonly IDashboardRepository _dasboardRepository;

        public DashboardController(IDashboardRepository dashboardRepository) 
        {
            _dasboardRepository= dashboardRepository;
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
    }
}
