using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace Pickle.Pages
{
    public class MonthModel : PageModel
    {
        public int Year { get; private set; }
        public int Month { get; private set; }

        public bool IsAvailable { get; private set; }

        public IActionResult OnGet()
        {
            var yearStr = RouteData.Values["year"];
            var monthStr = RouteData.Values["month"];

            var isValid = DateTime.TryParse($"{yearStr}-{monthStr}-01 00:00:00", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date);
            if (!isValid)
                return Redirect("/");

            IsAvailable = date >= DateTime.Today;
            Year = date.Year;
            Month = date.Month;
            return Page();
        }
    }
}
