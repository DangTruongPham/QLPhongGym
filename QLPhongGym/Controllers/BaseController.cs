using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace QLPhongGym.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var tenDangNhap = HttpContext.Session.GetString("TenDangNhap");

            if (string.IsNullOrEmpty(tenDangNhap))
            {
                context.Result = RedirectToAction("Login", "Auth");
                return;
            }

            base.OnActionExecuting(context);
        }

        protected bool IsAdmin()
        {
            return HttpContext.Session.GetString("VaiTro") == "Admin";
        }

        protected bool IsNhanVien()
        {
            return HttpContext.Session.GetString("VaiTro") == "NhanVien";
        }

        protected IActionResult RedirectAccessDenied()
        {
            return RedirectToAction("AccessDenied", "Auth");
        }
    }
}
