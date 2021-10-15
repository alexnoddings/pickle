using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace Pickle.Pages
{
    public class DayModel : PageModel
    {
        public enum Availability
        {
            Locked,
            Missing,
            Available
        }
        public int Year { get; private set; }
        public int Month { get; private set; }
        public int Day { get; private set; }

        public bool IsLocked { get; private set; }
        public string? FileExtension { get; private set; }

        private Files Files { get; }

        public DayModel(Files files)
        {
            Files = files ?? throw new ArgumentNullException(nameof(files));
        }

        public IActionResult OnGet()
        {
            var yearStr = RouteData.Values["year"];
            var monthStr = RouteData.Values["month"];
            var dayStr = RouteData.Values["day"];

            var isValid = DateTime.TryParse($"{yearStr}-{monthStr}-{dayStr} 00:00:00", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date);
            if (!isValid)
                return Redirect("/");

            IsLocked = date > DateTime.Today;
            if (!IsLocked)
                FileExtension = Files.GetFileExtension(date);

            Year = date.Year;
            Month = date.Month;
            Day = date.Day;
            return Page();
        }
    }
}
