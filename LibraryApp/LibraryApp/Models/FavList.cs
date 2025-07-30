namespace LibraryApp.Models
{
    public class FavList
    {
        public int Id { get; set; }  // Favori listesi ID'si
        public int UserId { get; set; }  // Favori listeyi oluşturan kullanıcının ID'si
        public int BookId { get; set; }  // Favorilere eklenen kitabın ID'si
        
        public User? User { get; set; } // İlişkili kullanıcı
        public Book? Book { get; set; }  // İlişkili kitap
    }
}