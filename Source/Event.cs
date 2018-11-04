using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace SharpSlugsEngine
{
    public class Event
    {

        public Event() { }

        public delegate void test1(Keys key, Point Location);

        public event test1 Test;

        public virtual void callEvent()
        {
            Test?.Invoke(Keys.Clear, Point.Empty);
        }
        public virtual void callEvent(Keys key)
        {
            Test?.Invoke(key, Point.Empty);
        }
        public virtual void callEvent(Point Location)
        {
            Test?.Invoke(Keys.Clear, Location);
        }
    }
}
