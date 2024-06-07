using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace LSP3.Pages
{
    public class BookSearchModel : MasterModel
    {
        private readonly ILogger<BookSearchModel> _logger;

        readonly HttpHelper helper = new();
        public AuthorDto? Results { get; set; }
        private readonly AppSettings _appSettings;

        public BookSearchModel(IOptions<AppSettings> appSettings, ILogger<BookSearchModel> logger, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _appSettings = appSettings.Value;
            _logger = logger;
        }
        public void OnGet()
        {
        }
    }
}
