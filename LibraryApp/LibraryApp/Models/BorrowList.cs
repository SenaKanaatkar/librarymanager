using System.Runtime.CompilerServices;

namespace LibraryApp.Models
{
    public class BorrowList
    {
        public int Id { get; set; }  // Ödünç alma kaydı ID'si
        public int UserId { get; set; }  // Ödünç alan kullanıcının ID'si
        public int BookId { get; set; }  // Ödünç alınan kitabın ID'si
        public DateTime BorrowDate { get; set; } = DateTime.Now;// Ödünç alma tarihi
        public DateTime? ReturnDate { get; set; }  // İade tarihi (null ise henüz iade edilmemiş)

        public User? User { get; set; } // İlişkili kullanıcı
        public Book? Book { get; set; }  // İlişkili kitap
    }
}


