namespace SharpSlugsEngine
{
    public interface IUpdatable
    {
        bool Alive { get; }

        void Update(GameTime gameTime);
    }

    public interface IDrawable
    {
        bool Alive { get; }

        void Draw(GameTime gameTime);
    }
}
