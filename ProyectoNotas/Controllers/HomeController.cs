using Microsoft.AspNetCore.Mvc;

namespace ProyectoNotas.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
