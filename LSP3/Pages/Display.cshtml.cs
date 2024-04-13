using LSP3.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LSP3.Pages;

public class DisplayModel : MasterModel
{
    public readonly IHttpContextAccessor _httpContextAccessor;

    private readonly AppSettings _appSettings;

    public DisplayModel(IOptions<AppSettings> appSettings, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _appSettings = appSettings.Value;
        _httpContextAccessor = httpContextAccessor;
    }

    public IActionResult OnGet()
    {       
        string imageName = HttpContext.Request.Query["target"].ToString();
        var fileExtension = Path.GetExtension(imageName).ToLower().Replace(".","");

        
        var filePath = _appSettings.ImageData + imageName;
        if (!System.IO.File.Exists(filePath))
            return Page();


        Byte[] myBuff = System.IO.File.ReadAllBytes(filePath);
        var fileLength = myBuff.Length;

        if (fileLength == 0)
            return Page();

        string contentType = "";

        switch (fileExtension)
        {
            case "txt":
                contentType = "html/text";
                break;
            case "docx":
                contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                break;
            case "doc":
                contentType = "application/msword";
                break;
            case "pdf":
                contentType = "application/pdf";
                break;
            case "jpeg":
            case "jpg":
                contentType = "image/jpg";
                break;
            case "gif":
                contentType = "image/gif";
                break;
            case "png":
                contentType = "image/png";
                break;
            case "tiff":
                contentType = "image/tiff";
                break;
        }

        Response.ContentType = contentType;

        if( contentType != "")
            return File(myBuff, contentType);
        else
            return Page();

    }
}
