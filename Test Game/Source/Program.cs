using System;
using System.Collections.Generic;
using SharpSlugsEngine;
using System.Drawing;

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
        int i;
        
        protected override void Initialize()
        {
            Controllers.ControllerAdded += (newController) =>
            {
                Console.WriteLine("New 360 controller added");

                newController.Disconnected += () => Console.WriteLine("Controller Disconnected");
                newController.Connected += () => Console.WriteLine("Controller Connected");

                newController.APressed += () => Console.WriteLine("A Pressed");
                newController.BPressed += () => Console.WriteLine("B Pressed");
                newController.XPressed += () => Console.WriteLine("X Pressed");
                newController.YPressed += () => Console.WriteLine("Y Pressed");
                newController.LBPressed += () => Console.WriteLine("LB Pressed");
                newController.RBPressed += () => Console.WriteLine("RB Pressed");
                newController.BackPressed += () => Console.WriteLine("Back Pressed");
                newController.StartPressed += () => Console.WriteLine("Start Pressed");
                newController.DPadUpPressed += () => Console.WriteLine("DPadUp Pressed");
                newController.DPadDownPressed += () => Console.WriteLine("DPadDown Pressed");
                newController.DPadLeftPressed += () => Console.WriteLine("DPadLeft Pressed");
                newController.DPadRightPressed += () => Console.WriteLine("DPadRight Pressed");
            };
        }

        protected override void LoadContent() {
            sprites.add("rect1", new Rectangle(400, 400, 50, 50), Color.Red);
            sprites.display("rect1", true);
            i = 0;
        }
        protected override void Update(GameTime gameTime)
        {
            Resolution = new Vector2(1280, 720);
            //Console.WriteLine("Update");
            if (i == 50) {
                sprites.scaleX("rect1", 2);
            }
            if (i == 100) {
                sprites.scaleY("rect1", 0.5);
            }
            if (i == 150) {
                sprites.move("rect1", 100, -200);
            }
            if (i == 200) {
                sprites.display("rect1", false);
                sprites.scale("rect1", 2);
            }
            if (i == 300) {
                sprites.display("rect1", true);
            }
            i++;
        }

        protected override void Draw(GameTime gameTime)
        {
            //Console.WriteLine("Draw");
            Graphics.DrawRectangle(50, 50, 100, 100, Color.Blue);
            Graphics.DrawLine(100, 100, 400, 400, Color.BlanchedAlmond);
            Graphics.DrawCircle(250, 250, 50, Color.FromArgb(69, 69, 69));
        }
    }
}
