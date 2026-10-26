using CMS.Core.DTOs;
using CMS.Infrastructure.Services.Users;
using Microsoft.AspNetCore.Mvc;
using CMS.Core.Constants;
using Results = CMS.Core.Constants.Results;

namespace CMS.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService=userService;
        }

        [HttpGet]
        //returns view and there is another request for the
        //datatable to return the needed data
        //so it displays the empty table
        public IActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetUserData(Pagination pagination, Query query)
        {
          var result =await _userService.GetAll(pagination, query);
            return Json(result);
 
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]CreateUserDto dto)
        {
            if (ModelState.IsValid)
            {
                await _userService.Create(dto);
                return Ok(Results.AddSuccessResult());
            }
            return View(dto);
        }
        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var user = await _userService.Get(id);
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Update([FromForm] UpdateUserDto dto)
        {
            if (ModelState.IsValid)
            {
                await _userService.Update(dto);
                return Ok(Results.EditSuccessResult());
            }
            return View(dto);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            await _userService.Delete(id);
            return Ok(Results.DeleteSuccessResult());

         
        }
        //[HttpGet]
        //public IActionResult DeExportToExcel()
        //{
        //    return View();
        //}
    }
}
