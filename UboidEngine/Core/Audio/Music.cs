using SDL2;
using System;
using UboidEngine.Core.Logging;

namespace UboidEngine.Core.Audio
{
    public static class Music
    {
        private static IntPtr musicPointer;

        public const string ACTION_NULL = "Tried to call Music.{0}() but the contents are null.";

        public static bool IsPlaying
        {
            get
            {
                if(Assert("get_IsPlaying()"))
                {
                    return false;
                }

                return SDL_mixer.Mix_PlayingMusic() > 0;
            }
        }

        public static double Position
        {
            get
            {
                if (Assert("get_Position()"))
                {
                    return 0d;
                }

                return SDL_mixer.Mix_GetMusicPosition(musicPointer);
            }
            set
            {
                if (Assert("set_Position(double)"))
                {
                    return;
                }

                SDL_mixer.Mix_SetMusicPosition(value);
            }
        }

        public static void LoadFrom(string file, bool autoPlay = false, int loops = 0)
        {
            Free();
            musicPointer = SDL_mixer.Mix_LoadMUS(file);

            if (autoPlay) Play(loops);
        }

        public static void Free()
        {
            if(musicPointer != IntPtr.Zero)
            {
                SDL_mixer.Mix_FreeMusic(musicPointer);
            }
            musicPointer = IntPtr.Zero;
        }

        static bool Assert(string fn)
        {
            return Log.Assert(musicPointer == IntPtr.Zero, string.Format(ACTION_NULL, fn), false);
        }

        public static void Play(int loops = 0)
        {
            if (Assert("Play"))
            {
                return;
            }

            SDL_mixer.Mix_PlayMusic(musicPointer, loops);
        }

        public static void Pause()
        {
            if (Assert("Pause"))
            {
                return;
            }

            SDL_mixer.Mix_PauseMusic();
        }

        public static void Resume()
        {
            if (Assert("Resume"))
            {
                return;
            }

            SDL_mixer.Mix_ResumeMusic();
        }

        public static void Stop()
        {
            if (Assert("Stop"))
            {
                return;
            }

            SDL_mixer.Mix_HaltMusic();
        }
    }
}
