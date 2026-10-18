using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CarRenting.BusinessObjects.Models;
using CarRenting.DTOs;
using CarRenting.Repositories.Context;

namespace CarRenting.Client.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly HttpClient _client;
        private string _apiUrl;

        public RegisterModel()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _apiUrl = Constants.Api + "Authentication/register";
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty] public CustomerDto Customer { get; set; } = default!;


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _client.PostAsJsonAsync(_apiUrl, Customer);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var dataResponse = result.Content.ReadFromJsonAsync<CustomerDto>().Result;
                if (dataResponse == null)
                {
                    ModelState.AddModelError(Customer.Email, "Duplicated email");
                }
            }else 
            {
                ModelState.AddModelError("Customer.Email", "Duplicated email");
                return Page();
            }
            return RedirectToPage("./Login");
        }
    }
}