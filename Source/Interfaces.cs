namespace SharpSlugsEngine
{
    /// <summary>
    /// Provides functionality for an object to be updated by a <see cref="Game"/> after being passed to <see cref="Game.AddUpdatable(IUpdatable)"/>
    /// </summary>
    public interface IUpdatable
    {
        /// <summary>
        /// Gets a value indicating whether the <see cref="Game"/> should continue updating the <see cref="IUpdatable"/>
        /// </summary>
        bool Alive { get; }

        /// <summary>
        /// Update function called from the <see cref="Game"/> prior to the main <see cref="Game.Update(GameTime)"/> call
        /// </summary>
        /// <param name="gameTime">Struct containing delta time information</param>
        void Update(GameTime gameTime);
    }

    /// <summary>
    /// Provides functionality for an object to receive draw calls after being passed to <see cref="Game.AddDrawable(IDrawable)"/>
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// Gets a value indicating whether the <see cref="Game"/> should continue to draw this <see cref="IDrawable"/>
        /// </summary>
        bool Alive { get; }

        /// <summary>
        /// Draw function called from the <see cref="Game"/> prior to the main <see cref="Game.Draw(GameTime)"/> call
        /// </summary>
        /// <param name="gameTime">Struct containing delta time information</param>
        void Draw(GameTime gameTime);
    }
}
