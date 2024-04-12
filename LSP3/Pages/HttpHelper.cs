using LSP3.Model;
using static System.Reflection.Metadata.BlobBuilder;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;

namespace LSP3.Pages
{
    public class HttpHelper
    {
        public async Task<HttpResponseMessage> PostAsync<T>(string url, T dto)
        {
            using (var client = new HttpClient())
            {
                // Serialize the DTO object to JSON
                var json = JsonConvert.SerializeObject(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send the POST request
                var response = await client.PostAsync(url, content);

                return response;
            }
        }


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
        public void SetCookie(IHttpContextAccessor ctx, string variable, string jsonstring)
        {
            ctx.HttpContext.Response.Cookies.Delete(variable);

            ctx.HttpContext.Response.Cookies.Append(variable, jsonstring, new CookieOptions
            {
                Expires = DateTime.Now.AddHours(1)
            });
        }
        public string GetCookie(IHttpContextAccessor ctx, string variable)
        {
            if (ctx != null && ctx.HttpContext != null && ctx.HttpContext.Request.Cookies[variable] != null)
                return ctx.HttpContext.Request.Cookies[variable].ToString();
            else
                return "";
        }
        
    }
}
