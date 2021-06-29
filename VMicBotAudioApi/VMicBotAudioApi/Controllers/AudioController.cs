using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VMicBotAudioApi.Youtube;
using VMicBotAudioApi.Audio;


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
        
        [HttpGet]
        [Route("pause")]
        public ActionResult<object> Pause()
        {
            var result = _audioPlayer.Pause();
            
            if (result)
            {
                return new AcceptedResult();
            }

            return new BadRequestResult();
        }
        
        [HttpGet]
        [Route("resume")]
        public ActionResult<object> Resume()
        {
            var result = _audioPlayer.Resume();

            if (result)
            {
                return new AcceptedResult();
            }

            return new BadRequestResult();
        }
    }
}