using System;
using SharpSlugsEngine;

namespace Test_Game
{
    static class Program
    {
        static void Main()
        {
            new TestGame().Run();
        }
    }

    class TestGame : Game
    {
        protected override void Update(GameTime gameTime)
        {
            Console.WriteLine("Update");
        }

        protected override void Draw(GameTime gameTime)
        {
            Console.WriteLine("Draw");
        }
    }
}
