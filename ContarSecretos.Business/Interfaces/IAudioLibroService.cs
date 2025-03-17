public interface IAudioLibroService{
    Task<BaseMessage<AudioLibro>> FindById(int id);
    Task<BaseMessage<AudioLibro>> AddAudioLibro(AudioLibro audioLibro);
    Task<BaseMessage<AudioLibro>> GetAll();
    Task<BaseMessage<AudioLibro>> UpdateAudioLibro(AudioLibro audioLibro);
}