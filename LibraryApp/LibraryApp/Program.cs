using Microsoft.EntityFrameworkCore;
using LibraryApp.Data;
using LibraryApp.Consumers;
using Serilog;
using Serilog.Events;

var logPath = "Logs/log-.txt";

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File(logPath, restrictedToMinimumLevel: LogEventLevel.Information,
        retainedFileCountLimit: 7,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}")
    .WriteTo.Console()  
    .CreateLogger();

try
{
    Log.Information("Uygulama başlatılıyor...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString));

    builder.Services.AddSingleton<IRabbitMqService, RabbitMqService>();
    builder.Services.AddHostedService<EmailConsumerService>();
    builder.Services.AddHostedService<LogConsumerService>();

    builder.Services.AddDistributedMemoryCache();
    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

    builder.Services.AddControllersWithViews();

    var app = builder.Build();

    app.UseSession();
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    
    Log.Information("Consumer'lar dinlemeye başladı.");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Uygulama beklenmedik şekilde durdu.");
}
finally
{
    Log.CloseAndFlush();
}
