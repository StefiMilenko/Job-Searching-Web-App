namespace Controllers;
[ApiController]
[Route("[controller]")]
public class KomentarController : ControllerBase
{
    public required ProjectContext Context { get; set; }
    private readonly UserManager<Korisnik> _userManager;
    public KomentarController(ProjectContext context, UserManager<Korisnik> userManager)
    {
        Context = context;
        _userManager = userManager;
    }

    [HttpGet("VratiKomentar")]
    public async Task<ActionResult> VratiKomentar([FromQuery]int KomentarId)
    {
        try
        {
            return Ok(await Context.Komentari.Include(k=>k.Autor).FirstOrDefaultAsync(k=>k.Id == KomentarId));
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
    [HttpDelete("ObrisiKomentar")]
    public async Task<ActionResult> ObrisiKomentar([FromQuery]int KomentarId)
    {
        try
        {
            var contextKomentari = Context.Komentari; 
            var komentar = await contextKomentari.FindAsync(KomentarId);
            if (komentar == null)
            {
                throw new Exception($"Nije pronađen komentar čiji je ID {KomentarId}");
            }
            var user = await KorisnikController.CheckTokenAndGetKorisnik(Context, Request);
            if (user == null)
            {
                return Unauthorized("Korisnik nije ulogovan");
            }
            if(komentar.Autor != user)
            {
                return Unauthorized("Korisnik sme da brise samo svoje komentare");
            }
            contextKomentari.Remove(komentar);
            await Context.SaveChangesAsync();
            return Ok("Obrisan komentar");
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
    [HttpGet("VratiKomentareTrazioca")]
    public async Task<ActionResult> VratiKomentareTrazioca([FromQuery]int TrazilacId)
    {
        try
        {
            var trazilac = await Context.Trazioci.Where(p => p.Id == TrazilacId)
                                            .Include(p => p.Komentari)
                                            .SingleOrDefaultAsync();
            if (trazilac == null)
            {
                throw new Exception($"Nije nađen tražilac");
            }
            if (trazilac.Komentari == null)
            {
                return Ok(new List<Komentar>());
            }
            return Ok(trazilac.Komentari.ToList());
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
    [HttpPost("DodajKomentar")]
    public async Task<ActionResult> DodajKomentar([FromBody]JObject data)
    {
        try
        {
            string sadrzaj = data["sadrzaj"].ToString();
            var korisnik = await KorisnikController.CheckTokenAndGetKorisnik(Context, Request);
            if(korisnik == null)
            {
                return Unauthorized("Korisnik nije ulogovan");
            }
            if(!await _userManager.IsInRoleAsync(korisnik, "Trazilac") && !await _userManager.IsInRoleAsync(korisnik, "Administrator"))
            {
                return Unauthorized("Korisnik mora biti trazilac posla");
            }
            var trazilac = await Context.Trazioci.FindAsync(korisnik.Id);
            if (trazilac == null)
            {
                throw new Exception("trazilac je null");
            }
            Posao? posao = null;
            Komentar? komentar = null;
            if (data["posaoId"] != null)
            {
                int PosaoId = int.Parse(data["posaoId"].ToString());
                posao = await Context.Poslovi.Include(p=>p.Poslodavac).FirstOrDefaultAsync(p=>p.Id == PosaoId);
                if(posao == null) throw new Exception("Posao nije nađen");
            }
            else if (data["komentarId"] != null)
            {
                int KomentarId = int.Parse(data["komentarId"].ToString());
                komentar = await Context.Komentari.Include(p=>p.Autor).FirstOrDefaultAsync(p=>p.Id == KomentarId);
                if(komentar == null) throw new Exception("Komentar nije nađen");
                posao = komentar.Posao;
                await PosaljiNotifikaciju(komentar.Autor.Id, $"{trazilac.Email} je odgovorio na vas komentar {komentar.Sadrzaj}", "");
            }else throw new Exception("Posalji posaoId il komentarId");
            var noviKomentar = new Komentar()
            {
                Sadrzaj = sadrzaj,
                DatumSlanja = DateTime.Now,
                DatumEditovanja = null,
                Autor = trazilac,
                Posao = posao,
            };
            await Context.AddAsync(noviKomentar);
            if(trazilac.Komentari == null)
            {
                trazilac.Komentari = new List<Komentar>();
            }
            if (komentar != null)
            {
                if (komentar.Odgovori == null)
                {
                    komentar.Odgovori = new List<Komentar>();
                }
                komentar.Odgovori.Add(noviKomentar);
            }
            trazilac.Komentari.Add(noviKomentar);
            
            if(posao != null)
                await PosaljiNotifikacijuL(posao.Poslodavac, $"{trazilac.Email} je postavio komentar na poslu {posao.Naziv}", "");
            if(komentar != null)
                await PosaljiNotifikacijuL(komentar.Autor, $"{trazilac.Email} je postavio odgovor na komentar {komentar.Id}", "");

            await Context.SaveChangesAsync();
            return Ok(noviKomentar);
        }
        catch (Exception e)
        {
            
            return BadRequest(new{
                msg = e.Message,
                stack = e.StackTrace,
                src = e.Source,
                inner = e.InnerException
            });
        }
    }
    [HttpPut("IzmeniKomentar")]
    public async Task<ActionResult> IzmeniKomentar([FromBody]JObject data)
    {
        try
        {
            int KomentarID = int.Parse(data["KomentarId"].ToString());
            string IzmenjenSadrzaj = data["Sadrzaj"].ToString();
            var komentar = await Context.Komentari.FindAsync(KomentarID);
            if (komentar == null)
            {
                throw new Exception("Nije pronađen komentar");
            }
            var user = await KorisnikController.CheckTokenAndGetKorisnik(Context, Request);
            if (user == null)
            {
                return Unauthorized("Korisnik nije ulogovan");
            }
            if(komentar.Autor != user && !await _userManager.IsInRoleAsync(user, "Administrator"))
            {
                return Unauthorized("Korisnik sme da menja samo svoje komentare");
            }
            komentar.Sadrzaj = IzmenjenSadrzaj;
            komentar.DatumEditovanja = DateTime.Now;
            await Context.SaveChangesAsync();
            return Ok($"Izmenjen je komentar");
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
    [HttpGet("VratiKomentarePosla")]
    public async Task<IActionResult> VratiKomentarePosla([FromQuery]int PosaoId)
    {
        try
        {
            var komentari = await Context.Komentari.Include(p=>p.Autor).Where(p => p.Posao.Id == PosaoId).ToListAsync();
            return Ok(komentari);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }

    }
    [HttpGet("VratiKomentareKomentara")]
    public async Task<IActionResult> VratiKomentareKomentara([FromQuery]int KomentarId)
    {
        try
        {
            var komentari = await Context.Komentari
            .Where(p => p.Id == KomentarId)
            .Include(p => p.Odgovori)
            .ThenInclude(p => p.Autor)
            .Select(p => p.Odgovori)
            .ToListAsync();
            return Ok(komentari);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }
    [HttpPost("PosaljiNotifikaciju")]
    public async Task<IActionResult> PosaljiNotifikaciju(int KorisnikId, string Sadrzaj, string Link)
    {
        try
        {
            Notifikacija notifikacija = new(){
                Korisnik = await Context.Korisnici.FindAsync(KorisnikId),
                Sadrzaj = Sadrzaj,
                Link = Link
            };
            Context.Notifikacije.Add(notifikacija);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
            throw;
        }

    }
    [HttpPost("PosaljiNotifikacijuL")]
    public async Task<bool> PosaljiNotifikacijuL(Korisnik Korisnik, string Sadrzaj, string Link)
    {
        try
        {
            
            Context.Notifikacije.Add(
                new Notifikacija(){
                    Korisnik = Korisnik,
                    Sadrzaj = Sadrzaj,
                    Link = Link
                }
            );
            await Context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }

    }

    [HttpGet("VratiNotifikacije")]
    public async Task<IActionResult> VratiNotifikacije([FromQuery]int KorisnikId)
    {
        try
        {
            var notifikacije = await Context.Notifikacije.Where(p => p.Korisnik.Id == KorisnikId).Take(10).ToListAsync();
            return Ok(notifikacije);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex);
            throw;
        }
    }
    [HttpPut("VidiNotifikacije")]
    public async Task<IActionResult> VidiNotifikacije([FromQuery]params int[] NotifikacijeIds)
    {
        foreach (var id in NotifikacijeIds)
        {
            var notifikacija = await Context.Notifikacije.FindAsync(id);
            if (notifikacija == null)
            {
                continue;
            }
            notifikacija.Seen = true;
        }
        await Context.SaveChangesAsync();
        return Ok();
    }

}