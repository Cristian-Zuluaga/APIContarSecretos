public interface IAudioLibroService{
    Task<BaseMessage<AudioLibro>> FindById(int id);
    Task<BaseMessage<AudioLibro>> AddAudioLibro(RequestAudioLibroAddDTO audioLibro);
    Task<BaseMessage<ResponseAudioLibroDTO>> GetAll();
    Task<BaseMessage<AudioLibro>> UpdateAudioLibro(AudioLibro audioLibro);
    Task<BaseMessage<AudioLibro>> GetAllFilter(RequestFilterAudioLibroDTO requestFilterAudioLibroDTO);
}