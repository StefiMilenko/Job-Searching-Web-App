namespace Controllers;
using System.Net;
using Newtonsoft.Json;

[ApiController]
[Route("[controller]")]
public class PosaoController : ControllerBase
{
    public ProjectContext Context { get; set; }
    private readonly UserManager<Korisnik> _userManager;

    public PosaoController(ProjectContext context, UserManager<Korisnik> userManager)
    {
        Context = context;
        _userManager = userManager;
    }

    [HttpPost("DodajPosao")]
    public async Task<ActionResult> DodajPosao([FromBody]Posao posao)
    {
        var poslodavac = await KorisnikController.CheckTokenAndGetKorisnik(Context, Request);
        if (poslodavac == null)
        {
            return Unauthorized("Korisnik nije ulogovan");
        }
        if(!await _userManager.IsInRoleAsync(poslodavac, "Poslodavac"))
        {
            return Unauthorized("Korisnik mora biti poslodavac");
        }

        string nominatimUrl = $"https://nominatim.openstreetmap.org/search?format=json&q={Uri.EscapeDataString(posao.Lokacija)}";
        using (WebClient client = new WebClient())
        {
            client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537");
            string nominatimJson = client.DownloadString(nominatimUrl);
            NominatimResponse[] responses = JsonConvert.DeserializeObject<NominatimResponse[]>(nominatimJson);

            if (responses.Length > 0)
            {
                NominatimResponse response = responses[0];
                posao.latitude = response.Latitude;
                posao.longitude = response.Longitude;

                Console.WriteLine($"Latitude: {posao.latitude}");
                Console.WriteLine($"Longitude: {posao.longitude}");
            }
            else {
                return BadRequest($"Lokacija nije nadjena");
            }
        }

        if (poslodavac!= null)
        {
            try
            {
                posao.Poslodavac = poslodavac as Poslodavac;
                await Context.Poslovi.AddAsync(posao);
                await Context.SaveChangesAsync();
                return Ok(posao.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        return BadRequest("Nije pronađen korisnik");
    }

    [HttpPut("IzmeniPosao")]
    public async Task<ActionResult> IzmeniPosao([FromBody]Posao posao)
    {
        try
        {
            var stariPosao = await Context.Poslovi.FindAsync(posao.Id);
            
            if (stariPosao != null)
            {
                var korisnik = await KorisnikController.CheckTokenAndGetKorisnik(Context, Request);
                if (korisnik == null)
                {
                    return Unauthorized("Korisnik nije ulogovan");
                }
                if(korisnik != posao.Poslodavac && !await _userManager.IsInRoleAsync(korisnik, "Administrator"))
                {
                    return Unauthorized();
                }
                stariPosao.Naziv = posao.Naziv;
                stariPosao.Lokacija = posao.Lokacija;
                string nominatimUrl = $"https://nominatim.openstreetmap.org/search?format=json&q={Uri.EscapeDataString(posao.Lokacija)}";
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537");
                    string nominatimJson = client.DownloadString(nominatimUrl);
                    NominatimResponse[] responses = JsonConvert.DeserializeObject<NominatimResponse[]>(nominatimJson);

                    if (responses.Length > 0)
                    {
                        NominatimResponse response = responses[0];
                        stariPosao.latitude = response.Latitude;
                        stariPosao.longitude = response.Longitude;

                        Console.WriteLine($"Latitude: {stariPosao.latitude}");
                        Console.WriteLine($"Longitude: {stariPosao.longitude}");
                    }
                    else {
                        return BadRequest($"Lokacija nije nadjena");
                    }
                }
                Context.Poslovi.Update(stariPosao);
            }

            await Context.SaveChangesAsync();
            return Ok(posao.Id);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("ObrisiPosao")]
    public async Task<ActionResult> ObrisiPosao([FromQuery]int PosaoId)
    {
        try
        {
            var posao = await Context.Poslovi.FindAsync(PosaoId);
            var korisnik = await KorisnikController.CheckTokenAndGetKorisnik(Context, Request);
            if (korisnik == null)
            {
                return Unauthorized("Korisnik nije ulogovan");
            }
            if (posao != null && 
            (posao.Poslodavac == korisnik || await _userManager.IsInRoleAsync(korisnik, "Administrator")))
            {
                Context.Poslovi.Remove(posao);
                await Context.SaveChangesAsync();
                return Ok($"Posao Obrisan");
            }
            else
            {
                return BadRequest($"Nije pronađen posao");
            }
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }


    [HttpGet("VratiPosao")]
    public async Task<ActionResult> VratiPosao([FromQuery]int PosaoId)
    {
        try
        {
            var posao = await Context.Poslovi
            .Where(p => p.Id == PosaoId)
            .Select(p => new{
                p.Id,
                p.Naziv,
                p.Lokacija,
                p.PlataOd,
                p.PlataDo,
                p.PlataValuta,
                p.PlataInfo,
                p.SektorRada,
                p.Industrija,
                p.VrstaPosla,
                p.TipPosla,
                p.OpisPosla,
                p.Komentari,
                p.Ocene,
                PoslodavacId = p.Poslodavac.Id,
                p.Poslodavac.NazivFirme,
                p.Poslodavac.Opis
            })
            .SingleOrDefaultAsync();
            if (posao != null) 
            {
                return Ok(posao);
            }
            else
            {
                return BadRequest($"Nije pronađen posao");
            }
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("PreuzmiPoslove/{strana}")]
    //Za sada pretrazuje po stvarima u FilterPosao.cs i nije case sensitive 
    public async Task<ActionResult> PreuzmiPoslove([FromQuery]string? query,
                                                    [FromQuery]string? firma,
                                                    [FromQuery]int? plataOd,
                                                    [FromQuery]int? plataDo,
                                                    [FromQuery]string? plataValuta,
                                                    [FromQuery]string? plataInfo,
                                                    [FromQuery]string? sektorRada,
                                                    [FromQuery]string? industrija,
                                                    [FromQuery]string? vrstaPosla,  //junior senior ..
                                                    [FromQuery]string? tipPosla, //freelance, permanent etc.
                                                    [FromQuery]string? Lokacija,
                                                    [FromQuery]int? Udaljenost, //u km
                                                    [FromRoute]int strana /*od 1 pa na dalje*/)
    {
        try
        {
            int pageSize = 20;
            var data = Context.Poslovi.AsQueryable();
            
            if (query != null)
                data = data.Where(p =>  p.Naziv.ToLower().Contains(query.ToLower()));
            
            if (firma != null)
                data = data.Where(p => p.Poslodavac.NazivFirme.ToLower().Contains(firma.ToLower()));
            

            if((plataDo==500000 && plataOd == 0)==false){
                if (plataOd != null)
                    data = data.Where(p => plataOd <= p.PlataDo );
                if (plataDo != null)
                    data = data.Where(p => plataDo >= p.PlataOd );
            }
            if (plataValuta != null && plataValuta.ToLower()!="any")
                data = data.Where(p => p.PlataValuta == plataValuta);
            if (plataInfo != null)
                data = data.Where(p => p.PlataInfo == plataInfo);
            if (sektorRada != null)
                data = data.Where(p => p.SektorRada == sektorRada);
            if (industrija != null)
                data = data.Where(p => p.Industrija == industrija);
            if (vrstaPosla != null && vrstaPosla.ToLower()!="any")
                data = data.Where(p => p.VrstaPosla == vrstaPosla);
            if (tipPosla != null && tipPosla.ToLower()!="any")
                data = data.Where(p => p.TipPosla == tipPosla);

            if (Lokacija != null && Lokacija!="" && Udaljenost != null && Udaljenost != 0 && Udaljenost != 2000)
            {
                Console.WriteLine($"Udaljenost: {Udaljenost}");
                try 
                {
                    if (Lokacija == null){
                        return BadRequest($"Morate ukucati željenu lokaciju");
                    }
                    string nominatimUrl = $"https://nominatim.openstreetmap.org/search?format=json&q={Uri.EscapeDataString(Lokacija)}";
                    using (WebClient client = new WebClient())
                    {
                        client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537");
                        string nominatimJson = client.DownloadString(nominatimUrl);
                        NominatimResponse[] responses = JsonConvert.DeserializeObject<NominatimResponse[]>(nominatimJson);

                        if (responses.Length > 0)
                        {
                            NominatimResponse response = responses[0];
                            var latitude = response.Latitude;
                            var longitude = response.Longitude;

                            Console.WriteLine($"Latitude: {latitude}");
                            Console.WriteLine($"Longitude: {longitude}");
                            List<JObject> filtered = new List<JObject>();
                            await data.Include(p => p.Poslodavac).ForEachAsync(p => {
                                if (CalculateDistance(latitude, longitude, (double)p.latitude, (double)p.longitude) <= Udaljenost){
                                    
                                    JObject jobject = JObject.FromObject(
                                        new{
                                            Id = p.Id,
                                            Naziv = p.Naziv,
                                            Lokacija = p.Lokacija,
                                            PlataOd = p.PlataOd,
                                            PlataDo = p.PlataDo,
                                            PlataValuta = p.PlataValuta,
                                            PlataInfo = p.PlataInfo,
                                            SektorRada = p.SektorRada,
                                            Industrija = p.Industrija,
                                            VrstaPosla = p.VrstaPosla,
                                            OpisPosla = p.OpisPosla,
                                            TipPosla = p.TipPosla,
                                            ProsecnaOcena = p.ProsecnaOcena,
                                            PoslodavacId = p.Poslodavac?.Id,
                                            NazivFirme = p.Poslodavac?.NazivFirme,
                                            Opis = p.Poslodavac?.Opis 
                                        }
                                    );
                                    filtered.Add(jobject);
                                }
                            });
                            return Ok(filtered.OrderByDescending(p => p["ProsecnaOcena"]).Skip((strana - 1) * pageSize).Take(pageSize));
                            //.Where(p=> CalculateDistance(latitude, longitude, (double)p.latitude, (double)p.longitude) <= Udaljenost);
                        }
                    }
                    
                }
                catch (Exception e)
                {
                    return BadRequest(e.ToString());
                }
               
            }
            var pageContent = await data.Select(p => new
            {
                Id = p.Id,
                Naziv = p.Naziv,
                Lokacija = p.Lokacija,
                PlataOd = p.PlataOd,
                PlataDo = p.PlataDo,
                PlataValuta = p.PlataValuta,
                PlataInfo = p.PlataInfo,
                SektorRada = p.SektorRada,
                Industrija = p.Industrija,
                VrstaPosla = p.VrstaPosla,
                OpisPosla = p.OpisPosla,
                TipPosla = p.TipPosla,
                ProsecnaOcena = p.ProsecnaOcena,
                PoslodavacId = p.Poslodavac.Id,
                Poslodavac = p.Poslodavac,
                NazivFirme = p.Poslodavac.NazivFirme,
                Opis = p.Poslodavac.Opis
            })
            .OrderByDescending(p => p.ProsecnaOcena)
            .Skip((strana - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
            return Ok(pageContent);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("VratiPoslovePoslodavca")]
    public async Task<ActionResult> VratiPoslovePoslodavca([FromQuery]int PoslodavacId)
    {
        try
        {
            var poslodavac = await Context.Poslodavci.Where(p => p.Id == PoslodavacId) //TODO ne ukljucivati password hash i ostalo
                                            .Include(p => p.Poslovi)
                                            .SingleOrDefaultAsync();
            if (poslodavac == null)
            {
                throw new Exception($"Nije nađen poslodavac");
            }
            if (poslodavac.Poslovi == null)
            {
                poslodavac.Poslovi = new List<Posao>();
            }
            return Ok(poslodavac.Poslovi.ToList());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    [HttpPut("DodajOcenu")]
    public async Task<IActionResult> DodajOcenu([FromQuery]int PosaoId,
                                                [FromQuery]int ocenaPlata, 
                                                [FromQuery]int ocenaOkruzenje,
                                                [FromQuery]int ocenaNapredovanje,
                                                [FromQuery]int ocenaPosoKuca )
    {
        try
        {
            var posao = await Context.Poslovi.Where(p => p.Id == PosaoId).Include(p => p.Ocene).SingleOrDefaultAsync();
            if (posao == null)
            {
                throw new Exception("Posao nije pronađen.");
            }
            var korisnik = await KorisnikController.CheckTokenAndGetKorisnik(Context, Request);
            if (korisnik == null)
            {
                return Unauthorized("Korisnik nije ulogovan");
            }
            if (await _userManager.IsInRoleAsync(korisnik, "Poslodavac"))
            {
                throw new Exception("Korisnik ne sme biti poslodavac.");
            }
            
            if (posao.Ocene == null)
            {
                posao.Ocene = new List<Ocena>();
            }

            int brOcena = posao.Ocene.Count;
            float trenutnaProsecnaOcena = posao.ProsecnaOcena;
            float zbirOcena = trenutnaProsecnaOcena * brOcena;   
            float ocenaVrednost = (ocenaPlata + ocenaNapredovanje + ocenaOkruzenje + ocenaPosoKuca) / 4;
            
            var postojecaOcena = await Context.Ocene
            .Where(p => p.Posao.Id == posao.Id && p.Korisnik.Id == korisnik.Id).SingleOrDefaultAsync();
            if(postojecaOcena != null)
            {
                zbirOcena -= (float)postojecaOcena.AvgVal;
                postojecaOcena.Plata = ocenaPlata;
                postojecaOcena.Okruzenje = ocenaOkruzenje;
                postojecaOcena.Napredovanje = ocenaNapredovanje;
                postojecaOcena.PosoKuca = ocenaPosoKuca;
                zbirOcena += ocenaVrednost;
                posao.ProsecnaOcena = zbirOcena / brOcena;
                Context.Ocene.Update(postojecaOcena);
                Context.Poslovi.Update(posao);
                await Context.SaveChangesAsync();
                return Ok("Ocena izmenjena");
            }

            var ocena = new Ocena()
            {
                Korisnik = korisnik,
                AvgVal= ocenaVrednost,
                Plata = ocenaPlata,
                Okruzenje = ocenaOkruzenje,
                Napredovanje = ocenaNapredovanje,
                PosoKuca = ocenaPosoKuca
            };

            ocena.Korisnik = korisnik;
            var task = Context.Ocene.AddAsync(ocena);
            
            posao.Ocene.Add(ocena);
            zbirOcena += ocena.AvgVal;
            posao.ProsecnaOcena = zbirOcena / (brOcena + 1);
            await task;
            Context.Poslovi.Update(posao);
            await Context.SaveChangesAsync();
            return Ok(ocena);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex);
            throw;
        }
    }
    [HttpGet("VratiOcenuKorisnika")]
    public async Task<IActionResult> VratiOcenuKorisnika([FromQuery]int PosaoId, [FromQuery]int KorisnikId)
    {
        try
        {
            var korisnik = await Context.Trazioci.FindAsync(KorisnikId); 
            var ocena = (await Context.Ocene
            .Where(p => p.Korisnik.Id == korisnik.Id && p.Posao.Id == PosaoId).SingleOrDefaultAsync());
            return Ok(ocena);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex);
            throw;
        }
    }
    [HttpGet("VratiOcene")]
    public async Task<IActionResult> VratiOcene([FromQuery]int PosaoId)
    {
        try
        {
            //var korisnik = await Context.Trazioci.FindAsync(KorisnikId); 
            var korisnik = await KorisnikController.CheckTokenAndGetKorisnik(Context, Request);
            
            var ocena = (korisnik == null)?null: (await Context.Ocene
            .Where(p => p.Korisnik.Id == korisnik.Id && p.Posao.Id == PosaoId).SingleOrDefaultAsync());
            var ocene = (await Context.Ocene
            .Where(p => p.Posao.Id == PosaoId).ToListAsync());
            float prosecna = (ocene.Count==0)?float.NaN:ocene.Average(p=>p.AvgVal);
            return Ok(new {
                prosecna = prosecna,
                sve = ocene,
                moj = ocena
            });
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex);
            throw;
        }
    }
    [HttpGet("VratiUdaljenostTackeIPosla")]
    public async Task<IActionResult> VratiUdaljenostTackeIPosla(double lat1, double lon1, [FromQuery]int PosaoId){
        var posao = (await Context.Poslovi.Where(p => p.Id == PosaoId).SingleOrDefaultAsync());
        return Ok(CalculateDistance(lat1, lon1, (double)posao.latitude, (double)posao.longitude));
    }

    [HttpGet("VratiUdaljenostDveTacke")]
    public async Task<IActionResult> VratiUdaljenostDveTacke(double lat1, double lon1, double lat2, double lon2){
        return Ok(CalculateDistance(lat1, lon1, lat2, lon2));
    }
    static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double radius = 6371; // Radius of the Earth in kilometers

        double dLat = ToRadians(lat2 - lat1);
        double dLon = ToRadians(lon2 - lon1);

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        double distance = radius * c;

        return distance;
    }

    static double ToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }

}

class NominatimResponse
{
    [JsonProperty("lat")]
    public double Latitude { get; set; }

    [JsonProperty("lon")]
    public double Longitude { get; set; }
}