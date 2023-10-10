using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    //to create a view
    //1. build (ctrl + shift + b)
    //2. right-click on the method's name
    //3. Add View (Razor View)
    //4. Don't change especially the name (normal practice)
    public class AdminController : Controller
    {
        public IActionResult Index()
        {



           //    if(User.Identity == null) return RedirectToAction("Error", "Home");

            if (
                    User.Identity.IsAuthenticated == false)
            {

                TempData["error"] = "Äccess Denied";

                return RedirectToAction("Index", "Home");
                //Index = Action
                //Home = Controller's name
            }
             
            return View();
        }
    }
}
