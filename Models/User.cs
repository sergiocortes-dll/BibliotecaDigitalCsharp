namespace BibliotecaDigital.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string DNI { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public List<Lean>? Leans { get; set; } 
}