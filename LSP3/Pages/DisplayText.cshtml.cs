using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LSP3.Pages
{
    public class DisplayTextModel : MasterModel
    {
        public DisplayTextModel(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }

        public void OnGet()
        {
            string imageName = System.Web.HttpUtility.HtmlDecode(Request.Query["target"].ToString());
            var imageType = Path.GetExtension(imageName).ToLower();
        }
    }
}
