using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SDL2;
using UboidEngine.Entities;
using UboidEngine.Scenes;

namespace UboidEngine
{
    /// <summary>
    /// Main class for all games.
    /// </summary>
    public class Game
    {
        public int m_iScreenW, m_iScreenH;

        public static StreamWriter PlayerLog;

        private bool _halt = false;

        public static Game Instance { get; private set; }

        public bool Running { get; private set; } = false;

        public static float DeltaTime { get; private set; }
        private uint frameStart = 0;

        public int FPS_LIMIT = 60;

        private string TITLE;

        public IntPtr m_pRenderer;
        public IntPtr m_pWindow;

        public byte[] ClearRGBA = new byte[4]
        {
            0,
            0,
            0,
            255
        };

        public Game(string title = "UboidEngine Window", int w = 640, int h = 480)
        {
            this.m_iScreenW = w;
            this.m_iScreenH = h;
            Instance = this;
            this.TITLE = title;
            new Camera();
        }

        public static IntPtr LoadTexture(string path)
        {
            IntPtr surface = SDL_image.IMG_Load(path);
            var tex = SDL.SDL_CreateTextureFromSurface(Instance.m_pRenderer, surface);
            SDL.SDL_FreeSurface(surface);

            return tex;
        }

        void CalculateDeltaTime()
        {
            uint ticks = SDL.SDL_GetTicks();
            DeltaTime = (ticks - frameStart) / 1000.0f;
            frameStart = ticks;
        }

        public virtual void Start() { }

        public virtual void Update()
        {
            SceneManager.Update();
        }

        public virtual void Draw()
        {
            SceneManager.Draw();
        }

        public void HaltExecution()
        {
            if (_halt)
                return;

            _halt = true;

            SceneManager.CurrentScene?.OnDestroy();
            SDL.SDL_DestroyRenderer(m_pRenderer);
            SDL.SDL_DestroyWindow(m_pWindow);
            SDL.SDL_Quit();

            Environment.Exit(0);
        }

        public void Run()
        {
            PlayerLog = new StreamWriter("player.log");
            PlayerLog.AutoFlush = true;

            AppDomain.CurrentDomain.ProcessExit += (o, e) =>
            {
                HaltExecution();

                if (PlayerLog != null)
                {
                    PlayerLog.Close();
                    PlayerLog.Dispose();
                    PlayerLog = null;
                }
            };

            try
            {

                if (SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING) < 0)
                {
                    EngineGlobal.Log($"[Uboid] There was an issue initilizing SDL. {SDL.SDL_GetError()}", ConsoleColor.Red);
                }
                else
                {
                    SDL.SDL_GetVersion(out var ver);
                    EngineGlobal.Log($"[Uboid] Initialized SDL v{ver.major}.{ver.minor}.{ver.patch}");
                }

                SDL_ttf.TTF_Init();

                m_pWindow = SDL.SDL_CreateWindow(TITLE, SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, m_iScreenW, m_iScreenH, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);

                if (m_pWindow == IntPtr.Zero)
                {
                    EngineGlobal.Log($"[Uboid] There was an issue creating the window. {SDL.SDL_GetError()}", ConsoleColor.Red);
                }

                m_pRenderer = SDL.SDL_CreateRenderer(m_pWindow,
                                                        -1,
                                                        SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED |
                                                        SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);

                if (m_pRenderer == IntPtr.Zero)
                {
                    EngineGlobal.Log($"[Uboid] There was an issue creating the renderer. {SDL.SDL_GetError()}", ConsoleColor.Red);
                }

                if (SDL_image.IMG_Init(SDL_image.IMG_InitFlags.IMG_INIT_PNG) == 0)
                {
                    EngineGlobal.Log($"[Uboid] There was an issue initilizing SDL2_Image {SDL_image.IMG_GetError()}", ConsoleColor.Red);
                }
                else
                {
                    SDL_image.SDL_IMAGE_VERSION(out var ver);
                    EngineGlobal.Log($"[Uboid] Initialized SDL_image v{ver.major}.{ver.minor}.{ver.patch}");
                }

                if (SDL_mixer.Mix_OpenAudio(44100, SDL_mixer.MIX_DEFAULT_FORMAT, 2, 2048) < 0)
                {
                    EngineGlobal.Log($"[Uboid] There was an issue initilizing SDL2_Mixer {SDL_mixer.Mix_GetError()}", ConsoleColor.Red);
                }
                else
                {
                    SDL_mixer.SDL_MIXER_VERSION(out var ver);
                    EngineGlobal.Log($"[Uboid] Initialized SDL_mixer v{ver.major}.{ver.minor}.{ver.patch}");
                }

                EngineGlobal.Log($"[Uboid] Running UboidEngine v{EngineGlobal.GetVersion()}", ConsoleColor.Cyan);

                Running = true;

                while (Running)
                {
                    while (SDL.SDL_PollEvent(out SDL.SDL_Event e) == 1)
                    {
                        switch (e.type)
                        {
                            case SDL.SDL_EventType.SDL_QUIT:
                                Running = false;
                                HaltExecution();
                                break;
                        }
                    }

                    if (!Running)
                        break;

                    SDL.SDL_SetRenderDrawColor(m_pRenderer, ClearRGBA[0], ClearRGBA[1], ClearRGBA[2], ClearRGBA[3]);
                    SDL.SDL_RenderClear(m_pRenderer);

                    Keyboard.Update();

                    CalculateDeltaTime();
                    if (DeltaTime < (1 / FPS_LIMIT))
                    {
                        DeltaTime = 1 / FPS_LIMIT;
                        SDL.SDL_Delay((uint)((1 / FPS_LIMIT) - DeltaTime));
                    }
                    Update();
                    Draw();

                    SDL.SDL_RenderPresent(m_pRenderer);
                }
            }
            catch(Exception ex)
            {
                EngineGlobal.Log($"An error has occured and the game has been halted.", ConsoleColor.DarkRed);
                EngineGlobal.Log($"Error is listed in 'player.log'", ConsoleColor.DarkRed);

                if(PlayerLog != null && !_halt)
                {
                    PlayerLog.WriteLine($"-- ERROR ! --");
                    PlayerLog.WriteLine($"--");
                    PlayerLog.WriteLine($"--");
                    PlayerLog.WriteLine($"--");
                    PlayerLog.WriteLine($"{ex.Message}\nStackTrace: {ex.StackTrace}");
                }
                
                HaltExecution();
            }
        }
    }
}
