using System.Drawing;
using SharpSlugsEngine;

namespace Test_Game
{
    public class Bullet : IUpdatable, IDrawable
    {
        private Game _game;

        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; private set; }
        public bool Dead;

        bool IUpdatable.Alive => !Dead;
        bool IDrawable.Alive => !Dead;

        public Bullet(Game game, Vector2 pos, Vector2 velocity)
        {
            _game = game;

            Position = pos;
            Velocity = velocity;

            _game.AddUpdatable(this);
            _game.AddDrawable(this);
        }

        void IUpdatable.Update(GameTime gameTime)
        {
            if (Dead) return;

            Position = Position + Velocity * (float)gameTime.deltaTime.TotalSeconds;

            if (Position.X < 0 || Position.X > _game.Graphics.WorldScale.X || Position.Y < 0 || Position.Y > _game.Graphics.WorldScale.Y)
            {
                Dead = true;
            }
        }

        void IDrawable.Draw(GameTime gameTime)
        {
            if (Dead) return;

            _game.Graphics.DrawCircle(Position, 5, Color.White);
        }
    }
}
