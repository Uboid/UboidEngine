using SDL2;
using System;
using System.Collections.Generic;
using System.Text;

namespace UboidEngine.Audio
{
    public class Sound : IDisposable
    {
        public IntPtr m_SoundPtr;

        public Sound(string path)
        {
            m_SoundPtr = SDL_mixer.Mix_LoadWAV(path);
            
            if(m_SoundPtr == IntPtr.Zero)
            {
                Console.WriteLine(SDL_mixer.Mix_GetError());
            }
        }

        public void Play(int channel = -1, int loops = 0)
        {
            SDL_mixer.Mix_PlayChannel(channel, m_SoundPtr, loops);
        }

        public void Dispose()
        {
            if (m_SoundPtr == IntPtr.Zero)
                return;

            SDL_mixer.Mix_FreeChunk(m_SoundPtr);
        }
    }
}
