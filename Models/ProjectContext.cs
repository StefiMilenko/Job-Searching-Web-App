namespace Models;

public class ProjectContext : IdentityDbContext<Korisnik, IdentityRole<int>, int>
{
    public required DbSet<Korisnik> Korisnici { get; set; }
    public required DbSet<Posao> Poslovi { get; set; }
    public required DbSet<Poslodavac> Poslodavci { get; set; }
    public required DbSet<Trazilac> Trazioci {get; set;}
    public required DbSet<Komentar> Komentari { get; set; }
    public required DbSet<AuthenticationToken> AuthenticationTokens { get; set; }
    public required DbSet<Ocena> Ocene { get; set; }
    public required DbSet<Notifikacija> Notifikacije { get; set; }

    public ProjectContext(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
  
        modelBuilder.ApplyConfiguration(new RoleConfiguration());

        modelBuilder.Entity<Poslodavac>()
            .HasMany(p => p.Poslovi)
            .WithOne(p => p.Poslodavac);

        modelBuilder.Entity<Korisnik>(entity =>{
    entity.ToTable("Korisnici");
    entity.Ignore(e => e.LockoutEnd);
    entity.Ignore(e => e.ConcurrencyStamp);
    entity.Ignore(e => e.NormalizedEmail);
   
    entity.Ignore(e => e.LockoutEnd);
});    

    
    modelBuilder.Entity<Trazilac>().ToTable("Trazioci");
    modelBuilder.Entity<Poslodavac>().ToTable("Poslodavci");
    }
}