var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((builder, config) =>
 {
     var env = builder.HostingEnvironment;

     config.SetBasePath(env.ContentRootPath)
     .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
     .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
     .AddEnvironmentVariables();
 });


    // Add services to the container.
    //builder.Services.AddRazorPages(options =>
    //{
    //    options.Conventions.AuthorizePage("/Index");
    //    options.Conventions.AuthorizeFolder("/BookContent");
    //    options.Conventions.AuthorizeFolder("/BookImages");
    //    //options.Conventions.AllowAnonymousToPage("/Private/PublicPage");
    //    //options.Conventions.AllowAnonymousToFolder("/Private/PublicPages");
    //});
builder.Services.AddRazorPages();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapRazorPages();
app.MapDefaultControllerRoute();

app.Run();