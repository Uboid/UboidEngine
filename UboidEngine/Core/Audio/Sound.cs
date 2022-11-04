using SDL2;
using System;
using UboidEngine.Core.Logging;

namespace UboidEngine.Core.Audio
{
    public class Sound : IDisposable
    {
        private IntPtr soundPointer;

        public bool Disposed { get; private set; }

        public const string DISPOSE_DISPOSED = "Tried to dispose an already disposed audio.";
        public const string DISPOSE_NULL = "Tried to dispose an audio which contents are null.";
        public const string ACTION_NULL = "Tried to call Sound.{0}() but the audio has already been disposed or the contents are null.";
        
        public static Sound LoadFrom(string file)
        {
            return new Sound() { soundPointer = SDL_mixer.Mix_LoadWAV(file) };
        }

        static bool Assert(Sound snd, string fn)
        {
            return Log.Assert(snd.Disposed || snd.soundPointer == IntPtr.Zero, string.Format(ACTION_NULL, fn), false);
        }

        public void Dispose()
        {
            Log.Assert(Disposed || soundPointer == IntPtr.Zero, Disposed ? DISPOSE_DISPOSED : DISPOSE_NULL);
            Disposed = true;
            SDL_mixer.Mix_FreeChunk(soundPointer);
            soundPointer = IntPtr.Zero;
        }

        public void Play(int channel = 0, int loops = 0)
        {
            if(Assert(this, "Play"))
            {
                return;
            }

            SDL_mixer.Mix_PlayChannel(channel, soundPointer, loops);
        }
    }
}
