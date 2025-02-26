using Microsoft.EntityFrameworkCore;

public class UnitOfWork : IUnitOfWork
{
    private readonly SecretosContext _context;
    private IRepository<int, Autor> _autorRepository;
    private IRepository<int, Libro> _libroRepository;
    private IRepository<int, AudioLibro> _audioLibroRepository;
    private bool _disposed = false;

    public UnitOfWork(SecretosContext context)
    {
        _context = context;
    }


    public IRepository<int, Autor> AutorRepository
    {
        get
        {
            _autorRepository ??= new Repository<int, Autor>(_context);
            return _autorRepository;
        }
    }

    public IRepository<int, Libro> LibroRepository
    {
        get
        {
            _libroRepository ??= new Repository<int, Libro>(_context);
            return _libroRepository;
        }
    }

    public IRepository<int, AudioLibro> AudioLibroRepository
    {
        get
        {
            _audioLibroRepository ??= new Repository<int, AudioLibro>(_context);
            return _audioLibroRepository;
        }
    }

    public async Task SaveAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            ex.Entries.Single().Reload();
        }
    }

    #region IDisposable
    protected virtual void Dispose(bool disposing)
    {
        if(!_disposed)
        {
            if(disposing)
            {
                _context.DisposeAsync();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
    }
    #endregion
}