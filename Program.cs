var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(options => 
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ProjectContext>(options =>
{
    if (OperatingSystem.IsWindows())
        options.UseSqlServer(builder.Configuration.GetConnectionString("ProjectCS"));
    else
            options.UseSqlServer(builder.Configuration.GetConnectionString("ProjectCS2"));

});

builder.Services.AddMvc();

builder.Services.AddControllers().AddJsonOptions(
    p => p.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddIdentity<Korisnik, IdentityRole<int>>(options =>{
    // Password opcije
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    // User opcije
    options.SignIn.RequireConfirmedEmail = true;
    options.User.RequireUniqueEmail = false;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    // Lockout opcije
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
}).AddEntityFrameworkStores<ProjectContext>()
.AddDefaultTokenProviders()
.AddRoles<IdentityRole<int>>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.Configure<ProjectContext>(o =>
{
  // Make sure the identity database is createdy
  o.Database.Migrate();
});
builder.Services.Configure<PasswordHasherOptions>(options =>options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2);

builder.Services.AddTransient<IEmailSender, EmailSender>();


Task CreateRoles(IServiceProvider serviceProvider)
    {
        var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
        var UserManager = serviceProvider.GetRequiredService<UserManager<Korisnik>>();
        string[] roleNames = { "Administrator", "Trazilac", "Poslodavac" };
        Task<IdentityResult> roleResult;

        foreach (var roleName in roleNames)
        {
            var roleExist = RoleManager.RoleExistsAsync(roleName);
            if (!roleExist.Result)
            {
                roleResult = RoleManager.CreateAsync(new IdentityRole<int>(roleName));
                roleResult.Wait();
            }
        }
        return Task.CompletedTask;
    }




builder.Services.AddCors(options =>
{
    options.AddPolicy("CORS", policy =>
    {
        policy.WithOrigins("https://localhost",
                           "http://localhost")
                .AllowAnyHeader()
                .AllowAnyMethod();
    });
});


var app = builder.Build();
using(var scope = app.Services.CreateScope())
{
    await CreateRoles(scope.ServiceProvider);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CORS");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseDefaultFiles();
app.UseStaticFiles();

app.Run();
