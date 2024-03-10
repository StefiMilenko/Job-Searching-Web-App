namespace Models;
[Table("AspNetUsers")]
public class Korisnik : IdentityUser<int>
{
    public required string Ime { get; set; }
    public required string Prezime { get; set; }
    public string? Drzava { get; set; } = null;
    [Newtonsoft.Json.JsonIgnore]
    public IList<AuthenticationToken> Tokens { get; set; } = new List<AuthenticationToken>();
    public string Biografija { get; set; }

}