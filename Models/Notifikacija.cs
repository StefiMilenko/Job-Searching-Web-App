namespace Models;

public class Notifikacija
{
    [Key]
    public int? Id { get; set; }
    public Korisnik? Korisnik { get; set; }
    public bool Seen { get; set; } = false;
    public string Sadrzaj { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty; 
}