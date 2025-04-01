public interface IAudioLibroService{
    Task<BaseMessage<ResponseAudioLibroDTO>> FindById(int id);
    Task<BaseMessage<AudioLibro>> AddAudioLibro(RequestAudioLibroDTO audioLibro);
    Task<BaseMessage<ResponseAudioLibroDTO>> GetAll();
    Task<BaseMessage<AudioLibro>> UpdateAudioLibro(RequestAudioLibroDTO audioLibro);
    Task<BaseMessage<ResponseAudioLibroDTO>> GetAllFilter(RequestFilterAudioLibroDTO requestFilterAudioLibroDTO);
    Task<bool> DeleteAudioLibro(int id);
}