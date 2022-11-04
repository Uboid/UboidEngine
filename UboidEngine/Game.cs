using System;
using System.IO;
using SDL2;
using UboidEngine.Core.Scenes;
using UboidEngine.Core.Logging;

namespace UboidEngine
{
    /// <summary>
    /// Main class for all games.
    /// </summary>
    public class Game
    {
        private int screenWidth, screenHeight;

        public static StreamWriter PlayerLog;

        private bool _halt = false;

        public static Action<SDL.SDL_EventType, SDL.SDL_Event> OnEvent;

        public static Game Instance { get; private set; }

        public bool Running { get; private set; } = false;

        public static float DeltaTime { get; private set; }
        private uint frameStart = 0;

        public float FPS_LIMIT = 60f;

        private string title;

        private IntPtr renderer;
        private IntPtr window;

        public Game(string title = "UboidEngine Window", int w = 640, int h = 480)
        {
            this.screenWidth = w;
            this.screenHeight = h;
            Instance = this;
            this.title = title;
        }

        void CalculateDeltaTime()
        {
            DeltaTime = (SDL.SDL_GetTicks() - frameStart) / 1000.0f;
            if (DeltaTime < (float)(1f / FPS_LIMIT))
            {
                float deltaFps = (1 / FPS_LIMIT);
                float time = (deltaFps - DeltaTime) * 1000;
                DeltaTime = deltaFps;
                SDL.SDL_Delay((uint)time);
            }
            frameStart = SDL.SDL_GetTicks();
        }

        public void HaltExecution()
        {
            SceneManager.Active?.Destroy();
            SDL.SDL_DestroyRenderer(renderer);
            SDL.SDL_DestroyWindow(window);
            SDL.SDL_Quit();

            Environment.Exit(0);
        }

        public void Run()
        {
            var now = DateTime.Now;
            if(!Directory.Exists($"{Directory.GetCurrentDirectory()}/logs"))
            {
                Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}/logs");
            }
            PlayerLog = new StreamWriter($"logs/player-{now.Day}-{now.Month}-{now.Year}_{now.Hour}-{now.Minute}-{now.Second}.log");
            try
            {
                if (SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING) < 0)
                {
                    Log.Info($"[Uboid] There was an issue initilizing SDL. {SDL.SDL_GetError()}", ConsoleColor.Red);
                }
                else
                {
                    SDL.SDL_GetVersion(out var ver);
                    Log.Info($"[Uboid] Initialized SDL v{ver.major}.{ver.minor}.{ver.patch}");
                }

                SDL_ttf.TTF_Init();

                window = SDL.SDL_CreateWindow(title, SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, screenWidth, screenHeight, SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL | SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);

                if (window == IntPtr.Zero)
                {
                    Log.Info($"[Uboid] There was an issue creating the window. {SDL.SDL_GetError()}", ConsoleColor.Red);
                }

                renderer = SDL.SDL_CreateRenderer(window, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);

                if (renderer == IntPtr.Zero)
                {
                    Log.Info($"[Uboid] There was an issue creating the renderer. {SDL.SDL_GetError()}", ConsoleColor.Red);
                }

                if (SDL_image.IMG_Init(SDL_image.IMG_InitFlags.IMG_INIT_PNG) == 0)
                {
                    Log.Info($"[Uboid] There was an issue initilizing SDL2_image {SDL_image.IMG_GetError()}", ConsoleColor.Red);
                }
                else
                {
                    SDL_image.SDL_IMAGE_VERSION(out var ver);
                    Log.Info($"[Uboid] Initialized SDL_image v{ver.major}.{ver.minor}.{ver.patch}");
                }

                if (SDL_mixer.Mix_OpenAudio(44100, SDL_mixer.MIX_DEFAULT_FORMAT, 2, 2048) < 0)
                {
                    Log.Info($"[Uboid] There was an issue initilizing SDL2_mixer {SDL_mixer.Mix_GetError()}", ConsoleColor.Red);
                }
                else
                {
                    SDL_mixer.SDL_MIXER_VERSION(out var ver);
                    Log.Info($"[Uboid] Initialized SDL_mixer v{ver.major}.{ver.minor}.{ver.patch}");
                }

                Log.Info($"[Uboid] Running UboidEngine v{EngineGlobal.GetVersion()}", ConsoleColor.Cyan);

                Running = true;

                Keyboard.Start();

                while (Running)
                {
                    while (SDL.SDL_PollEvent(out SDL.SDL_Event e) == 1)
                    {
                        OnEvent?.Invoke(e.type, e);
                        switch (e.type)
                        {
                            case SDL.SDL_EventType.SDL_QUIT:
                                Running = false;
                                PlayerLog.Flush();
                                HaltExecution();
                                return;
                        }
                    }

                    SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255);
                    SDL.SDL_RenderClear(renderer);

                    CalculateDeltaTime();

                    // Update Logic Here
                    SceneManager.UpdateScene();

                    SDL.SDL_RenderPresent(renderer);
                }
            }
            catch(Exception ex)
            {
                Log.Info($"An error has occured and the game has been halted.", ConsoleColor.DarkRed);
                Log.Info($"Error is listed in 'player.log'", ConsoleColor.DarkRed);

                if(PlayerLog != null && !_halt)
                {
                    PlayerLog.WriteLine($"-- ERROR ! --");
                    PlayerLog.WriteLine($"--");
                    PlayerLog.WriteLine($"--");
                    PlayerLog.WriteLine($"--");
                    PlayerLog.WriteLine($"{ex.Message}\nStackTrace: {ex.StackTrace}");
                }

                PlayerLog.Flush();
            }

            try
            {
                PlayerLog.Flush();
            }
            catch(Exception ex)
            {

            }
        }

        public static Game GetInstance() => Instance;
        public IntPtr GetRenderer() => renderer;
        public IntPtr GetWindow() => window;
        public int GetWidth() => screenWidth;
        public int GetHeight() => screenHeight;
    }
}
