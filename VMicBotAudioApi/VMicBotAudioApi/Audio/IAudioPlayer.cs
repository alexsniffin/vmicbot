using NAudio.Wave;

namespace VMicBotAudioApi.Audio
{
    public interface IAudioPlayer
    {
        void Play(string song);
        void Stop();
        bool Resume();
        bool Pause();
    }
}