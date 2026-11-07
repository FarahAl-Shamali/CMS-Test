using CMS.Core.Enums;
using CMS.Infrastructure.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CMS.Web.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected readonly IUserService _userService;
        protected string userType;
        public BaseController(IUserService userService)
        {
            _userService = userService;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (User.Identity.IsAuthenticated)
            {
                var userName = User.Identity.Name;
                var user = _userService.GetUserByUserName(userName);
                ViewBag.FullName = user.FullName;
                userType = user.UserType;
                ViewBag.image = user.ImageUrl;
                ViewBag.UserType = user.UserType.ToString();
            }

        }
    }
}
