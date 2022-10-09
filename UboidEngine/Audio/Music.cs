using SDL2;
using System;
using System.Collections.Generic;
using System.Text;

namespace UboidEngine.Audio
{
    public static class Music
    {
        public static IntPtr m_MusicPtr { get; private set; }

        public static bool IsPlaying
        {
            get
            {
                if (m_MusicPtr == IntPtr.Zero)
                {
                    return false;
                }

                return SDL_mixer.Mix_PlayingMusic() == 1;
            }
        }

        public static double Position
        {
            get
            {
                if (m_MusicPtr == IntPtr.Zero)
                {
                    return 0;
                }

                return SDL_mixer.Mix_GetMusicPosition(m_MusicPtr);
            }
            set
            {
                if (m_MusicPtr == IntPtr.Zero)
                {
                    return;
                }

                SDL_mixer.Mix_SetMusicPosition(value);
            }
        }

        public static void Load(string path)
        {
            if(m_MusicPtr != null)
            {
                SDL_mixer.Mix_HaltMusic();
                SDL_mixer.Mix_FreeMusic(m_MusicPtr);
            }
            m_MusicPtr = SDL_mixer.Mix_LoadMUS(path);
        }

        public static void Play(int loops = 0)
        {
            if(m_MusicPtr == IntPtr.Zero)
            {
                return;
            }

            SDL_mixer.Mix_PlayMusic(m_MusicPtr, loops);
        }

        public static void Pause()
        {
            if (m_MusicPtr == IntPtr.Zero)
            {
                return;
            }

            SDL_mixer.Mix_PauseMusic();
        }

        public static void Resume()
        {
            if (m_MusicPtr == IntPtr.Zero)
            {
                return;
            }

            SDL_mixer.Mix_ResumeMusic();
        }

        public static void Stop()
        {
            if (m_MusicPtr == IntPtr.Zero)
            {
                return;
            }

            SDL_mixer.Mix_HaltMusic();
        }
    }
}
