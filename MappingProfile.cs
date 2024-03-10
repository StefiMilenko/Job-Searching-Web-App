public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<KorisnikRegistration, Korisnik>()
            .ForMember(korisnik => korisnik.UserName, opt => opt.MapFrom(x => x.Email));
    }
}