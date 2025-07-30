using LibraryApp.Data;
using LibraryApp.Models;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller

{
    private readonly AppDbContext _context;

    public AccountController(AppDbContext context) => _context = context;//di kullanıcı oluşturma için DbContext'i alıyoruz

    [HttpGet]
    public IActionResult RegisterPartial()
    {
        return PartialView("_RegisterPartial");
    }
    [HttpPost]
    public IActionResult Register(User user)
    {
        if (ModelState.IsValid)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") });
        }
        return Json(new { success = false, message = "Kayıt başarısız. Bilgileri kontrol edin." });
    }

    [HttpGet]
    public IActionResult LoginPartial()
    {
        return PartialView("_LoginPartial");
    }

    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
        if (user != null)
        {
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.Username);// Oturumda kullanıcı bilgilerini sakla
            return Json(new { success = true, redirectUrl = Url.Action("Index", "Book") });
        }
        return Json(new { success = false, message = "Kullanıcı adı veya şifre yanlış." });
    }

    // Admin paneli için partial view
    [HttpGet]
    public IActionResult AdminPartial()//burada formu ekranda gösteriyoruz sadece post ile veritabanına kaydedeceğiz
    {
        return PartialView("_AdminPartial");
    }
    [HttpPost]
    public IActionResult AdminLogin(string email, string password)
    {
        // Veritabanında bu kullanıcı var mı?
        var admin = _context.Users
            .FirstOrDefault(u => u.Email == email && u.Password == password && u.IsAdmin);

        if (admin != null)
        {
            // Admin bulundu 
            HttpContext.Session.SetInt32("AdminId", admin.Id);
            return Json(new { success = true, redirectUrl = Url.Action("Admin", "Book") });
        }


        // Admin yoksa hata döndür
        ViewBag.Error = "Geçersiz giriş bilgileri!";
        return View();
    }



    public IActionResult Logout()//buranın önüne buton eklenicek
    {
        HttpContext.Session.Clear(); // Tüm oturum bilgilerini sil
        return RedirectToAction("Index", "Home");

    }
    //kullanıcı silme
[HttpPost]
public IActionResult DeleteUser(int id)
{
    var user = _context.Users.FirstOrDefault(u => u.Id == id);
    if (user == null)
    {
        return Json(new { success = false, message = "Kullanıcı bulunamadı." });
    }

    _context.Users.Remove(user);
    _context.SaveChanges();

    return Json(new { success = true, message = "Kullanıcı başarıyla silindi." });
}


}