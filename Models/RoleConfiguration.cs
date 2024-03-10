namespace Models;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
        new IdentityRole
        {
            Name = "Posetilac",
            NormalizedName = "POSETILAC"
        },
        new IdentityRole
        {
            Name = "Administrator",
            NormalizedName = "ADMINISTRATOR"
        },
        new IdentityRole
        {
            Name = "Trazilac Posla",
            NormalizedName = "TRAZILAC"
        },
        new IdentityRole
        {
            Name = "Poslodavac",
            NormalizedName = "POSLODAVAC"
        });
        
    }
}
