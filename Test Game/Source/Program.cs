using System;
using SharpSlugsEngine;

namespace Test_Game
{
    static class Program
    {
        static void Main()
        {
            TestGame test =  new TestGame();
            test.Resolution = new Vector2(1920, 1080);
            test.Run();
        }
    }

    class TestGame : Game
    {
        protected override void Update(GameTime gameTime)
        {
            Resolution = new Vector2(1280, 720);
            Console.WriteLine("Update");
        }

        protected override void Draw(GameTime gameTime)
        {
            Console.WriteLine("Draw");
        }
    }
}
