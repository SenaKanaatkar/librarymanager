using LibraryApp.Data;
using LibraryApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Controllers
{
    public class FavoriteController : Controller
    {
        private readonly AppDbContext _context;

        public FavoriteController(AppDbContext context)
        {
            _context = context;
        }

        // Favorilere kitap ekle (POST)
        [HttpPost]
        public IActionResult AddToFavorites(int bookId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Json(new { success = false, message = "Giriş yapmanız gerekiyor!" });

            // Zaten ekli mi kontrol et
            bool exists = _context.FavLists
                .Any(f => f.BookId == bookId && f.UserId == userId.Value);

            if (exists)
                return Json(new { success = false, message = "Bu kitap zaten favorilerinizde!" });

            var favorite = new FavList
            {
                UserId = userId.Value,
                BookId = bookId
            };

            _context.FavLists.Add(favorite);
            _context.SaveChanges();

            return Json(new { success = true, message = "Kitap favorilere eklendi." });
        }

        // Favori kitapları listele (Partial View)
        [HttpGet]
        public IActionResult FavoriteBooksPartial()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return PartialView("_FavoriteBooksPartial", new List<FavList>());

            var favorites = _context.FavLists
                .Include(f => f.Book)
                .Where(f => f.UserId == userId.Value)
                .ToList();

            return PartialView("_FavoriteBooksPartial", favorites);
        }
    }
}
