using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MediaToolkit;
using MediaToolkit.Model;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

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
            var uri = new Uri(video);
            var query = HttpUtility.ParseQueryString(uri.Query);
            var videoId = string.Empty;
            if (query.AllKeys.Contains("v"))
            {
                videoId = query["v"];
            }
            else
            {
                videoId = uri.Segments.Last();
            }

            if (!File.Exists($"{Mp3Path}{videoId}.mp3"))
            {
                var streamManifest = await _youtubeClient.Videos.Streams.GetManifestAsync(video);
                var streamInfo = streamManifest.GetAudioStreams().GetWithHighestBitrate();
                await _youtubeClient.Videos.Streams.DownloadAsync(streamInfo, $"{VideoPath}temp.mp3");

                var inputFile = new MediaFile {Filename = $"{VideoPath}temp.mp3"};
                var outputFile = new MediaFile {Filename = $"{Mp3Path}{videoId}.mp3"};

                using (var engine = _engine)
                {
                    engine.GetMetadata(inputFile);
                    engine.Convert(inputFile, outputFile);
                }
            }

            return videoId;
        }
    }
}