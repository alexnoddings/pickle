using Microsoft.AspNetCore.Mvc;

namespace Pickle.Pages.Shared.Components.NavBar;

public class NavBarViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
