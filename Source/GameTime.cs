using System;

namespace SharpSlugsEngine
{
    /// <summary>
    /// GameTime struct to keep track of Delta time and total time the game has been running.
    /// </summary>
    public struct GameTime
    {
        /// <summary>
        /// Gets the DeltaTime, time since last frame
        /// </summary>
        public TimeSpan DeltaTime { get; private set; }

        /// <summary>
        /// Gets the TotalTime since the game started
        /// </summary>
        public TimeSpan TotalTime { get; private set; }
    }
}
