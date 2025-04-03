using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Inyecciones
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IAutorService,AutorService>();
builder.Services.AddScoped<ILibroService,LibroService>();
builder.Services.AddScoped<IAudioLibroService,AudioLibroService>();
builder.Services.AddScoped<IEstadisticaService,EstadisticaService>();
builder.Services.AddScoped<IFileService,FileService>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<SecretosContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWTConfig:ValidAudience"],
        ValidIssuer = builder.Configuration["JWTConfig:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTConfig:SecretKey"]))
    };
});



//Inyeccion de contexto - conexion sql
builder.Services.AddDbContext<SecretosContext>(
    opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("SecretosBD"))
);


var app = builder.Build();

//Llamado a la carga de datos de bd
PopulateDB(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

#region PopulateDB
async void PopulateDB(WebApplication app)
{
    using(var scope = app.Services.CreateScope())
    {
        var seedMain = scope.ServiceProvider.GetRequiredService<IUserService>();
        await seedMain.SeedAdmin();
    }
}
#endregion