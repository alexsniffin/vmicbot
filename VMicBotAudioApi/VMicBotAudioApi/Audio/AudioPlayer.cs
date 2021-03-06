using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using NAudio.Wave;

namespace VMicBotAudioApi.Audio
{
    public class AudioPlayer : IAudioPlayer
    {
        private readonly WaveOutEvent _outputDevice;

        private Thread _currentSongThread;

        public AudioPlayer(WaveOutEvent outputDevice)
        {
            _outputDevice = outputDevice;
            
            for (var n = -1; n < WaveOut.DeviceCount; n++)
            {
                var caps = WaveOut.GetCapabilities(n);
                
                if (caps.ProductName.StartsWith("Voice"))
                {
                    _outputDevice.DeviceNumber = n;
                }
                if (n == WaveOut.DeviceCount) throw new Exception("Failed to find correct audio playback device");
            }
        }

        public void Play(string song)
        {
            if (_outputDevice.PlaybackState == PlaybackState.Playing)
            {
                _outputDevice.Stop();
            }
            
            _outputDevice.Pause();

            _currentSongThread = new Thread(() =>
            {
                using(var audioFile = new AudioFileReader(song))
                using(_outputDevice)
                {
                    _outputDevice.Init(audioFile);
                    _outputDevice.Play();
                    while (_outputDevice.PlaybackState == PlaybackState.Playing || _outputDevice.PlaybackState == PlaybackState.Paused)
                    {
                        Thread.Sleep(1000);
                    }
                }
            });
            _currentSongThread.Start();
        }

        public bool Pause()
        {
            if (_outputDevice.PlaybackState == PlaybackState.Playing)
            {
                _outputDevice.Pause();
                return true;
            }

            return false;
        }
        
        public bool Resume()
        {
            if (_outputDevice.PlaybackState == PlaybackState.Paused)
            {
                _outputDevice.Play();
                return true;
            }

            return false;
        }

        public void Stop()
        {
            if (_outputDevice.PlaybackState == PlaybackState.Playing)
            {
                _outputDevice.Stop();
            }
        }
    }
}