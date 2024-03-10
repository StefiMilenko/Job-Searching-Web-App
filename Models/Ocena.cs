namespace Models;

public class Ocena
{
    [Key]
    public int? Id { get; set; }
    public float AvgVal { get; set; }

    public int Plata { get; set; } // Ocenjuje visinu plate, beneficije, bonusi itd.

    public int Okruzenje { get; set; } //Ocenjuje kvalitet radnog okruženja, atmosferu, odnose sa kolegama i menađmentom itd.

    public int Napredovanje { get; set; } //Ocenjuje mogućnosti za profesionalni razvoj, unapređenje karijere i sticanje novih veština itd.

    public int PosoKuca { get; set; } //ocenjuju posao na osnovu ravnoteže između obaveza na poslu i privatnih obaveza. Fleksibilnost radnog vremena itd.
    public Korisnik? Korisnik { get; set; }
    [Newtonsoft.Json.JsonIgnore]
    public Posao? Posao { get; set; }
    
}