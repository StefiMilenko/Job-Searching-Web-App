namespace Controllers;
[ApiController]
[Route("[controller]")]
public class KorisnikController : ControllerBase
{
    public required ProjectContext Context { get; set; }
    private readonly UserManager<Korisnik> _userManager;
    public KorisnikController(ProjectContext context, UserManager<Korisnik> userManager)
    {
        Context = context;
        _userManager = userManager;
    }
    [HttpDelete("ObrisiKorisnika")]
    public async Task<IActionResult> ObrisiKorisnika([FromQuery]int UserId)
    {
        try
        {
            var korisnik = await KorisnikController.CheckTokenAndGetKorisnik(Context, Request);
            if(korisnik.Id != UserId && !await _userManager.IsInRoleAsync(korisnik, "Administrator"))
            {
                return Unauthorized();
            }
            var user = Context.Korisnici.FindAsync(UserId);
            Context.Korisnici.Remove(user.Result);
            Context.SaveChangesAsync();
            return Ok($"Obrisan je korisnik");
        }
        catch (System.Exception ex)
        {
            return BadRequest();
        }
    }
    [HttpGet("VratiKorisnika")]
    public async Task<IActionResult> VratiKorisnika()
    {
        try
        {
            var korisnik = await KorisnikController.CheckTokenAndGetKorisnik(Context, Request);
            if(korisnik==null) return Unauthorized("Not logged in");
            
            if (await _userManager.IsInRoleAsync(korisnik,"Trazilac"))// Context.Trazioci.Contains(korisnik))
            {
                var data = await Context.Trazioci.Where(p => p.Id == korisnik.Id)
                .Select(p => new {
                    p.Id,
                    p.Ime,
                    p.Prezime,
                    p.Drzava,
                    p.Email,
                    p.CV,
                    p.StrucnaSprema
                }).ToListAsync();
                return Ok(data);
            }
            else if (await _userManager.IsInRoleAsync(korisnik,"Poslodavac"))//Context.Poslodavci.Contains(korisnik))
            {
                var data = await Context.Poslodavci.Where(p => p.Id == korisnik.Id)
                .Select(p => new {
                    p.Id,
                    p.Ime,
                    p.Prezime,
                    p.Drzava,
                    p.Email,
                    p.NazivFirme,
                    p.Opis
                }).ToListAsync();
                return Ok(data);
            }
            return Ok(korisnik.Id);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex);
        }
    }
    [HttpGet("VratiKorisnikaId")]
    public async Task<IActionResult> VratiKorisnikaId([FromQuery]int KorisnikId)
    {
        try
        {
            var korisnik = await Context.Korisnici.FindAsync(KorisnikId);
            if (await _userManager.IsInRoleAsync(korisnik,"Trazilac"))
            {
                var data = await Context.Trazioci.Where(p => p.Id == KorisnikId)
                .Select(p => new {
                    p.Id,
                    p.Ime,
                    p.Prezime,
                    p.Drzava,
                    p.Email,
                    p.CV,
                    p.StrucnaSprema
                }).ToListAsync();
                return Ok(data);
            }
            else if (await _userManager.IsInRoleAsync(korisnik,"Poslodavac"))
            {
                var data = await Context.Poslodavci.Where(p => p.Id == KorisnikId)
                .Select(p => new {
                    p.Id,
                    p.Ime,
                    p.Prezime,
                    p.Drzava,
                    p.Email,
                    p.NazivFirme,
                    p.Opis
                }).ToListAsync();
                return Ok(data);
            }
            return Ok(korisnik.Id);
        }
        catch (System.Exception ex)
        {
            return BadRequest();
        }
    }
    public static async Task<Korisnik?> CheckTokenAndGetKorisnik(ProjectContext Context, HttpRequest Request)
    {
        string? AuthenticationToken = Request.Cookies["authentication-token"];
        if(AuthenticationToken==null) return null;
        var tokenUser = await Context.AuthenticationTokens
            .Where(p => p.TokenString == AuthenticationToken && p.ExpireTime < DateTime.Now)
            .Include(p => p.Korisnik)
            .SingleOrDefaultAsync();
        if(tokenUser == null)
        {
            var tokens = await Context.AuthenticationTokens.Where(p => p.TokenString == AuthenticationToken).ToArrayAsync();
            foreach(var token in tokens)
            {
                Context.AuthenticationTokens.Remove(token);
            }
            await Context.SaveChangesAsync();
            return null;
        }
        return tokenUser.Korisnik;
    }

}