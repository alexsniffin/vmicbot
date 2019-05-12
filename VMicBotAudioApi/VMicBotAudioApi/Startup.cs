using MediaToolkit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NAudio.Wave;
using VMicBotAudioApi.Audio;
using VMicBotAudioApi.Youtube;
using YoutubeExplode;

namespace VMicBotAudioApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IAudioPlayer, AudioPlayer>();
            services.AddScoped<IYoutubeExtractor, Youtube.YoutubeExtractor>();
            services.AddSingleton(new Engine(@"D:\vmicbot\FFmpegLib\bin\ffmpeg.exe"));
            services.AddSingleton(new YoutubeClient());
            services.AddSingleton(new WaveOutEvent());
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}