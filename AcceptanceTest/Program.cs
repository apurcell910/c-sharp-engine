using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpSlugsEngine;
using SharpSlugsEngine.Input;
using SharpSlugsEngine.Sound;
using SharpSlugsEngine.Physics;
using System.Windows.Forms;
namespace AcceptanceTest {
    class Program {
        static void Main() {
            AceptanceTestGame game = new AceptanceTestGame();
            game.Resolution = new Vector2(1280 * 0.75f, 1280 * 0.75f);
            game.Run();
        }
    }
    class AceptanceTestGame : Game {
        private bool gameOver;
        private int winner;
        protected override void Initialize() {
            Graphics.BackColor = Color.Gray;
            Graphics.SetWorldScale(new Vector2(1280, 720));
            gameOver = false;
        }

        protected override void LoadContent() {
            Content.AddImage("../../images/redTank.png", "tank1");
            Content.AddImage("../../images/blueTank.png", "tank2");
            Content.AddFont("../../images/heav.ttf", "Heavy Data");

            Sprites.Add("Player1", new PlayerTank(this, "tank1", 100, 695, true));
            Sprites.Add("Player2", new PlayerTank(this, "tank2", 1100, 695, false));
            Sprites.Add("Ground", new Rect(0, 715, 1280, 5, Color.Brown));
            Sprites.Add("Wall", new Rect(640, 500, 10, 220, Color.Brown));
            Sprites.Display("Wall", true);
            Sprites.Display("Ground", true);
        }

        protected override void Update(GameTime gameTime) {
            if (gameOver)
            {
                //Maybe start new game on some button press?
                return;
            }

            if (!Sprites.IsAlive("Player1"))
            {
                winner = 2;
                Sprites.Kill("Player2");
                gameOver = true;
                return;
            }

            if (!Sprites.IsAlive("Player2"))
            {
                winner = 1;
                Sprites.Kill("Player1");
                gameOver = true;
                return;
            }


        }

        protected override void Draw(GameTime gameTime) {
            if (gameOver)
            {
                Graphics.DrawText("Player " + winner + " Wins", Content.GetFont("Heavy Data", 128), Color.White, Resolution, Resolution, 0, TextAlign.BottomRight, DrawType.Screen);
            }
        }

    }

    class PlayerTank : SImage {
        double fireAngle;
        double fireCooldown;
        int health;
        bool playerOne;
        ActionSet action;

        public PlayerTank(Game game, string image, double x, double y, bool playerOne) : base(game, x, y, 40, 20, image) {
            if (playerOne) {
                fireAngle = -45;
            } else {
                fireAngle = -225;
            }
            health = 100;
            action = new ActionSet(game);
            action.setPlayerControls(playerOne);
            this.Display(true);
            this.playerOne = playerOne;
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            this.SetVelocityX(0);

            if (action.right.IsPressed) this.SetVelocityX((int)(75));
            if (action.left.IsPressed) this.SetVelocityX((int)(-75));

            if (this.collider.IsTouching(game.Sprites.GetSprite("Wall").collider)) {
                this.SetVelocityX(0);
                if (playerOne) {
                    MoveX(-1);
                } else {
                    MoveX(1);
                }
            }

            if (playerOne) {
                if (fireAngle > -120 && action.up.IsPressed) this.fireAngle -= 50 * gameTime.DeltaTime.TotalSeconds;
                if (fireAngle < 0 && action.down.IsPressed) this.fireAngle += 50 * gameTime.DeltaTime.TotalSeconds;
            } else {
                if (fireAngle > -300 && action.down.IsPressed) this.fireAngle -= 50 * gameTime.DeltaTime.TotalSeconds;
                if (fireAngle < -120 && action.up.IsPressed) this.fireAngle += 50 * gameTime.DeltaTime.TotalSeconds;
            }
            if (fireCooldown > 0) {
                fireCooldown -= gameTime.DeltaTime.TotalSeconds;
            }

            if (action.shoot.IsPressed && fireCooldown <= 0) {
                fireBullet(playerOne);
                fireCooldown = 1.5;
            }

            if (health <= 0) this.Kill();
        }

        public override void Draw(GraphicsManager graphics) {
            base.Draw(graphics);
            graphics.DrawPolygon(collider.Vertices, Color.Red, false);
        }

        public void fireBullet(bool playerOne) {
            string bullet = GetBulletName();
            this.game.Sprites.Add(bullet, new Bullet(this.game, x, y, fireAngle, playerOne));
        }

        private static string bullet = "bullet";
        private static int bulletCount = 0;

        public static string GetBulletName() => bullet + bulletCount++;

    }

    class Bullet : Ellipse {
        bool playerOne;
        public Bullet(Game game, double x, double y, double fireAngle, bool playerOne) : base(game, x, y, 5, 5, Color.Black) {
            this.SetGravityY(2);
            Vector2 velocity;
            float bulletSpeed = 225;
            if (playerOne) {
                velocity = Vector2.Right.Rotate(Vector2.Zero, (float)fireAngle) * bulletSpeed;
            } else {
                velocity = Vector2.Right.Rotate(Vector2.Zero, (float)fireAngle) * bulletSpeed;
            }
            
            this.SetVelocityX((int)velocity.X);
            this.SetVelocityY((int)velocity.Y);
            this.Display(true);
            this.playerOne = playerOne;
        }
        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            if (this.y > game.Graphics.WorldScale.Y) {
                this.Kill();
            }
            if (playerOne) {
                if (this.collider.IsTouching(game.Sprites.GetSprite("Player2").collider)) {
                    game.Sprites.sprites["Player2"].Kill();
                    this.Kill();
                }
            } else {
                if (this.collider.IsTouching(game.Sprites.GetSprite("Player1").collider)) {
                    game.Sprites.sprites["Player1"].Kill();
                    this.Kill();
                }
            }
            if (this.collider.IsTouching(game.Sprites.GetSprite("Wall").collider)) {
                this.SetVelocityX(0);
            }
        }

        public override void Draw(GraphicsManager graphics) {
            base.Draw(graphics);
            //game.Graphics.DrawPolygon(collider.Vertices, Color.Red, true);
        }
    }

    class ActionSet
    {
        public InputAction left { get; private set; }
        public InputAction right { get; private set; }
        public InputAction up { get; private set; }
        public InputAction down { get; private set; }
        public InputAction shoot { get; private set; }

        public ActionSet(Game game)
        {
            left = new InputAction(game);
            right = new InputAction(game);
            up = new InputAction(game);
            down = new InputAction(game);
            shoot = new InputAction(game);

            game.Controllers.ControllerAdded += (newController) =>
            {
                left.AddDevice(newController);
                right.AddDevice(newController);
                up.AddDevice(newController);
                down.AddDevice(newController);
                shoot.AddDevice(newController);
                Console.WriteLine("Controller added");
            };
        }

        public void setPlayerControls(bool playerOne)
        {
            if(playerOne)
            {
                left.AddKey(Keys.Left);
                right.AddKey(Keys.Right);
                up.AddKey(Keys.Up);
                down.AddKey(Keys.Down);

                left.AddKey(Keys.D);
                right.AddKey(Keys.A);
                up.AddKey(Keys.W);
                down.AddKey(Keys.S);

                shoot.AddKey(Keys.Space);
            } else
            {
                left.AddXboxButtons(XboxController.ButtonType.DPadLeft);
                right.AddXboxButtons(XboxController.ButtonType.DPadRight);
                up.AddXboxButtons(XboxController.ButtonType.DPadUp);
                down.AddXboxButtons(XboxController.ButtonType.DPadDown);

                shoot.AddXboxButtons(XboxController.ButtonType.Y);
            }
        }
    }
}
