using System;
using System.Drawing;
using SharpSlugsEngine;
using SharpSlugsEngine.Physics;

namespace Test_Game
{
    public class Asteroid : IUpdatable, IDrawable
    {
        private Vector2 topLeftOffset;
        private float hpWidth;

        private PolygonCollider poly;
        public Vector2[] Vertices => poly.Vertices;

        public Vector2 Position
        {
            get => poly.Position;
            set => poly.Position = value;
        }

        public Vector2 Velocity
        {
            get => poly.Velocity;
            set => poly.Velocity = value;
        }

        private Game _game;
        private float rotSpeed;

        public float Size { get; private set; }

        private int hp;
        private int maxHp;

        public bool Dead;

        bool IUpdatable.Alive => !Dead;
        bool IDrawable.Alive => !Dead;

        public Asteroid(Game game, Vector2 pos, Vector2 velocity)
        {
            _game = game;

            Random rnd = new Random();
            Size = (float)rnd.NextDouble() + 0.5f;
            poly = PolygonCollider.GenerateRandom(Size * 66.66f);

            Initialize(pos, velocity);
        }

        public Asteroid(Game game, Vector2[] verts, Vector2 pos, Vector2 velocity)
        {
            _game = game;

            Size = 1f;
            poly = new PolygonCollider(verts);

            Initialize(pos, velocity);
        }

        private void Initialize(Vector2 pos, Vector2 velocity)
        {
            rotSpeed = (float)new Random().NextDouble() * 50 - 25;

            hp = (int)(Size * 5);
            maxHp = hp;

            RectangleF rect = poly.GetBoundingBox();
            topLeftOffset = rect.Location;
            hpWidth = rect.Width;

            poly.IsPhysicsObject = true;
            poly.Position = pos;
            poly.Velocity = velocity;

            _game.AddUpdatable(this);
            _game.AddDrawable(this);
        }

        void IUpdatable.Update(GameTime gameTime)
        {
            if (Dead) return;

            poly.Position = poly.Position + poly.Velocity * (float)gameTime.deltaTime.TotalSeconds;

            if (poly.Position.X < -100 || poly.Position.X > _game.Graphics.WorldScale.X + 100 || poly.Position.Y < -100 || poly.Position.Y > _game.Graphics.WorldScale.Y + 100)
            {
                Dead = true;
            }

            poly.Rotation += rotSpeed * (float)gameTime.deltaTime.TotalSeconds * 5;
        }

        public bool CheckCollision(Vector2 loc)
            => poly.IsTouching(loc);

        public void Damage()
        {
            if (--hp <= 0)
            {
                Dead = true;
            }
        }

        void IDrawable.Draw(GameTime gameTime)
        {
            Vector2 topLeft = Position + topLeftOffset;

            _game.Graphics.DrawPolygon(poly.Vertices, Color.White, false);

            _game.Graphics.DrawRectangle(topLeft + Vector2.Up * 10, new Vector2(hpWidth, 10), Color.Red);
            _game.Graphics.DrawRectangle(topLeft + Vector2.Up * 10, new Vector2(hpWidth * (hp / (float)maxHp), 10), Color.Green);
        }
    }
}
