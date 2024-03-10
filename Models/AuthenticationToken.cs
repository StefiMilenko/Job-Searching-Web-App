namespace Models;

public class AuthenticationToken
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [MinLength(100)]
    [MaxLength(100)]
    public string? TokenString { get; set; }
    [JsonIgnore]
    public required Korisnik Korisnik { get; set; }
    public DateTime? ExpireTime { get; set; }
}