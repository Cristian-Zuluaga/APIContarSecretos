public interface IAudioLibroService{
    Task<BaseMessage<AudioLibro>> FindById(int id);
    Task<BaseMessage<AudioLibro>> AddAudioLibro(RequestAudioLibroAddDTO audioLibro);
    Task<BaseMessage<AudioLibro>> GetAll();
    Task<BaseMessage<AudioLibro>> UpdateAudioLibro(AudioLibro audioLibro);
}