using System.Threading.Tasks;
using VideoLibrary;
using YoutubeExplode;

namespace VMicBotAudioApi.Youtube
{
    public interface IYoutubeExtractor
    {
        Task<string> Extract(string video);
    }
}