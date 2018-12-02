using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpSlugsEngine.Input
{
    /// <summary>
    /// Tracks current state of mouse.
    /// </summary>
    public class MouseManager : IDisposable
    {
        private readonly Game gameInternal;

        private MouseState asyncMouseState;
        private MouseState oldMouseState;
        private MouseState currentMouseState;

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseManager"/> class with the given parent <see cref="Game"/>
        /// </summary>
        /// <param name="game">The parent <see cref="Game"/> of the new <see cref="KeyboardManager"/></param>
        internal MouseManager(Game game)
        {
            gameInternal = game;
            HookForm();
        }

        /// <summary>
        /// Used for broadcasting location
        /// </summary>
        /// <param name="Location">Current mouse location</param>
        public delegate void BroadcastLocation(Vector2 Location);

        /// <summary>
        /// Used for all click events
        /// </summary>
        public delegate void MouseClick();

        /// <summary>
        /// Broadcasts the location to events.
        /// </summary>
        public event BroadcastLocation Broadcast;

        /// <summary>
        /// Called when left click occurs
        /// </summary>
        public event MouseClick LeftClick
        {
            add => _leftClick += value;
            remove => _leftClick -= value;
        }

        /// <summary>
        /// Called when right click occurs
        /// </summary>
        public event MouseClick RightClick
        {
            add => _rightClick += value;
            remove => _rightClick -= value;
        }

        /// <summary>
        /// Called when middle click occurs
        /// </summary>
        public event MouseClick MiddleClick
        {
            add => _middleClick += value;
            remove => _middleClick -= value;
        }

        /// <summary>
        /// Private backing field for <see cref="LeftClick"/>
        /// </summary>
        private event MouseClick _leftClick;

        /// <summary>
        /// Private backing field for <see cref="RightClick"/>
        /// </summary>
        private event MouseClick _rightClick;

        /// <summary>
        /// Private backing field for <see cref="MiddleClick"/>
        /// </summary>
        private event MouseClick _middleClick;

        /// <summary>
        /// Gets current state of mouse. Contains location and which parts are clicked.
        /// </summary>
        public MouseState State { get; private set; }

        /// <summary>
        /// Add an event to be called on left click
        /// </summary>
        /// <param name="e">event to be called</param>
        public void AddLeftClick(Event e)
        {
            LeftClick += e.CallEvent;
        }

        /// <summary>
        /// Remove an event to be called on left click
        /// </summary>
        /// <param name="e">event to be called</param>
        public void RemoveLeftClick(Event e)
        {
            LeftClick -= e.CallEvent;
        }

        /// <summary>
        /// Add an event to be called on right click
        /// </summary>
        /// <param name="e">event to be called</param>
        public void AddRightClick(Event e)
        {
            RightClick += e.CallEvent;
        }

        /// <summary>
        /// Remove an event to be called on right click
        /// </summary>
        /// <param name="e">event to be called</param>
        public void RemoveRightClick(Event e)
        {
            RightClick -= e.CallEvent;
        }

        /// <summary>
        /// Add an event to be called on middle click
        /// </summary>
        /// <param name="e">event to be called</param>
        public void AddMiddleClick(Event e)
        {
            MiddleClick += e.CallEvent;
        }

        /// <summary>
        /// Remove an event to be called on middle click
        /// </summary>
        /// <param name="e">event to be called</param>
        public void RemoveMiddleClick(Event e)
        {
            MiddleClick -= e.CallEvent;
        }

        /// <summary>
        /// Adds an event to get sent the location of the mouse constantly
        /// </summary>
        /// <param name="e">event to be called</param>
        public void AddLocationBind(Event e)
        {
            Broadcast += e.CallEvent;
        }
        
        /// <summary>
        /// Gets world location from camera.
        /// </summary>
        /// <param name="camera">sorurce camera</param>
        /// <returns>The world coordinates of the mouse</returns>
        public Vector2 WorldLoc(Camera camera)
        {
            return camera.CameraToWorld(State.Location - camera.DrawPosition);
        }

        /// <summary>
        /// When manager is disposed of, unhooks the form.
        /// </summary>
        void IDisposable.Dispose() => UnhookForm();

        /// <summary>
        /// Updates the current state of the mouse, calls on-click events
        /// </summary>
        internal void Update()
        {
            oldMouseState = currentMouseState;
            currentMouseState = asyncMouseState;

            int x = currentMouseState.X;
            int y = currentMouseState.Y;
            Vector2 loc = currentMouseState.Location;
            Click l = new Click(currentMouseState.Left.IsClicked, !oldMouseState.Left.IsClicked && currentMouseState.Left.IsClicked);
            Click r = new Click(currentMouseState.Right.IsClicked, !oldMouseState.Right.IsClicked && currentMouseState.Right.IsClicked);
            Click c = new Click(currentMouseState.Center.IsClicked, !oldMouseState.Center.IsClicked && currentMouseState.Center.IsClicked);

            MouseState now = new MouseState(x, y, l, r, c);
            State = now;
            Broadcast?.Invoke(currentMouseState.Location);

            if (_leftClick != null && now.Left.WasClicked)
            {
                _leftClick();
            }

            if (_rightClick != null && now.Right.WasClicked)
            {
                _rightClick();
            }

            if (_middleClick != null && now.Center.WasClicked)
            {
                _middleClick();
            }
        }

        /// <summary>
        /// Update mousestate when mousemove triggers.
        /// </summary>
        /// <param name="sender">source object</param>
        /// <param name="e">event information</param>
        private void MouseMove(object sender, MouseEventArgs e)
        {
            asyncMouseState.X = e.X;
            asyncMouseState.Y = e.Y;
            asyncMouseState.Location = e.Location;
        }

        /// <summary>
        /// Hooks the key events on the parent <see cref="Game"/>'s <see cref="Form"/>
        /// </summary>
        private void HookForm()
        {
            UnhookForm();

            gameInternal.Platform.Form.MouseDown += new MouseEventHandler(MouseDown);
            gameInternal.Platform.Form.MouseUp += new MouseEventHandler(MouseUp);
            gameInternal.Platform.Form.MouseMove += new MouseEventHandler(MouseMove);
        }

        /// <summary>
        /// Unhooks the key events on the parent <see cref="Game"/>'s <see cref="Form"/>
        /// </summary>
        private void UnhookForm()
        {
            gameInternal.Platform.Form.MouseDown -= new MouseEventHandler(MouseDown);
            gameInternal.Platform.Form.MouseUp -= new MouseEventHandler(MouseUp);
            gameInternal.Platform.Form.MouseMove -= new MouseEventHandler(MouseMove);
        }

        /// <summary>
        /// Update mousestate when mousedown triggers.
        /// </summary>
        /// <param name="sender">source object</param>
        /// <param name="e">event information</param>
        private void MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    asyncMouseState.Left = new Click(true);
                    break;
                case MouseButtons.Right:
                    asyncMouseState.Right = new Click(true);
                    break;
                case MouseButtons.Middle:
                    asyncMouseState.Center = new Click(true);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Update mousestate when mouseup triggers.
        /// </summary>
        /// <param name="sender">source object</param>
        /// <param name="e">event information</param>
        private void MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    asyncMouseState.Left = new Click(false);
                    break;
                case MouseButtons.Right:
                    asyncMouseState.Right = new Click(false);
                    break;
                case MouseButtons.Middle:
                    asyncMouseState.Center = new Click(false);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Stores status of the mouse
        /// </summary>
        public struct MouseState
        {
            public int X;
            public int Y;
            public Click Left;
            public Click Right;
            public Click Center;
            public Vector2 Location;

            /// <summary>
            /// Initializes a new instance of the <see cref="MouseState"/> struct.
            /// </summary>
            /// <param name="x">x position of mouse</param>
            /// <param name="y">y position of mouse</param>
            /// <param name="l">left mouse button status</param>
            /// <param name="r">right mouse button status</param>
            /// <param name="c">middle mouse button status</param>
            internal MouseState(int x, int y, Click l, Click r, Click c)
            {
                X = x;
                Y = y;
                Left = l;
                Right = r;
                Center = c;
                Location = new Vector2(x, y);
            }
        }

        /// <summary>
        /// Status of mouse button
        /// </summary>
        public struct Click
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Click"/> struct.
            /// </summary>
            /// <param name="clicked">Current status of button</param>
            internal Click(bool clicked)
            {
                IsClicked = clicked;
                WasClicked = false;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Click"/> struct.
            /// </summary>
            /// <param name="clicked">Current status of button</param>
            /// <param name="wasClicked">Previous status of button</param>
            internal Click(bool clicked, bool wasClicked)
            {
                IsClicked = clicked;
                WasClicked = wasClicked;
            }

            /// <summary>
            /// Gets a value indicating whether the mouse button is currently cicked.
            /// </summary>
            public bool IsClicked { get; private set; }

            /// <summary>
            /// Gets a value indicating whether the mouse button is immediately previously clicked.
            /// </summary>
            public bool WasClicked { get; private set; }
        }
    }
}
