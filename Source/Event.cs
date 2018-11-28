using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SharpSlugsEngine
{
    public class Event
    {
        public Event()
        {
        }

        public delegate void Actions(Keys key, Vector2 Location);

        public event Actions Snippets;

        public virtual void CallEvent()
        {
            Snippets?.Invoke(Keys.Clear, Vector2.Zero);
        }

        public virtual void CallEvent(Keys key)
        {
            Snippets?.Invoke(key, Vector2.Zero);
        }

        public virtual void CallEvent(Vector2 location)
        {
            Snippets?.Invoke(Keys.Clear, location);
        }
    }
}
