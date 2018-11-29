using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpSlugsEngine;
using SharpSlugsEngine.Input;
using SharpSlugsEngine.Sound;
using System.Windows.Forms;
namespace AcceptanceTest {
    class Program {
        static void Main() {

        }
    }
    class AceptanceTestGame : Game {
        protected override void Initialize() {
            Graphics.BackColor = Color.Gray;
            Graphics.SetWorldScale(new Vector2(1280, 720));
        }

        protected override void LoadContent() {
            Content.AddImage("putFilePathHere", "tank");
            Sprites.Add("Player1", new PlayerTank("tank", 100, 700, true));
            Sprites.Add("Player2", new PlayerTank("tank", 1100, 700, false));
            //Tim add rectangle(s) for map
        }

        protected override void Update(GameTime gameTime) {
            //Tim add check for health <= 0;
        }

        protected override void Draw(GameTime gameTime) {
            throw new NotImplementedException();
        }

    }

    class PlayerTank : SImage {
        double fireAngle;
        int health;
        ActionSet action;
        public PlayerTank(string image, int x, int y, bool playerOne) : base(x, y, 40, 20, image) {
            fireAngle = 0;
            health = 100;
            action = new ActionSet(game);
            action.setPlayerControls(playerOne);
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            if (action.right.IsPressed) this.SetVelocityX(2);
            if(action.left.IsPressed) this.SetVelocityX(-2);

            if(fireAngle < 120 && action.up.IsPressed) this.fireAngle += 1;
            if(fireAngle > 45 && action.down.IsPressed) this.fireAngle -= 1;

            if (action.shoot.IsPressed) fireBullet();
        }

        public void fireBullet() {
            this.game.Sprites.Add(GetBulletName(), new Bullet(x, y, fireAngle));
        }

        private static string bullet = "bullet";
        private static int bulletCount = 0;

        public static string GetBulletName() => bullet + bulletCount++;

    }

    class Bullet : Ellipse {
        public Bullet(int x, int y, double fireAngle) : base(x, y, 5, 5, Color.Black) {
            this.SetGravityY(100);
            Vector2 velocity = Vector2.Down.Rotate(Vector2.Zero, (float)fireAngle) * 50;
            this.SetVelocityX((int)velocity.X);
            this.SetVelocityY((int)velocity.Y);
        }
        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            if (this.y > 720) {
                this.Kill();
            }
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
