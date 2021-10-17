using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pickle.Pages
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            var now = DateTime.UtcNow.Date;
            return Redirect($"/{now.Year}/{now.Month}");
        }
    }
}
