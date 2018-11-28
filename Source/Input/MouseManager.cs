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

        public delegate void BroadcastLocation(Vector2 Location);

        public delegate void MouseClick();

        public event BroadcastLocation Broadcast;

        public event MouseClick LeftClick
        {
            add => _leftClick += value;
            remove => _leftClick -= value;
        }
        
        public event MouseClick RightClick
        {
            add => _rightClick += value;
            remove => _rightClick -= value;
        }

        public event MouseClick MiddleClick
        {
            add => _middleClick += value;
            remove => _middleClick -= value;
        }

        private event MouseClick _leftClick;
        private event MouseClick _rightClick;
        private event MouseClick _middleClick;

        /// <summary>
        /// Gets current state of mouse. Contains location and which parts are clicked.
        /// </summary>
        public MouseState State { get; private set; }
                
        public void AddLeftClick(Event e)
        {
            LeftClick += e.CallEvent;
        }

        public void RemoveLeftClick(Event e)
        {
            LeftClick -= e.CallEvent;
        }

        public void AddRightClick(Event e)
        {
            RightClick += e.CallEvent;
        }

        public void RemoveRightClick(Event e)
        {
            RightClick -= e.CallEvent;
        }

        public void AddMiddleClick(Event e)
        {
            MiddleClick += e.CallEvent;
        }

        public void RemoveMiddleClick(Event e)
        {
            MiddleClick -= e.CallEvent;
        }

        public void AddLocationBind(Event e)
        {
            Broadcast += e.CallEvent;
        }
            
        public Vector2 WorldLoc(Camera camera)
        {
            return camera.CameraToWorld(State.Location - camera.DrawPosition);
        }

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

        public struct MouseState
        {
            public int X;
            public int Y;
            public Click Left;
            public Click Right;
            public Click Center;
            public Vector2 Location;

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

        public struct Click
        {
            internal Click(bool clicked)
            {
                IsClicked = clicked;
                WasClicked = false;
            }

            internal Click(bool clicked, bool wasClicked)
            {
                IsClicked = clicked;
                WasClicked = wasClicked;
            }

            public bool IsClicked { get; private set; }

            public bool WasClicked { get; private set; }
        }
    }
}
