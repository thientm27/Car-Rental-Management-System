using System.Net.Http.Headers;
using CarRenting.DTOs;
using CarRenting.DTOs.Reponse;
using CarRenting.DTOs.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarRenting.Client.Pages
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient _client;
        private string _apiUrl;

        public LoginModel()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _apiUrl = Constants.Api + "Authentication/login";
        }

        [BindProperty] public CustomerDto MemberAccount { get; set; } = default!;

        public async Task<IActionResult> OnPostLogin()
        {
            if (MemberAccount == null)
            {
                return Page();
            }
            var response = await _client.PostAsJsonAsync(_apiUrl, new LoginRequest()
            {
                Email = MemberAccount.Email,
                Password = MemberAccount.Password
            });
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var dataResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
         
                HttpContext.Session.SetString("JWToken", dataResponse.Jwt);
                HttpContext.Session.SetString("UserName", dataResponse.Name);
                HttpContext.Session.SetString("UserId", dataResponse.Id);
                if (dataResponse.Id == "-1")
                {
                    return RedirectToPage("/Admin2/Customer/Index");
                }
                return RedirectToPage("/Customer2/Index");
            }
            else
            {
                ViewData["notification"] = response.Content;
                return Page();
            }
       
        }

        public IActionResult OnPostLogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Login");
        }
    }
}