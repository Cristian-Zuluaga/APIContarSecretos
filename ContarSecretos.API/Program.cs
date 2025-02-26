using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


//Inyecciones
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
builder.Services.AddScoped<IAutorService,AutorService>();
builder.Services.AddScoped<ILibroService,LibroService>();
builder.Services.AddScoped<IAudioLibroService,AudioLibroService>();


builder.Services.AddDbContext<SecretosContext>(
    opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("SecretosBD"))
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

