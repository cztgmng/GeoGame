using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace GeoGame.Pages
{
    public class LoginModel : PageModel
    {
        static Dictionary<string, string> users = new Dictionary<string, string>
            {

            }; // update your users here

        /*
            It should look like this  

            {"user1", "pass1"},
            {"user2", "pass2"},
            {"user3", "pass3"}

        */

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/Game");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (users.ContainsKey(Username))
            {
                if (Password == users[Username])
                {
                    var option = new CookieOptions
                    {
                        Expires = DateTime.Now.AddYears(1)
                    };
                    Response.Cookies.Append("UserCookie", Username, option);

                    return Redirect("/Game");
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return Page();
        }
    }
}
