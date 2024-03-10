namespace Models;
public class Poslodavac : Korisnik
{
    public string? NazivFirme {get; set;}
    public string? Opis {get; set;}
    [Newtonsoft.Json.JsonIgnore]
    public IList<Posao>? Poslovi { get; set; }
}