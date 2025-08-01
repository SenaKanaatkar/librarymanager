namespace LibraryApp.Models
{
    public class Book
    {
        public int Id { get; set; }         
        public string Title { get; set; }  = string.Empty;
        
        public string Author { get; set; }= string.Empty;   
        public string Details { get; set; } = string.Empty;      
        public string Genre { get; set; }    = string.Empty;
        public int Stock { get; set; }    
    }
}
