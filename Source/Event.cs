using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace SharpSlugsEngine
{
    public class Event
    {

        public Event() { }

        public delegate void test1(Keys key, Vector2 Location);

        public event test1 Test;

        public virtual void callEvent()
        {
            Test?.Invoke(Keys.Clear, Vector2.Zero);
        }
        public virtual void callEvent(Keys key)
        {
            Test?.Invoke(key, Vector2.Zero);
        }
        public virtual void callEvent(Vector2 Location)
        {
            Test?.Invoke(Keys.Clear, Location);
        }
    }
}
