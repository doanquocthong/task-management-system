using Microsoft.AspNetCore.Mvc;

namespace practice_aegona_v3.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}