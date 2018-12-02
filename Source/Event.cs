using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SharpSlugsEngine
{
    /// <summary>
    /// Used for mouse and Keyboard events
    /// </summary>
    /// <warning>Not fully implemented</warning>
    public class Event
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class.
        /// </summary>
        public Event()
        {
        }

        /// <summary>
        /// Used for <see cref="Snippets"/>
        /// </summary>
        /// <param name="key">key data to broadcast</param>
        /// <param name="Location">location data to broadcast</param>
        public delegate void Actions(Keys key, Vector2 Location);

        /// <summary>
        /// snippets of code to call in the event
        /// </summary>
        public event Actions Snippets;

        /// <summary>
        /// Calls the current event with no data
        /// </summary>
        public virtual void CallEvent()
        {
            Snippets?.Invoke(Keys.Clear, Vector2.Zero);
        }

        /// <summary>
        /// Calls the current event with key data
        /// </summary>
        /// <param name="key">key data to call with</param>
        public virtual void CallEvent(Keys key)
        {
            Snippets?.Invoke(key, Vector2.Zero);
        }

        /// <summary>
        /// Calls the current event with location data
        /// </summary>
        /// <param name="location">location to call with</param>
        public virtual void CallEvent(Vector2 location)
        {
            Snippets?.Invoke(Keys.Clear, location);
        }
    }
}
