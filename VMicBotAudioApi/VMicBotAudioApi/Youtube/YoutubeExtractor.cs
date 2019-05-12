using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediaToolkit;
using MediaToolkit.Model;
using VideoLibrary;
using YoutubeExplode;
using YoutubeExplode.Models.MediaStreams;

namespace VMicBotAudioApi.Youtube
{
    public class YoutubeExtractor : IYoutubeExtractor
    {
        private const string VideoPath = @"D:\vmicbot\videosTemp\";
        private const string Mp3Path = @"D:\vmicbot\mp3\";

        private readonly Engine _engine;
        private readonly YoutubeClient _youtubeClient;

        public YoutubeExtractor(Engine engine, YoutubeClient youtubeClient)
        {
            _engine = engine;
            _youtubeClient = youtubeClient;
        }
        
        public async Task<string> Extract(string video)
        {
            var id = YoutubeClient.ParseVideoId(video);

            if (!File.Exists($"{Mp3Path}{id}.mp3"))
            {
                var streamInfoSet = await _youtubeClient.GetVideoMediaStreamInfosAsync(id);
                var streamInfo = streamInfoSet.Audio.WithHighestBitrate();
                var ext = streamInfo.Container.GetFileExtension();

                await _youtubeClient.DownloadMediaStreamAsync(streamInfo, $"{VideoPath}temp.{ext}");

                var inputFile = new MediaFile {Filename = $"{VideoPath}temp.{ext}"};
                var outputFile = new MediaFile {Filename = $"{Mp3Path}{id}.mp3"};

                using (var engine = _engine)
                {
                    engine.GetMetadata(inputFile);
                    engine.Convert(inputFile, outputFile);
                }
            }

            return id;
        }
    }
}