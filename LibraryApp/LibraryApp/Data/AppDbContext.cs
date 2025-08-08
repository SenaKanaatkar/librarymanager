using Microsoft.EntityFrameworkCore;
using LibraryApp.Models;  // Book modelini kullanmak için


namespace LibraryApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }  // Books tablosu
        public DbSet<User> Users { get; set; } 
        public DbSet<BorrowList> BorrowLists { get; set; } // BorrowList tablosu
        public DbSet<FavList> FavLists { get; set; } // FavList tablosu
        public DbSet<WishBook> WishBooks { get; set; } //   bağışlanacak kitaplar tablosu 
        public DbSet<Log> Logs { get; set; } 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "Admin",
                    Email = "admin@library.com",
                    Password = "1234",  // ileride hashleyeceğiz
                    IsAdmin = true
                }
            );
        }

    }
}
