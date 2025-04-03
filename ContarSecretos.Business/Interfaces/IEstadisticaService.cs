public interface IEstadisticaService
{
    Task<BaseMessage<Estadistica>> AddEstadistica(Estadistica estadistica);
    Task<BaseMessage<Estadistica>> UpdateEstadistica(Estadistica estadistica);
    Task<bool> DeleteEstadistica(int? audioLibroId,int? libroId);
    Task<BaseMessage<Estadistica>> GetEstadistica(int? audioLibroId,int? libroId);
}