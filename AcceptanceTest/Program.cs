using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpSlugsEngine;
using SharpSlugsEngine.Input;
using SharpSlugsEngine.Sound;

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
            Sprites.Add("Player1", new PlayerTank("tank", 100, 700));
            Sprites.Add("Player2", new PlayerTank("tank", 1100, 700));
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
        public PlayerTank(string image, int x, int y) : base(x, y, 40, 20, image) {
            fireAngle = 0;
            health = 100;
        }
        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
            //Harpreet put controls here
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
}
