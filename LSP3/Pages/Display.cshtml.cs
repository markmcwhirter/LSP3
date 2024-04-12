using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LSP3.Model;
using Microsoft.AspNetCore.Hosting.Server;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System;
using System.Net.Mime;

namespace LSP3.Pages;

public class DisplayModel : MasterModel
{
    public readonly IHttpContextAccessor _httpContextAccessor;

    public DisplayModel(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public IActionResult OnGet()
    {       
        string imageName = HttpContext.Request.Query["target"].ToString();
        var fileExtension = Path.GetExtension(imageName).ToLower().Replace(".","");


        var filePath = @"C:\\LSP\\LSP3\\LSP3\\Pages\\data\\" + imageName;
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
