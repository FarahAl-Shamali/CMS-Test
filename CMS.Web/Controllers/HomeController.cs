
using CMS.Infrastructure.Services;
using CMS.Infrastructure.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CMS.Web.Controllers
{
  
    public class HomeController : BaseController
    {
        private readonly IDashBoardService _dashboardService;
        public HomeController(IDashBoardService dashboardService, IUserService userService) : base(userService)
        {
            _dashboardService = dashboardService;
        }
        public async Task<IActionResult> Index()
        {
            if(userType != "Administrator")
            {
                return Redirect("/category");
            }
            var data = await _dashboardService.GetData();
            return View(data);
        }
        public async Task<IActionResult> GetUserTypeChartData()
        {
            var data = await _dashboardService.GetUserTypeChart();
            return Ok(data);
        }
        public async Task<IActionResult> GetContentTypeChartData()
        {
        
            var data = await _dashboardService.GetContentTypeChart();
            return Ok(data);
        }
        public async Task<IActionResult> GetContentByMonthChartData()
        {        
            var data = await _dashboardService.GetContentByMonthChart();
            return Ok(data);
        }

    }
}
