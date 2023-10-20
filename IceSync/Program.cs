using CPanel.DistributionWorker.Manager.Database;
using IceCreamCompanySync;
using IceCreamCompanySync.Database.Intefaces;
using IceCreamCompanySync.HttpHandler;
using IceCreamCompanySync.HttpHandlers;
using IceCreamCompanySync.Interfaces;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHostedService<IceCreamBackgrounWorker>();
builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddSingleton<IRequestHandler, RequestHandler>();
builder.Services.AddSingleton<IDatabaseManager, DatabaseManager>();

var logDirectory = Path.Combine(builder.Environment.ContentRootPath, "Logs"); // Specify the folder
var logFilePath = Path.Combine(logDirectory, "log.txt"); // Specify the file path

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
    .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Logging.AddSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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

