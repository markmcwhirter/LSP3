using LSP3.Model;
using static System.Reflection.Metadata.BlobBuilder;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;

namespace LSP3;

public class SessionHelper
{

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

    public bool IsAuthenticated(IHttpContextAccessor ctx)
    {
        var isAuthenticated = GetSessionString(ctx, "Authenticated");
        

        return (!string.IsNullOrEmpty(isAuthenticated) &&  isAuthenticated == "true");
    }

    public bool IsAdmin(IHttpContextAccessor ctx)
    {
        var isAuthenticated = GetSessionString(ctx, "Admin");


        return (!string.IsNullOrEmpty(isAuthenticated) && isAuthenticated == "true");
    }
    public int GetAuthorId(IHttpContextAccessor ctx)
    {
        var authorId = GetSessionString(ctx, "AuthorId");
        return (!string.IsNullOrEmpty(authorId)) ? Convert.ToInt32(authorId) : 0;
    }
}
