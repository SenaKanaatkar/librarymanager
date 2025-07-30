using Microsoft.EntityFrameworkCore;
using LibraryApp.Data; // DbContext buradan gelecek

var builder = WebApplication.CreateBuilder(args);

// Connection string'i appsettings.json'dan al
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// EF Core'u ekle
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDistributedMemoryCache();// Oturum verilerini bellek içinde saklamak için
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum süresi
    options.Cookie.HttpOnly = true;  // Güvenlik
    options.Cookie.IsEssential = true; // Çerez zorunlu olsun
});


// MVC desteğini ekle
builder.Services.AddControllersWithViews();

var app = builder.Build();
app.UseSession();

// Hata yönetimi ve HTTPS ayarları
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Varsayılan route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
