namespace Models;

public class Posao
{
    [Key]
    public int Id {get; set;}
    [JsonIgnore]
    public Poslodavac? Poslodavac {get; set;}
    public required string Naziv {get; set;}
    public required string Lokacija {get; set;}
    public double? latitude {get; set;}
    public double? longitude {get; set;}
    public int? PlataOd { get; set; }
    public int? PlataDo { get; set; }
    public string? PlataValuta { get; set; }
    public string? PlataInfo { get; set; }
    public required string SektorRada {get; set;}
    public required string Industrija {get; set;}
    public required string VrstaPosla {get; set;}
    public required string TipPosla {get; set;}
    public required string OpisPosla {get; set;}
    public IList<Ocena>? Ocene { get; set; }
    public IList<Komentar>? Komentari { get; set; }
    public float ProsecnaOcena { get; set; } = 0;
    
}

