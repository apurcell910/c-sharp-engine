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
        int i;
        
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
            sprites.add("rect1", new Rectangle(400, 400, 50, 50), Color.Red, Shape.RECTANGLE);
            sprites.add("ellipse", 800, 300, 40, 80, Color.White, Shape.ELLIPSE);
            sprites.add("line", new Point(30, 20), new Point(800, 300), Color.Violet, Shape.LINE);
            sprites.add("file", 500, 500, 1,  @"..\..\Content\test.bmp", Shape.FILE);
            sprites.display("rect1", true);
            sprites.setAnchor("rect1", 0.5);
            sprites.display("ellipse", true);
            sprites.display("line", true);
            sprites.display("file", true);

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
            sprites.moveX("ellipse", -5);
            sprites.scaleY("ellipse", 1.01);
            sprites.rotate("rect1", 1);
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
