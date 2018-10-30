using System;
using System.IO;
using SharpSlugsEngine;
using SharpSlugsEngine.Input;
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
        protected override void Initialize()
        {
            TargetFramerate = 1;
            
            Controllers.ControllerAdded += (newController) =>
            {
                Console.WriteLine("New controller added: " + newController.Type);

                if (newController.Type == ControllerType.Xbox360)
                {
                    Xbox360Controller controller = (Xbox360Controller)newController;

                    controller.Disconnected += () => Console.WriteLine("Controller Disconnected");
                    controller.Connected += () => Console.WriteLine("Controller Connected");

                    controller.APressed += () => Console.WriteLine("A Pressed");
                    controller.BPressed += () => Console.WriteLine("B Pressed");
                    controller.XPressed += () => Console.WriteLine("X Pressed");
                    controller.YPressed += () => Console.WriteLine("Y Pressed");
                    controller.LBPressed += () => Console.WriteLine("LB Pressed");
                    controller.RBPressed += () => Console.WriteLine("RB Pressed");
                    controller.BackPressed += () => Console.WriteLine("Back Pressed");
                    controller.StartPressed += () => Console.WriteLine("Start Pressed");
                    controller.DPadUpPressed += () => Console.WriteLine("DPadUp Pressed");
                    controller.DPadDownPressed += () => Console.WriteLine("DPadDown Pressed");
                    controller.DPadLeftPressed += () => Console.WriteLine("DPadLeft Pressed");
                    controller.DPadRightPressed += () => Console.WriteLine("DPadRight Pressed");
                }
            };
        }
        
        protected override void LoadContent() {
            ContentManager manager = new ContentManager();
            Bitmap [] bmp = manager.SplitImage(@"..\..\Content\test.bmp", 4, "test_bmp");
            Bitmap[] scaled = manager.ScaleImage(bmp, 4);
            manager.printNames();
            sprites.add("rect", new Rect(400, 400, 50, 50, Color.Aqua));
            sprites.add("ellipse", new Ellipse(800, 600, 120, 50, Color.Black));
            sprites.add("line", new Line(200, 200, 800, 500, Color.Green));
            sprites.display("rect", true);
            sprites.display("ellipse", true);
            sprites.display("line", true);
            sprites.add("img", new SImage(400, 500, scaled[1]));
            sprites.display("img", true);
            sprites.add("img2", new SImage(800, 100, "../../Content/test.bmp"));
            sprites.scale("img2", 0.3333333);
            sprites.display("img2", true);
        }
        protected override void Update(GameTime gameTime)
        {
            Resolution = new Vector2(1280, 720);
            //Console.WriteLine("Update");
            
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
