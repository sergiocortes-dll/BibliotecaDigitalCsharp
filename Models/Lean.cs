namespace BibliotecaDigital.Models;

public class Lean
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int BookId { get; set; }
    public DateTime LeanDate { get; set; } = DateTime.Now;
    public DateTime ReturnDate { get; set; }
    
    public User? User { get; set; }
    public Book? Book { get; set; }
}