using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LSP3.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LSP3.Pages
{
    public class AuthorSearch : MasterModel
    {
        private readonly ILogger<AuthorSearch> _logger;
        public AuthorSearchModel SearchTerm { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }

        public IList<AuthorListResultsModel> Results { get; set; }

        HttpHelper helper = new HttpHelper();
        Extensions<List<AuthorDto>> extensions = new Extensions<List<AuthorDto>>();

        public AuthorSearch(ILogger<AuthorSearch> logger, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _logger = logger;
        }

        public async Task OnGetAsync(string LastName, string FirstName)
        {
            if (LastName == null && FirstName == null)
            {
                return;
            }

            SearchTerm = new AuthorSearchModel();
            SearchTerm.LastName = LastName == null ? " " : LastName; ;
            SearchTerm.FirstName = FirstName == null ? " " : FirstName;



            Extensions<List<AuthorListResultsModel>> listextensions = new Extensions<List<AuthorListResultsModel>>();
            try
            {

                var response = await helper.PostAsync($"http://localhost:5253/api/author/search", SearchTerm);

                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();

                Results = JsonConvert.DeserializeObject<List<AuthorListResultsModel>>(responseString);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}: {ex.InnerException}: {ex.StackTrace}");
            }

        }
    }
}
