namespace Models;
public class Trazilac : Korisnik
{
    public string? CV { get; set; }
    public string? StrucnaSprema { get; set; }
    [JsonIgnore]
    public IList<Komentar>? Komentari { get; set; } = new List<Komentar>();

}