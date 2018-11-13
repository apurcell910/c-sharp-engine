using System;
using System.Collections.Generic;
using SharpSlugsEngine;
using SharpSlugsEngine.Input;
using SharpSlugsEngine.Physics;
using System.Drawing;
using System.Windows.Forms;

namespace Test_Game
{
    static class Program
    {
        static void Main()
        {
            TestGame test =  new TestGame();
            test.Resolution = new Vector2(1280 * 0.75f, 720 * 0.75f);
            test.Run();
        }
    }

    class TestGame : Game
    {
        public readonly Random rnd = new Random();

        private readonly Dictionary<string, InputAction> inputActions = new Dictionary<string, InputAction>();

        private List<Bullet> bullets = new List<Bullet>();
        private float bulletCooldown;

        private List<Asteroid> asteroids = new List<Asteroid>();
        private float asteroidCooldown;

        private Vector2 playerPos = new Vector2(640, 360);
        private Vector2 mousePos;

        private bool usingMouse;

        private Vector2 cursorSize;
        private Vector2 shipSize;

        private Vector2 shootDir;
        private bool shooting;

        private bool gameOver;

        private PolygonCollider polygonTest;
        private PolygonCollider polygonTest2;

        private TriangleCollider triangleTest1;
        private TriangleCollider triangleTest2;

        private pEllipse ellipseTest;
        private pRectangle rectTest;

        protected override void Initialize()
        {
            Graphics.BackColor = Color.Black;
            Graphics.SetWorldScale(new Vector2(1280, 720));

            inputActions.Add("Left", new InputAction(this));
            inputActions.Add("Right", new InputAction(this));
            inputActions.Add("Up", new InputAction(this));
            inputActions.Add("Down", new InputAction(this));

            inputActions["Left"].AddKey(Keys.Left);
            inputActions["Right"].AddKey(Keys.Right);
            inputActions["Up"].AddKey(Keys.Up);
            inputActions["Down"].AddKey(Keys.Down);

            inputActions["Left"].AddKey(Keys.A);
            inputActions["Right"].AddKey(Keys.D);
            inputActions["Up"].AddKey(Keys.W);
            inputActions["Down"].AddKey(Keys.S);

            inputActions["Left"].AddXboxButtons(XboxController.ButtonType.DPadLeft);
            inputActions["Right"].AddXboxButtons(XboxController.ButtonType.DPadRight);
            inputActions["Up"].AddXboxButtons(XboxController.ButtonType.DPadUp);
            inputActions["Down"].AddXboxButtons(XboxController.ButtonType.DPadDown);

            Controllers.ControllerAdded += (newController) =>
            {
                foreach (InputAction action in inputActions.Values)
                {
                    action.AddDevice(newController);
                }
            };
            
            ShowCursor = false;
        }
        
        protected override void LoadContent()
        {
            Content.AddImage("../../Content/GameOver.png", "GameOver");
            Content.AddImage("../../Content/Asteroid.png", "Asteroid");

            sprites.add("cursor", new SImage(0, 0, "../../Content/Cursor.png"));
            sprites.scale("cursor", 0.5);
            sprites.display("cursor", true);

            sprites.add("ship", new SImage(640, 360, "../../Content/Ship.png"));
            sprites.scale("ship", 0.5);
            sprites.display("ship", true);
            
            cursorSize = sprites.getSize("cursor");
            shipSize = sprites.getSize("ship");
            
            Vector2[] ccw = new Vector2[]
            {
                new Vector2(200, 0),
                new Vector2(100, 70),
                new Vector2(150, 60),
                new Vector2(100, 110),
                new Vector2(120, 150),
                new Vector2(160, 70),
                new Vector2(200, 170),
                new Vector2(120, 180),
                new Vector2(170, 230),
                new Vector2(240, 150),
                new Vector2(180, 70),
                new Vector2(260, 90),
                new Vector2(250, 110),
                new Vector2(230, 90),
                new Vector2(230, 110),
                new Vector2(270, 130),
                new Vector2(270, 50),
                new Vector2(200, 50)
            };

            Vector2[] cw = new Vector2[ccw.Length];
            ccw.CopyTo(cw, 0);
            Array.Reverse(cw);

            for (int i = 0; i < cw.Length; i++)
            {
                cw[i] += new Vector2(300, 0);
            }

            polygonTest = new PolygonCollider(ccw);
            polygonTest2 = new PolygonCollider(cw);

            triangleTest2 = new TriangleCollider(new Vector2(100.0f, 5.0f), new Vector2(50.0f, 50.0f), new Vector2(800.0f, 400.0f));
            triangleTest1 = new TriangleCollider(new Vector2(110.0f, 10.0f), new Vector2(55.0f, 45.0f), new Vector2(100.0f, 20.0f));

            rectTest = new pRectangle(new Vector2(400.0f, 400.0f), new Vector2(600.0f, 600.0f));

            ellipseTest = new pEllipse(new Vector2(400.0f, 400.0f), 300f, 350f);
            if(triangleTest1.IsTouching(triangleTest2))
            {
                Console.WriteLine("Triangles are touching!");
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (gameOver) return;

            if (mousePos != Graphics.ToWorldScale(Mouse.State.Location))
            {
                usingMouse = true;
                mousePos = Graphics.ToWorldScale(Mouse.State.Location);
            }

            shooting = usingMouse && Mouse.State.Left.IsClicked;
            
            //Search for sticks outside of a modest deadzone
            //Left stick for movement and right stick for shooting
            Vector2 moveVec = new Vector2(0, 0);
            Vector2 shootVec = new Vector2(0, 0);
            foreach (XboxController controller in Controllers.XboxControllers)
            {
                if (controller.LeftStick.State.Length >= 0.25)
                {
                    moveVec = controller.LeftStick.State;
                }

                if (controller.RightStick.State.Length >= 0.25)
                {
                    shootVec = controller.RightStick.State;
                    shooting = true;
                    usingMouse = false;
                }
            }
            
            sprites.display("cursor", usingMouse);

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
            if (playerPos.X < shipSize.X / 2f) playerPos = new Vector2(shipSize.X / 2f, playerPos.Y);
            if (playerPos.X > Graphics.WorldScale.X - shipSize.X / 2f) playerPos = new Vector2(Graphics.WorldScale.X - shipSize.X / 2f, playerPos.Y);
            if (playerPos.Y < shipSize.Y / 2f) playerPos = new Vector2(playerPos.X, shipSize.Y / 2f);
            if (playerPos.Y > Graphics.WorldScale.Y - shipSize.Y / 2f) playerPos = new Vector2(playerPos.X, Graphics.WorldScale.Y - shipSize.Y / 2f);
            
            sprites.moveto("cursor", (int)(mousePos.X - cursorSize.X / 2f), (int)(mousePos.Y - cursorSize.Y / 2f));
            sprites.moveto("ship", (int)(playerPos.X - shipSize.X / 2f), (int)(playerPos.Y - shipSize.Y / 2f));
            
            if (usingMouse)
            {
                shootDir = (playerPos - mousePos).Normalize();
            }
            else if (shootVec != Vector2.Zero)
            {
                shootDir = -shootVec.Normalize();
            }

            float angleRadians = (float)Math.Atan2(shootDir.Y, shootDir.X);
            float angleDegrees = angleRadians * (float)(180f / Math.PI) - 90;
            
            sprites.setRotation("ship", angleDegrees);

            bullets.ForEach(bullet => bullet.Update(gameTime));
            bullets.RemoveAll(bullet => bullet.Dead);

            asteroids.ForEach(asteroid => asteroid.Update(gameTime));
            asteroids.RemoveAll(asteroid => asteroid.Dead);

            bulletCooldown -= (float)gameTime.deltaTime.TotalSeconds;
            asteroidCooldown -= (float)gameTime.deltaTime.TotalSeconds;

            if (shooting && bulletCooldown <= 0f)
            {
                bulletCooldown = 0.25f;
                bullets.Add(new Bullet(this, playerPos - shootDir * shipSize.Y / 2f, shootDir * -500));
            }

            if (asteroidCooldown <= 0f)
            {
                asteroidCooldown = (float)(rnd.NextDouble() + 0.5f);

                //0 - top
                //1 - left
                //2 - bottom
                //3 - right
                Vector2 pos = new Vector2(0, 0);
                Vector2 vel = new Vector2(0, 0);
                switch (rnd.Next(4))
                {
                    case 0:
                        pos = new Vector2(rnd.Next(0, (int)Graphics.WorldScale.X), 0);
                        vel = new Vector2(rnd.Next(100) - 50, rnd.Next(50));
                        break;
                    case 1:
                        pos = new Vector2(0, rnd.Next(0, (int)Graphics.WorldScale.Y));
                        vel = new Vector2(rnd.Next(50), rnd.Next(100) - 50);
                        break;
                    case 2:
                        pos = new Vector2(rnd.Next(0, (int)Graphics.WorldScale.X), Graphics.WorldScale.Y);
                        vel = new Vector2(rnd.Next(100) - 50, -rnd.Next(50));
                        break;
                    case 3:
                        pos = new Vector2(Graphics.WorldScale.X, rnd.Next(0, (int)Graphics.WorldScale.Y));
                        vel = new Vector2(-rnd.Next(50), rnd.Next(100) - 50);
                        break;
                }

                asteroids.Add(new Asteroid(this, pos, vel.Normalize() * rnd.Next(100, 200)));
            }

            foreach (Asteroid asteroid in asteroids)
            {
                foreach (Bullet bullet in bullets)
                {
                    if (asteroid.CheckCollision(bullet.Position, 5))
                    {
                        bullet.Dead = true;
                        asteroid.Damage();
                    }
                }
            }

            bullets.RemoveAll(bullet => bullet.Dead);
            asteroids.RemoveAll(asteroid => asteroid.Dead);

            foreach (Asteroid asteroid in asteroids)
            {
                if (asteroid.CheckCollision(playerPos, (int)(shipSize.X / 3f)))
                {
                    sprites.display("ship", false);
                    sprites.display("cursor", false);
                    ShowCursor = true;
                    gameOver = true;
                    break;
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            bullets.ForEach(bullet => bullet.Draw());
            asteroids.ForEach(asteroid => asteroid.Draw());
            
            /*foreach (Triangle tri in polygonTest.Triangles)
            {
                Graphics.DrawTri(tri, Color.NavajoWhite, false);
            }

            foreach (Triangle tri in polygonTest2.Triangles)
            {
                Graphics.DrawTri(tri, Color.PaleVioletRed, false);
            }

            foreach(Triangle tri in ellipseTest.Triangles)
            {
                Graphics.DrawTri(tri, Color.Azure);
            }
            
            Graphics.DrawTri(triangleTest1.triangle, Color.MediumAquamarine);
            Graphics.DrawTri(triangleTest2.triangle, Color.MediumAquamarine);

            Graphics.DrawTri(rectTest.TriOne, Color.Maroon);
            Graphics.DrawTri(rectTest.TriTwo, Color.Orange);*/
            
            if (gameOver)
            {
                Graphics.DrawBMP(Content.GetImage("GameOver"), 0, 0, (int)Resolution.X, (int)Resolution.Y, 0, DrawType.Screen);
            }
        }

        public class Bullet
        {
            private Game _game;

            public Vector2 Position { get; private set; }
            public Vector2 Velocity { get; private set; }
            public bool Dead;

            public Bullet(Game game, Vector2 pos, Vector2 velocity)
            {
                _game = game;

                Position = pos;
                Velocity = velocity;
            }

            public void Update(GameTime gameTime)
            {
                if (Dead) return;

                Position = Position + Velocity * (float)gameTime.deltaTime.TotalSeconds;

                if (Position.X < 0 || Position.X > _game.Graphics.WorldScale.X || Position.Y < 0 || Position.Y > _game.Graphics.WorldScale.Y)
                {
                    Dead = true;
                }
            }

            public void Draw()
            {
                if (Dead) return;

                _game.Graphics.DrawCircle((Point)Position, 5, Color.White);
            }
        }

        public class Asteroid
        {
            private Bitmap image;

            private Game _game;
            private float rotSpeed;
            private float rotation;
            private float size;

            private int hp;
            private int maxHp;

            public Vector2 Position { get; private set; }
            public Vector2 Velocity { get; private set; }
            public bool Dead;

            public Asteroid(Game game, Vector2 pos, Vector2 velocity)
            {
                _game = game;

                image = _game.Content.GetImage("Asteroid");

                Random rnd = new Random();
                rotSpeed = (float)rnd.NextDouble() * 50 - 25;
                size = (float)rnd.NextDouble() + 0.5f;

                hp = (int)(size * 5);
                maxHp = hp;

                Position = pos;
                Velocity = velocity;
            }

            public void Update(GameTime gameTime)
            {
                if (Dead) return;

                Position = Position + Velocity * (float)gameTime.deltaTime.TotalSeconds;

                if (Position.X < -(image.Width * size) / 2f || Position.X > _game.Graphics.WorldScale.X + (image.Width * size) / 2f
                    || Position.Y < -(image.Height * size) / 2f || Position.Y > _game.Graphics.WorldScale.Y + (image.Height * size) / 2f)
                {
                    Dead = true;
                }

                rotation += rotSpeed * (float)gameTime.deltaTime.TotalSeconds * 5;
                if (rotation > 360) rotation -= 360;
            }

            public bool CheckCollision(Vector2 loc, int radius)
            {
                Vector2 dist = Position - loc;
                dist = new Vector2(Math.Abs(dist.X), Math.Abs(dist.Y));

                float selfRadius = image.Width / 2f;

                return dist.Length <= radius + selfRadius * size;
            }

            public void Damage()
            {
                if (--hp <= 0)
                {
                    Dead = true;
                }
            }

            public void Draw()
            {
                Vector2 topLeft = Position - ((Vector2)image.Size * size) / 2f;
                _game.Graphics.DrawBMP(image, topLeft.X, topLeft.Y, image.Width * size, image.Height * size, rotation);

                _game.Graphics.DrawRectangle(topLeft + Vector2.Up * 10, new Vector2(image.Size.Width * size, 10), Color.Red);
                _game.Graphics.DrawRectangle(topLeft + Vector2.Up * 10, new Vector2(image.Size.Width * size * (hp / (float)maxHp), 10), Color.Green);
            }
        }
    }
}
