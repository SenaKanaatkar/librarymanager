using LibraryApp.Data;
using LibraryApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;        

namespace LibraryApp.Controllers
{
    public class BorrowController : Controller
    {
        private readonly AppDbContext _context;

        public BorrowController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult BorrowBook(int bookId)
        {
            // id al
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Json(new { success = false, message = "Giriş yapmanız gerekiyor!" });
            }

            // Kitabı bul
            var book = _context.Books.FirstOrDefault(b => b.Id == bookId);
            if (book == null)
            {
                return Json(new { success = false, message = "Kitap bulunamadı!" });
            }

            // 3) Stok kontrolü
            if (book.Stock <= 0)
            {
                return Json(new { success = false, message = "Kitap stokta yok!" });
            }

            // 4) BorrowList’e ekle
            var borrow = new BorrowList
            {
                UserId = userId.Value,
                BookId = book.Id,
                BorrowDate = DateTime.Now,
                ReturnDate = null,
                User = _context.Users.Find(userId.Value), // İlişkili kullanıcıyı yükle
                Book = book // İlişkili kitabı yükle
            };

            _context.BorrowLists.Add(borrow);

            // 5) Stok 1 azalt
            book.Stock -= 1;

            _context.SaveChanges();

            return Json(new { success = true, message = $"{book.Title} ödünç alındı." });
        }



        // Ödünç alınan kitapları listele
        [HttpGet]
public IActionResult BorrowedBooksPartial()
{
    var userId = HttpContext.Session.GetInt32("UserId");
    if (userId == null)
        return PartialView("_BorrowedBooksPartial", new List<BorrowList>());

    var borrowedBooks = _context.BorrowLists
        .Include(b => b.Book)
        .Where(b => b.UserId == userId.Value )
        .ToList();

    return PartialView("_BorrowedBooksPartial", borrowedBooks);
}


        // İade işlemi
            [HttpPost]
        public IActionResult ReturnBook(int borrowId)
        {
            // 1) Kullanıcı ID'sini al
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Json(new { success = false, message = "Giriş yapmanız gerekiyor!" });

            }
            // 2) BorrowList kaydını bul
            var borrow = _context.BorrowLists.Include(b => b.Book).FirstOrDefault(b => b.Id == borrowId && b.UserId == userId.Value);
            if (borrow == null)
            {
                return Json(new { success = false, message = "Ödünç kaydı bulunamadı!" });
            }
            // 3) İade tarihini güncelle
            borrow.ReturnDate = DateTime.Now;
            // 4) Kitabın stokunu artır
            borrow.Book.Stock += 1;
            // 5) Değişiklikleri kaydet
            _context.SaveChanges();
            return Json(new { success = true, message = $"{borrow.Book.Title} iade edildi." });

        }
    }
}
