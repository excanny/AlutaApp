using Microsoft.AspNetCore.Mvc;

namespace PermissionsAuthMVC.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
