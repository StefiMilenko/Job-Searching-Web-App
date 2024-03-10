namespace Models;
public class Komentar
{
    [Key]
    public int? Id { get; set; }
    public required string Sadrzaj { get; set; }
    public required DateTime DatumSlanja { get; set; }
    public DateTime? DatumEditovanja { get; set; }
    public bool Editovan { get { return DatumEditovanja != null; } }
    [JsonIgnore]
    public IList<Komentar>? Odgovori { get; set; }
    public  Posao? Posao { get; set; }
    public  Trazilac? Autor { get; set; }
}