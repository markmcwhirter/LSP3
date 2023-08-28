using LSP3.Model;
using static System.Reflection.Metadata.BlobBuilder;
using System.Text.Json;

namespace LSP3.Pages
{
    public class HttpHelper
    {
        public async Task<string> Get(string url)
        {
            using (var httpClient = new HttpClient())
            {
                using (HttpResponseMessage response = await httpClient.GetAsync(url))
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }
        public string GetSessionString(IHttpContextAccessor ctx, string variable) => ctx.HttpContext.Session.GetString(variable);
        public void SetSessionString(IHttpContextAccessor ctx, string variable, string value)
        {
            if (ctx != null && ctx.HttpContext != null) ctx.HttpContext.Session.SetString(variable, value);
        }
    }
}
