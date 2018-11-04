using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SharpSlugsEngine
{
    public class Event
    {

        public Event() { }

        public delegate void test1(Keys key);

        public event test1 Test;

        public virtual void callEvent()
        {
            Test?.Invoke(Keys.Clear);
        }
        public virtual void callEvent(Keys key)
        {
            Test?.Invoke(key);
        }
    }
}
