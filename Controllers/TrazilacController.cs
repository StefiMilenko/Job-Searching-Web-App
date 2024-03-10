namespace Controllers;
[ApiController]
[Route("[controller]")]
public class TrazilacController : ControllerBase
{
    public required ProjectContext Context { get; set; }
    private readonly UserManager<Korisnik> _userManager;
    public TrazilacController(ProjectContext context, UserManager<Korisnik> userManager)
    {
        Context = context;
        _userManager = userManager;
    }
   


    [HttpPut("IzmeniTrazioca")]
    public async Task<ActionResult> IzmeniTrazioca([FromBody]Trazilac trazilac)
    {
        try
        {
            var korisnik = await KorisnikController.CheckTokenAndGetKorisnik(Context, Request);
            if (korisnik == null)
            {
                return Unauthorized("Korisnik nije ulogovan");
            }
            if(korisnik.Id != trazilac.Id && !await _userManager.IsInRoleAsync(korisnik, "Administrator"))
            {
                return Unauthorized();
            }
            var stariTrazilac = await Context.Trazioci.FindAsync(trazilac.Id);

            if (stariTrazilac != null)
            {
                stariTrazilac.Ime = trazilac.Ime;
                stariTrazilac.Prezime = trazilac.Prezime;
                stariTrazilac.CV = trazilac.CV;
                stariTrazilac.StrucnaSprema = trazilac.StrucnaSprema;
                stariTrazilac.Drzava = trazilac.Drzava;
                stariTrazilac.Biografija = trazilac.Biografija;
                Context.Trazioci.Update(stariTrazilac);
            }

            await Context.SaveChangesAsync();
            return Ok($"Izmenjen trazilac");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("ObrisiTrazioca")]
    public async Task<ActionResult> ObrisiTrazioca([FromQuery]int TrazilacId)
    {
        try
        {
            var trazilac = await Context.Poslovi.FindAsync(TrazilacId);
            
            if (trazilac != null)
            {
                var korisnik = await KorisnikController.CheckTokenAndGetKorisnik(Context, Request);
                if (korisnik == null)
                {
                    return Unauthorized("Korisnik nije ulogovan");
                }
                if(korisnik.Id != trazilac.Id && !await _userManager.IsInRoleAsync(korisnik, "Administrator"))
                {
                    return Unauthorized();
                }
                Context.Poslovi.Remove(trazilac);
                await Context.SaveChangesAsync();
                return Ok("Trazilac obrisan");
            }
            else
            {
                return BadRequest($"Nije pronađen trazilac");
            }
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("PreuzmiTrazioce")]
    public async Task<ActionResult> PreuzmiTrazioce()
    {
        try
        {
            return Ok(await Context
                .Trazioci
                .ToListAsync());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    [HttpGet("PreuzmiTrazioca")]
    public async Task<ActionResult> PreuzmiTrazioca([FromQuery]int TrazilacId)
    {
        try
        {
            return Ok(await Context
                .Trazioci
                .FindAsync(TrazilacId));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}