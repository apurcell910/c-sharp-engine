using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSlugsEngine.Sprites //naming namespace Sprite breaks Sprite.cs
{
    public class SpriteEvents
    {
        public Dictionary<string, SEvent> events = new Dictionary<string, SEvent>();
        public SpriteList sprites;
        public SpriteEvents(SpriteList list)
        {
            sprites = list;
        }

        public void add(string S, SEvent E)
        {
            events.Add(S, E);
        }

        public void delete(string S)
        {
            events.Remove(S);
        }

        public void enable(string S)
        {
            events[S].enable();
        }

        public void disable(string S)
        {
            events[S].disable();
        }

        public void swap(string S)
        {
            events[S].swap();
        }

        public void Update()
        {
            foreach (KeyValuePair<string, SEvent> eve in events)
                if (eve.Value.Target.alive && eve.Value.On)
                    eve.Value.call();
        }
        
    }

    public class SEvent
    {
        public Sprite Target;
        public bool On;
        public delegate void SpriteAction();
        public event SpriteAction change;

        public SEvent(Sprite target) {
            Target = target;
            On = true;
        }

        public void call() {
            change?.Invoke();
        }

        public void enable()
        {
            On = false;
        }

        public void disable()
        {
            On = true;
        }

        public void swap()
        {
            On = !On;
        }

    }
}
