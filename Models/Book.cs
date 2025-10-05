namespace BibliotecaDigital.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Code { get; set; }
    public int Available { get; set; }
    public List<Lean>? Leans { get; set; }
}