using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;

namespace LSP3.Pages;

public class FilesModel : MasterModel
{
    [BindProperty]
    public List<FileDisplayModel> Files { get; set; }

    private readonly ILogger<FilesModel> _logger;

    private readonly AppSettings _appSettings;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FilesModel(IOptions<AppSettings> appSettings, ILogger<FilesModel> logger, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _appSettings = appSettings.Value;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        Files = new List<FileDisplayModel>();
    }

    public async Task<IActionResult> OnGet()
    {

        try
        {
            SessionHelper sessionHelper = new();


            if (!base.IsAuthenticated)
                return Redirect("/Account/Login");

            if (!base.IsAdmin)
                return Redirect("/Index");


            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), _appSettings.ImageData);

            if (!Directory.Exists(fullPath))
            {
                // Handle the case where the directory doesn't exist (e.g., log an error)
                ;
            }

            var filelist = new DirectoryInfo(fullPath).GetFiles("*.*").ToList();

            foreach( var file in filelist)
            {
                Files.Add(new FileDisplayModel
                {
                    name = file.Name,
                    length = file.Length,
                    createDate = file.CreationTime.ToString("yyyy-MM-dd hh:mm:ss"),
                    modifyDate = file.LastWriteTime.ToString("yyyy-MM-dd hh:mm:ss")
                });
            }

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}: {ex.InnerException}: {ex.StackTrace}");
        }

        return Page();
    }


}
