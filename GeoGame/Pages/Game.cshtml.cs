using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GeoGame.Pages
{
    public class GameModel : PageModel
    {
        public IActionResult OnGet()
        {
            if (Request.Cookies["UserCookie"] == null)
            {
                return Redirect("Login");
            }

            return Page();
        }
    }
}
