namespace Controllers;
[ApiController]
[Route("[controller]")]
public class AccountController : Controller
{

    public ProjectContext Context { get; set; }
    private readonly IMapper _mapper;
    private readonly UserManager<Korisnik> _userManager;
    private readonly SignInManager<Korisnik> _signInManager;
    private readonly ILogger<AccountController> _logger;
    private readonly IEmailSender _emailSender;
    
    public AccountController(IMapper mapper,
        UserManager<Korisnik> userManager,
        SignInManager<Korisnik> signInManager,
        ProjectContext context, 
        ILogger<AccountController> logger,
        IEmailSender emailSender)
    {
        _mapper = mapper;
        _userManager = userManager;
        _signInManager = signInManager;
        Context = context;
        _logger = logger; 
        _emailSender = emailSender;
    }

    [HttpGet("Register")]
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost("Register")]

    public async Task<IActionResult> Register([FromBody]KorisnikRegistration userModel)
    {
        try
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Model state is invalid");
            }
        if (userModel.Uloga == Role.Trazilac){
            Korisnik _u = _mapper.Map<Korisnik>(userModel);
            Trazilac user = new Trazilac()
            {
                Ime = userModel.Ime,
                Prezime = userModel.Prezime,
                Drzava = userModel.Drzava,
                Email = userModel.Email,
                Biografija = userModel.Biografija ?? "",
                CV = userModel.CV,
                StrucnaSprema = userModel.StrucnaSprema,

                UserName = _u.UserName,
                NormalizedUserName = _u.NormalizedUserName,
                NormalizedEmail = _u.NormalizedEmail
            };
            var result = await _userManager.CreateAsync(user, userModel.Password);
            if(!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState.Select(p => p.Value?.Errors.Select(e => e.ErrorMessage).First()).First());
            }
            await _userManager.AddToRoleAsync(user, "TRAZILAC");
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);
            await _emailSender.SendEmailAsync(user.Email, "Link za Email", confirmationLink);
            return Ok($"Uspešno registrovan akaunt \'{user.UserName}\'");
        }


        else if (userModel.Uloga == Role.Poslodavac){
            Korisnik _u = _mapper.Map<Korisnik>(userModel);
            Poslodavac user = new Poslodavac()
            {
                Ime = userModel.Ime,
                Prezime = userModel.Prezime,
                Drzava = userModel.Drzava,
                Email = userModel.Email,
                Biografija = userModel.Biografija ?? "",
                NazivFirme = userModel.NazivFirme,
                Opis = userModel.Opis,

                UserName = _u.UserName,
                NormalizedUserName = _u.NormalizedUserName,
                NormalizedEmail = _u.NormalizedEmail
            };
            var result = await _userManager.CreateAsync(user, userModel.Password);
            if(!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState.Select(p => p.Value?.Errors.Select(e => e.ErrorMessage).First()).First());
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);
            await _emailSender.SendEmailAsync(user.Email, "Link za Email", confirmationLink);
            await _userManager.AddToRoleAsync(user, "POSLODAVAC");
            return Ok($"Uspešno registrovan akaunt \'{user.UserName}\'");
        }
        else{
            Korisnik user = _mapper.Map<Korisnik>(userModel);
            await _userManager.AddToRoleAsync(user, "ADMINISTRATOR");
            return Ok("Administrator kreiran");
        }
        }
        
    
        
        catch (System.Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpGet("Login")]
    public IActionResult Login()
    {
        //return View();//
        return Ok();
    }
    
    [HttpPost("Login")]  
   // [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([FromBody]UserLogin userModel)
    {   
        
        if(!ModelState.IsValid)
        {
            //return View(userModel);//
             return BadRequest("Model state is invalid");
        }
        try
        {
            var user = await _userManager.FindByNameAsync(userModel.Email);
            if(user == null || !await _userManager.CheckPasswordAsync(user, userModel.Password))
            {
                return Unauthorized("Pogrešna šifra ili e-mail.");
            }
            var confirmed = await _userManager.IsEmailConfirmedAsync(user); //Da li je email potvrdjen
            if (!confirmed)
            {
                return Unauthorized("Nije potvrđen e-mail");
            }
            TimeSpan expireTime =  TimeSpan.FromMinutes(30);
            var token = new Models.AuthenticationToken()
            {
                Korisnik = user,
                ExpireTime = DateTime.UtcNow + expireTime
            };
            user.Tokens.Add(token);
            await Context.AuthenticationTokens.AddAsync(token);
            await Context.SaveChangesAsync();
            HttpContext.Response
            .Cookies
            .Append("authentication-token", token.TokenString, new CookieOptions()
            {
                Expires = DateTime.Now.AddMinutes(30),
                Path = "/",
            });
            HttpContext.Response.ContentType = "text/plain";
            var bytes = System.Text.Encoding.UTF8.GetBytes("Korisnik uspešno ulogovan");
            HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            await HttpContext.Response.Body.WriteAsync(bytes);

            await HttpContext.Response.CompleteAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            //return View();//
            return BadRequest(ex);
        }
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (userId == null || token == null)
        {
            return BadRequest("userId ili token su null");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            ViewBag.ErrorMessage = $"The User ID {userId} is invalid";
            return BadRequest("Not Found");
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
        {
            return Ok("Confirmed email");
        }

        ViewBag.ErrorTitle = "Email cannot be confirmed";
        return BadRequest("Errorce");
    }

    [HttpPost("Logout")]  
    public async Task<IActionResult> Logout()
    {
        if(!ModelState.IsValid)
        {
            //return View(userModel);//
             return BadRequest("Model state is invalid");
        }
        try
        {

            var korisnik = await Context.AuthenticationTokens
            .Where(p => p.TokenString == Request.Cookies["authentication-token"])
            .Select(p => p.Korisnik)
            .FirstOrDefaultAsync();

            if(korisnik == null) throw new Exception("Nije pronađen korisnik");
            var tokens = Context.AuthenticationTokens
            .Where(p => p.Korisnik.NormalizedUserName == korisnik.NormalizedUserName);
            foreach(var token in tokens)
            {
                Context.AuthenticationTokens.Remove(token);
            }
            await Context.SaveChangesAsync();

            

            HttpContext.Response.Cookies.Delete("authentication-token");
  
            HttpContext.Response.StatusCode = StatusCodes.Status200OK;

            await HttpContext.Response.CompleteAsync();
            return Ok("Izlogovani ste");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            //return View();//
            return BadRequest(ex);
        }
    }
}
