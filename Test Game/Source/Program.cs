using System;
using System.Collections.Generic;
using SharpSlugsEngine;
using SharpSlugsEngine.Input;
using System.Drawing;
using System.Windows.Forms;

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
        private readonly Dictionary<string, InputAction> inputActions = new Dictionary<string, InputAction>();

        private Vector2 playerPos = new Vector2(640, 360);

        protected override void Initialize()
        {
            TargetFramerate = 1;

            inputActions.Add("Left", new InputAction(this));
            inputActions.Add("Right", new InputAction(this));
            inputActions.Add("Up", new InputAction(this));
            inputActions.Add("Down", new InputAction(this));

            inputActions["Left"].AddKey(Keys.Left);
            inputActions["Right"].AddKey(Keys.Right);
            inputActions["Up"].AddKey(Keys.Up);
            inputActions["Down"].AddKey(Keys.Down);

            inputActions["Left"].Add360Buttons(Xbox360Controller.ButtonType.DPadLeft);
            inputActions["Right"].Add360Buttons(Xbox360Controller.ButtonType.DPadRight);
            inputActions["Up"].Add360Buttons(Xbox360Controller.ButtonType.DPadUp);
            inputActions["Down"].Add360Buttons(Xbox360Controller.ButtonType.DPadDown);

            Controllers.ControllerAdded += (newController) =>
            {
                foreach (InputAction action in inputActions.Values)
                {
                    action.AddDevice(newController);
                }
            };

            Event EventA = new Event();
            EventA.Test += (key, location) => Console.WriteLine("Mouse at {0}", location);
            Mouse.AddLocationBind(EventA);

            ShowCursor = false;
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
            sprites.add("test", new TestSprite(100, 100, 50, 75, Color.AliceBlue, true));
            sprites.display("test", true);

            sprites.add("newrect", new Rect(500, 500, 10, 10, Color.Red));
            sprites.display("newrect", true);
            Event MoveRect = new Event();
            MoveRect.Test += (key, location) => sprites.moveto("newrect", location.X, location.Y);
            Mouse.AddLocationBind(MoveRect);
        }
        protected override void Update(GameTime gameTime)
        {
            Resolution = new Vector2(1280, 720);
            //Console.WriteLine("Update");

            //Search for a left stick outside of a modest deadzone
            Vector2 moveVec = new Vector2(0, 0);
            foreach (Xbox360Controller controller in Controllers.Xbox360Controllers)
            {
                if (controller.LeftStick.State.Length >= 0.25)
                {
                    moveVec = controller.LeftStick.State;
                    break;
                }
            }

            //Apply InputAction bindings to this vector
            if (inputActions["Left"].IsPressed) moveVec = new Vector2(-1, moveVec.Y);
            if (inputActions["Right"].IsPressed) moveVec = new Vector2(1, moveVec.Y);
            if (inputActions["Up"].IsPressed) moveVec = new Vector2(moveVec.X, -1);
            if (inputActions["Down"].IsPressed) moveVec = new Vector2(moveVec.X, 1);
            
            //Make sure the vector isn't too long
            if (moveVec.Length > 1f)
            {
                moveVec = moveVec.Normalize();
            }

            //Move the player
            playerPos += moveVec * 250 * (float)gameTime.deltaTime.TotalSeconds;

            //Keep the player on the screen
            if (playerPos.X < 10) playerPos = new Vector2(10, playerPos.Y);
            if (playerPos.X > Resolution.X - 10) playerPos = new Vector2(Resolution.X - 10, playerPos.Y);
            if (playerPos.Y < 10) playerPos = new Vector2(playerPos.X, 10);
            if (playerPos.Y > Resolution.Y - 10) playerPos = new Vector2(playerPos.X, Resolution.Y - 10);
        }

        protected override void Draw(GameTime gameTime)
        {
            //Console.WriteLine("Draw");
            Graphics.DrawRectangle(50, 50, 100, 100, Color.Blue);
            Graphics.DrawLine(100, 100, 400, 400, Color.BlanchedAlmond);
            Graphics.DrawCircle(250, 250, 50, Color.FromArgb(69, 69, 69));

            Graphics.DrawCircle((Point)playerPos, 10, Color.Black);
        }
    }

    //Test override of Sprite class based on Rectangle class
    public class TestSprite : Sprite {
        Color color;
        bool fill;
        public TestSprite(int x, int y, int w, int h, Color color, bool fill = true) {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            this.color = color;
            this.fill = fill;
            disp = false;
            alive = true;
            angle = 0;
            xAnchor = yAnchor = 0;
        }

        public override void Draw(GraphicsManager graphics) {
            graphics.DrawRectangle(x, y, w, h, color, fill);
        }

        public override void Update() {
            move(5, 10);
            if (x > 720 || y > 1280) {
                kill();
            }
        }
    }
}
