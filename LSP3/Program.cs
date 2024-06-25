using LSP3;
using LSP3.Model;

using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.FileProviders;

using Serilog;


var configuration = new ConfigurationBuilder()
     .SetBasePath(Directory.GetCurrentDirectory())
     .AddJsonFile("appsettings.json")
     .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
     .Build();

var sequrl = configuration.GetValue<string>("Seq");

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .Enrich.WithThreadId()
    .Enrich.WithProperty("Application", "LSP")
    .Enrich.WithProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production")
    .Enrich.WithProperty("ThreadName", Thread.CurrentThread.Name ?? "Unnamed Thread")
    .Enrich.WithProcessId()
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithEnvironmentUserName()
    .Enrich.WithClientIp()
    .Enrich.WithRequestHeader("User-Agent")
    .WriteTo.File("logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        rollOnFileSizeLimit: true)
    .WriteTo.Seq(sequrl)
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddTransient<EmailService>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
});

builder.Services.AddSingleton<HttpRequestAndCorrelationContextEnricher>();
builder.Host.UseSerilog(Log.Logger);

builder.Services.AddCors(options =>
{
    options.AddPolicy("corspolicy", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});



var app = builder.Build();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials

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

app.UseSerilogRequestLogging();

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