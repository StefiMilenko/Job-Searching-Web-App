namespace Models;
public enum Role
{
    Trazilac,
    Poslodavac,
}
public class KorisnikRegistration
{
    public required string Ime { get; set; }
    public required string Prezime { get; set; }
    public required string Email { get; set; }
    public string? Drzava { get; set; }
    [Required(ErrorMessage ="Nedostaje šifra")]
    [DataType(DataType.Password)]
    public required string Password { get; set; }
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage ="Sifra se ne poklapa sa potvrđenom šifrom.")]
    public required string ConfirmPassword { get; set; }
    public required Role Uloga { get; set; }
 
    public string? Biografija { get; set; }
    public string? CV { get; set; }
    public string? StrucnaSprema { get; set; }

    public string? NazivFirme { get; set; }
    public string? Opis { get; set; }
}