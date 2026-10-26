using CMS.Core.DTOs;
using CMS.Infrastructure.Services.Advertisments;
using CMS.Infrastructure.Services.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Results = CMS.Core.Constants.Results;
namespace CMS.Web.Controllers
{
    public class AdvertismentController : Controller
    {
        private readonly IAdvertismentService _advertismentService;
        public AdvertismentController(IAdvertismentService advertismentService)
        {
            _advertismentService = advertismentService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetAdvertismentData(Pagination pagination, Query query)
        {
            var result = await _advertismentService.GetAll(pagination, query);
            return Json(result);

        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["Owners"] = new SelectList(await _advertismentService.GetAdvertismentOwners(), "Id", "FullName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateAdvertismentDto dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.OwnerId))
            {
                ModelState.Remove("Owner.FullName");
                ModelState.Remove("Owner.Email");
                ModelState.Remove("Owner.PhoneNumber");
            }
            if (ModelState.IsValid)
            {
                await _advertismentService.Create(dto);
                return Ok(Results.AddSuccessResult());
            }
            return View(dto);
        }
        //[HttpGet]
        //public async Task<IActionResult> Update(int id)
        //{
        //    var user = await _advertismentService.Get(id);
        //    return View(user);
        //}
        //[HttpPost]
        //public async Task<IActionResult> Update(UpdateAdvertismentDto dto)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        await _advertismentService.Update(dto);
        //        return Ok(Results.EditSuccessResult());
        //    }
        //    return View(dto);
        //}
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _advertismentService.Delete(id);
            return Ok(Results.DeleteSuccessResult());


        }
    }
}
