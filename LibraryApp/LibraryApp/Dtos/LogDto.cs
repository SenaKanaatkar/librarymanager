using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace LibraryApp.Dtos
{
    public class LogDto
    {
        public String Message { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public int Id { get; set; } 
        public string Action { get; set; } = string.Empty;
        
       
        public string Details { get; set; } = string.Empty;
    }
}