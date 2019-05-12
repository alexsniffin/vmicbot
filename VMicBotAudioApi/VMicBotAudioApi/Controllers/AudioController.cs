using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NAudio.Wave;
using VMicBotAudioApi.Youtube;
using NAudio.Wave.WaveFormats;
using VMicBotAudioApi.Audio;
using YoutubeExplode;
using YoutubeExplode.Models.MediaStreams;

namespace VMicBotAudioApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AudioController : ControllerBase
    {
        private readonly IYoutubeExtractor _youtubeExtractor;
        private readonly IAudioPlayer _audioPlayer;

        public AudioController(IYoutubeExtractor youtubeExtractor, IAudioPlayer audioPlayer)
        {
            _youtubeExtractor = youtubeExtractor;
            _audioPlayer = audioPlayer;
        }
        
        [HttpGet]
        [Route("play")]
        public async Task<ActionResult<object>> Play([FromQuery] string link)
        {
            var id = await _youtubeExtractor.Extract(link);
            _audioPlayer.Play($@"D:\vmicbot\mp3\{id}.mp3");
            
            return new 
            {
                Status = "OK",
            };
        }
        
        [HttpGet]
        [Route("stop")]
        public ActionResult<object> Stop()
        {
            _audioPlayer.Stop();
            
            return new 
            {
                Status = "OK"
            };
        }
    }
}