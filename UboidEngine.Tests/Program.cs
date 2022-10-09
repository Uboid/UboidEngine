using System;
using UboidEngine.Scenes;
using UboidEngine.Tests.Shared.Scenes;

namespace UboidEngine.Tests
{
    /// <summary>
    /// CREATE A SIMPLE PONG GAME USING UBOID ENGINE
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game("Pong!");
            SceneManager.LoadScene(new PongScene());
            game.Run();
        }
    }
}
