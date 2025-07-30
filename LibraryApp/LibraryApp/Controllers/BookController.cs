using LibraryApp.Data;
using LibraryApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Controllers;

public class BookController : Controller
{
    private readonly AppDbContext _context;

    public BookController(AppDbContext context) // Dependency injection for the database context nesne oluşturmanın yeni yolu
    {
        _context = context;
    }

    // Kitapları listele
    public IActionResult Index()
    {
        var books = _context.Books.ToList();
        return View(books);
    }

    // Partial view ile formu yükle
    public IActionResult CreatePartial()
    {
        return PartialView("_CreatePartial", new Book());
    }

    // Formu gönderince kitap ekle
    [HttpPost]
    public IActionResult Create(Book book)
    {
        if (ModelState.IsValid)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(book);
    }

    // Kitap detayları (isteğe bağlı)
    public IActionResult Details(int id)
    {
        var book = _context.Books.Find(id);
        if (book == null)
        {
            return NotFound();
        }
        return View(book);
    }

    // Kitap sil
    public IActionResult Delete(int id)
    {
        var book = _context.Books.Find(id);
        if (book != null)
        {
            _context.Books.Remove(book);
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }
    public IActionResult Admin()
    {
        var viewModel = new AdminViewModel
        {
            Books = _context.Books.ToList(),
            Users = _context.Users.ToList(),
        };

        return View(viewModel); // Views/Book/Admin.cshtml dosyasını kullanır
    }
    [HttpPost]
    public IActionResult Add(Book book)
    {
        if (ModelState.IsValid)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
            return Json(new { success = true });
        }
        return Json(new { success = false, message = "Kitap eklenemedi." });
    }
    // Kitap güncelleme
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var book = _context.Books.FirstOrDefault(b => b.Id == id);
        if (book == null)
        {
            return NotFound();
        }
        return View(book);  // Edit.cshtml dosyasına kitabı gönderiyoruz
    }
[HttpPost]
public IActionResult Edit(Book book)
{
    if (!ModelState.IsValid)
    {
        return View(book);
    }

    var existingBook = _context.Books.FirstOrDefault(b => b.Id == book.Id);
    if (existingBook == null)
    {
        return NotFound();
    }

    // Alanları güncelle
    existingBook.Title = book.Title;
    existingBook.Author = book.Author;
    existingBook.Genre = book.Genre;
    existingBook.Stock = book.Stock;

    _context.SaveChanges();

    return RedirectToAction("Admin", "Book"); // Düzenleme sonrası admin kitap listesine geri dön
}


}
