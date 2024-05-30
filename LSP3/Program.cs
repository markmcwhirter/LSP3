using LSP3;
using LSP3.Model;

using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((builder, config) =>
 {
     var env = builder.HostingEnvironment;

     config.SetBasePath(env.ContentRootPath)
     .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
     .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
     .AddEnvironmentVariables();
 });

builder.Services.AddRazorPages();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddTransient<EmailService>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var app = builder.Build();

app.UseCookiePolicy();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");

    app.UseHsts();
}


app.UseStaticFiles()
.UseStaticFiles(new StaticFileOptions()
 {
     FileProvider = new PhysicalFileProvider(
                System.IO.Path.GetFullPath(@"data")),
     RequestPath = new PathString("/data"),
     DefaultContentType = "application/octet-stream"
 });


app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapRazorPages();
app.MapDefaultControllerRoute();

app.Run();