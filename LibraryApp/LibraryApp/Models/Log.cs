namespace LibraryApp.Models;
public class Log
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string Action { get; set; }=string.Empty;
    public string Level { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
}