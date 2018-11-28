using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSlugsEngine.Sprites // naming namespace Sprite breaks Sprite.cs
{
    /// <summary>
    /// Contains a list of actions to modify sprites every update cycle.
    /// </summary>
    public class SpriteEvents
    {
        public Dictionary<string, SEvent> Events = new Dictionary<string, SEvent>();
        public SpriteList Sprites;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteEvents"/> class
        /// </summary>
        /// <param name="list">Current list of sprites</param>
        public SpriteEvents(SpriteList list)
        {
            Sprites = list;
        }

        /// <summary>
        /// Adds an event to the list
        /// </summary>
        /// <param name="s">Name of event to be added</param>
        /// <param name="e">Event to be deleted</param>
        public void Add(string s, SEvent e)
        {
            Events.Add(s, e);
        }

        /// <summary>
        /// Removes an event from the list
        /// </summary>
        /// <param name="s">Name of event to be deleted</param>
        public void Delete(string s)
        {
            Events.Remove(s);
        }

        /// <summary>
        /// Enables an event on the list
        /// </summary>
        /// <param name="s">Name of event to enable</param>
        public void Enable(string s)
        {
            Events[s].Enable();
        }

        /// <summary>
        /// Disables an event on the list
        /// </summary>
        /// <param name="s">Name of event to disable</param>
        public void Disable(string s)
        {
            Events[s].Disable();
        }

        /// <summary>
        /// Swaps the state (enabled or disabled) of the event
        /// </summary>
        /// <param name="s">Name of the event to toggle</param>
        public void Swap(string s)
        {
            Events[s].Swap();
        }
        
        /// <summary>
        /// During the update cycle, calls each enabled event
        /// </summary>
        public void Update()
        {
            foreach (KeyValuePair<string, SEvent> eve in Events)
            {
                if (eve.Value.Target.alive && eve.Value.On)
                {
                    eve.Value.Call();
                }
            }
        }
    }

    /// <summary>
    /// Event storing code snippets
    /// </summary>
    public class SEvent
    {
        public Sprite Target;
        public bool On;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SEvent"/> class.
        /// </summary>
        /// <param name="target">Sprite source of the event</param>
        public SEvent(Sprite target)
        {
            Target = target;
            On = true;
        }

        /// <summary>
        /// Generic delegate.
        /// </summary>
        public delegate void SpriteAction();

        /// <summary>
        /// Store code snippet here.
        /// </summary>
        public event SpriteAction Change;

        /// <summary>
        /// Calls the code in the event
        /// </summary>
        public void Call()
        {
            Change?.Invoke();
        }
        
        /// <summary>
        /// Enables the event.
        /// </summary>
        public void Enable()
        {
            On = false;
        }

        /// <summary>
        /// Disables the event
        /// </summary>
        public void Disable()
        {
            On = true;
        }

        /// <summary>
        /// Toggles the event's enable/disable state.
        /// </summary>
        public void Swap()
        {
            On = !On;
        }
    }
}
