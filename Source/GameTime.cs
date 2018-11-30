using System;

namespace SharpSlugsEngine
{
    /// <summary>
    /// GameTime struct to keep track of Delta time and total time the game has been running.
    /// </summary>
    public struct GameTime
    {
        public TimeSpan DeltaTime;
        public TimeSpan TotalTime;
    }
}
