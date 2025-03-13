using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class SecretosContext : IdentityDbContext<ApplicationUser> 
{
    public DbSet<Autor> Autores { get; set; }
    public DbSet<Libro> Libros { get; set; }
    public DbSet<AudioLibro> AudioLibros { get; set; }

    public SecretosContext(DbContextOptions<SecretosContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        if(builder == null)
        {
            return;
        }

        builder.Entity<Autor>().ToTable("Autor").HasKey(k => k.Id);
        builder.Entity<Libro>().ToTable("Libro").HasKey(k => k.Id);
        builder.Entity<AudioLibro>().ToTable("AudioLibro").HasKey(k => k.Id);
        base.OnModelCreating(builder);
    }

}