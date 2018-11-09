﻿using System;
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
            test.Resolution = new Vector2(1280, 720);
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

        protected override void Initialize()
        {
            Graphics.BackColor = Color.Black;
            Graphics.SetWorldScale(200);

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
        }

        protected override void Update(GameTime gameTime)
        {
            if (gameOver) return;

            if (mousePos != Mouse.State.Location)
            {
                usingMouse = true;
                mousePos = Mouse.State.Location;
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
            if (playerPos.X > Resolution.X - shipSize.X / 2f) playerPos = new Vector2(Resolution.X - shipSize.X / 2f, playerPos.Y);
            if (playerPos.Y < shipSize.Y / 2f) playerPos = new Vector2(playerPos.X, shipSize.Y / 2f);
            if (playerPos.Y > Resolution.Y - shipSize.Y / 2f) playerPos = new Vector2(playerPos.X, Resolution.Y - shipSize.Y / 2f);

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
                        pos = new Vector2(rnd.Next(0, (int)Resolution.X), 0);
                        vel = new Vector2(rnd.Next(100) - 50, rnd.Next(50));
                        break;
                    case 1:
                        pos = new Vector2(0, rnd.Next(0, (int)Resolution.Y));
                        vel = new Vector2(rnd.Next(50), rnd.Next(100) - 50);
                        break;
                    case 2:
                        pos = new Vector2(rnd.Next(0, (int)Resolution.X), Resolution.Y);
                        vel = new Vector2(rnd.Next(100) - 50, -rnd.Next(50));
                        break;
                    case 3:
                        pos = new Vector2(Resolution.X, rnd.Next(0, (int)Resolution.Y));
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
            //Graphics.DrawRectangle(new Vector2(100, 100), 20, 20, Color.Red);
            //Graphics.DrawRectangle(500, 500, 500, 500, Color.Red);
            if (gameOver)
            {
                Graphics.DrawBMP(Content.GetImage("GameOver"), 0, 0, (int)Resolution.X, (int)Resolution.Y);
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

                if (Position.X < 0 || Position.X > _game.Resolution.X || Position.Y < 0 || Position.Y > _game.Resolution.Y)
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

                Position = pos;
                Velocity = velocity;
            }

            public void Update(GameTime gameTime)
            {
                if (Dead) return;

                Position = Position + Velocity * (float)gameTime.deltaTime.TotalSeconds;

                if (Position.X < -(image.Width * size) / 2f || Position.X > _game.Resolution.X + (image.Width * size) / 2f
                    || Position.Y < -(image.Height * size) / 2f || Position.Y > _game.Resolution.Y + (image.Height * size) / 2f)
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

                return dist.Length <= radius + (image.Width / 2f) * size;
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
                _game.Graphics.DrawBMP(image, (int)(Position.X - (image.Width * size) / 2f), (int)(Position.Y - (image.Height * size) / 2f), (int)(image.Width * size), (int)(image.Height * size), rotation);
            }
        }
    }
}
