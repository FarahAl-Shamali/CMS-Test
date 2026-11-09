using CMS.Core.DTOs;
using CMS.Infrastructure.Services.Advertisments;
using CMS.Infrastructure.Services.Categories;
using CMS.Infrastructure.Services.Tracks;
using CMS.Infrastructure.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Results = CMS.Core.Constants.Results;

namespace CMS.Web.Controllers
{
    public class TrackController : BaseController
    {
        private readonly ITrackService _trackService;
        private readonly ICategoryService _categoryService;
        public TrackController(ITrackService trackService, ICategoryService categoryService, IUserService userService) : base(userService)
        {
            _trackService = trackService;
            _categoryService = categoryService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetTracksData(Pagination pagination, Query query)
        {
            var result = await _trackService.GetAll(pagination, query);
            return Json(result);

        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["Categories"] = new SelectList(await _categoryService.GetCategoryList(), "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTrackDto dto)
        {
          
            if (ModelState.IsValid)
            {
                await _trackService.Create(dto);
                return Ok(Results.AddSuccessResult());
            }
            ViewData["Categories"] = new SelectList(await _categoryService.GetCategoryList(), "Id", "Name");
            return View(dto);
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var track = await _trackService.Get(id);
            ViewData["Categories"] = new SelectList(await _categoryService.GetCategoryList(), "Id", "Name");

            return View(track);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateTrackDto dto)
        {
            if (ModelState.IsValid)
            {
                await _trackService.Update(dto);
                return Ok(Results.EditSuccessResult());
            }
            ViewData["Categories"] = new SelectList(await _categoryService.GetCategoryList(), "Id", "Name");
            return View(dto);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _trackService.Delete(id);
            return Ok(Results.DeleteSuccessResult());


        }
    }
}
