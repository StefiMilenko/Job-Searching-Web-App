namespace Controllers;
[ApiController]
[Route("[controller]")]
public class PoslodavacController : ControllerBase
{
    public required ProjectContext Context { get; set; }
    private readonly UserManager<Korisnik> _userManager;
    public PoslodavacController(ProjectContext context, UserManager<Korisnik> userManager)
    {
        Context = context;
        _userManager = userManager;
    }
 
    [HttpPut("IzmeniPoslodavca")]
    public async Task<ActionResult> IzmeniPoslodavca([FromBody]Poslodavac poslodavac)
    {
        try
        {
            var korisnik = await KorisnikController.CheckTokenAndGetKorisnik(Context, Request);
            if (korisnik == null)
            {
                return Unauthorized("Korisnik nije ulogovan");
            }
            if (!await _userManager.IsInRoleAsync(korisnik, "Poslodavac"))
            {
                return Unauthorized("Korisnik mora biti poslodabac");
            }
            var stariPoslodavac = korisnik as Poslodavac;
            if (stariPoslodavac != null)
            {
                stariPoslodavac.Ime = poslodavac.Ime;
                stariPoslodavac.Prezime = poslodavac.Prezime;
                stariPoslodavac.Opis = poslodavac.Opis;
                stariPoslodavac.Drzava = poslodavac.Drzava;
                stariPoslodavac.NazivFirme = poslodavac.NazivFirme;
                stariPoslodavac.Biografija = poslodavac.Biografija;
                Context.Poslodavci.Update(stariPoslodavac);
            }

            await Context.SaveChangesAsync();
            return Ok("Izmenjen poslodavac");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("ObrisiPoslodavca")]
    public async Task<ActionResult> ObrisiPoslodavca([FromQuery]int PoslodavacId)
    {
        try
        {
            var poslodavac = await Context.Poslovi.FindAsync(PoslodavacId);

            if (poslodavac != null)
            {
                var korisnik = await KorisnikController.CheckTokenAndGetKorisnik(Context, Request);
                if (korisnik == null)
                {
                    return Unauthorized("Korisnik nije ulogovan");
                }
                if(korisnik.Id != poslodavac.Id && !await _userManager.IsInRoleAsync(korisnik, "Administrator"))
                {
                    return Unauthorized();
                }
                Context.Poslovi.Remove(poslodavac);
                await Context.SaveChangesAsync();
                return Ok($"Poslodavac obrisan");
            }
            else
            {
                return BadRequest($"Nije pronađen poslodavac");
            }
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("PreuzmiPoslodavce")]
    public async Task<ActionResult> PreuzmiPoslodavce()
    {
        try
        {
            return Ok(await Context
                .Poslodavci
                .ToListAsync());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    [HttpGet("PreuzmiPoslodavca")]
    public async Task<ActionResult> PreuzmiPoslodavca([FromQuery]int PoslodavacId)
    {
        try
        {
            return Ok(await Context
                .Poslodavci
                .FindAsync(PoslodavacId));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    [HttpGet("PreuzmiFirme")]
    public async Task<IActionResult> PreuzmiFirme()
    {
        Dictionary<string, string> firme = new Dictionary<string, string>();
        await Context.Poslodavci.ForEachAsync(p => firme.TryAdd(p.NazivFirme, p.NazivFirme));
        return Ok(firme.ToList());
    }
}