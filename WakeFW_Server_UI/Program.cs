using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WakeFW_Server_UI.Data;
using NLog;
using NLog.Web;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string connectionString = builder.Configuration.GetConnectionString("WakeFW_Server_UIContext");
logger.Debug($"Connection string used: {connectionString}");
try
{
    builder.Services.AddDbContext<WakeFW_Server_UIContext>(options =>
        options.UseSqlite(connectionString));
}
catch(Exception ex)
{
    logger.Error($"Exception occured while adding SQLite to services. Exception message: \n {ex.Message}");
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
