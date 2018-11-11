using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace SharpSlugsEngine.Input
{
    public class MouseManager : IDisposable
    {
        private readonly Game _game;

        private MouseState _asyncMouseState;
        private MouseState _oldMouseState;
        private MouseState _currentMouseState;

        public MouseState State { get; private set; }

        internal void Update()
        {
            _oldMouseState = _currentMouseState;
            _currentMouseState = _asyncMouseState;

            int x = _currentMouseState.X;
            int y = _currentMouseState.Y;
            Point Loc = _currentMouseState.Location;
            Click L = new Click(_currentMouseState.Left.IsClicked, !_oldMouseState.Left.IsClicked && _currentMouseState.Left.IsClicked);
            Click R = new Click(_currentMouseState.Right.IsClicked, !_oldMouseState.Right.IsClicked && _currentMouseState.Right.IsClicked);
            Click C = new Click(_currentMouseState.Center.IsClicked, !_oldMouseState.Center.IsClicked && _currentMouseState.Center.IsClicked);

            MouseState now = new MouseState(x, y, L, R, C);
            State = now;
            Broadcast?.Invoke(_currentMouseState.Location);

            if (_leftClick != null && now.Left.WasClicked) _leftClick();
            if (_rightClick != null && now.Right.WasClicked) _rightClick();
            if (_middleClick != null && now.Center.WasClicked) _middleClick();
        }

        internal MouseManager(Game game)
        {
            _game = game;
            HookForm();
        }

        private void UnhookForm()
        {
            _game.platform.form.MouseDown -= new MouseEventHandler(MouseDown);
            _game.platform.form.MouseUp -= new MouseEventHandler(MouseUp);
            _game.platform.form.MouseMove -= new MouseEventHandler(MouseMove);
        }

        private void HookForm()
        {
            UnhookForm();

            _game.platform.form.MouseDown += new MouseEventHandler(MouseDown);
            _game.platform.form.MouseUp += new MouseEventHandler(MouseUp);
            _game.platform.form.MouseMove += new MouseEventHandler(MouseMove);
        }

        private void MouseDown(Object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    _asyncMouseState.Left = new Click(true);
                    break;
                case MouseButtons.Right:
                    _asyncMouseState.Right = new Click(true);
                    break;
                case MouseButtons.Middle:
                    _asyncMouseState.Center = new Click(true);
                    break;
                default:
                    break;
            }
        }

        private void MouseUp(Object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    _asyncMouseState.Left = new Click(false);
                    break;
                case MouseButtons.Right:
                    _asyncMouseState.Right = new Click(false);
                    break;
                case MouseButtons.Middle:
                    _asyncMouseState.Center = new Click(false);
                    break;
                default:
                    break;
            }
        }

        private void MouseMove(Object sender, MouseEventArgs e)
        {
            _asyncMouseState.X = e.X;
            _asyncMouseState.Y = e.Y;
            _asyncMouseState.Location = e.Location;
        }

        public delegate void broadcastLocation(Point Location);

        public event broadcastLocation Broadcast;

        public delegate void MouseClick();

        private event MouseClick _leftClick;
        public event MouseClick LeftClick
        {
            add => _leftClick += value;
            remove => _leftClick -= value;
        }

        private event MouseClick _rightClick;
        public event MouseClick RightClick
        {
            add => _rightClick += value;
            remove => _rightClick -= value;
        }

        private event MouseClick _middleClick;
        public event MouseClick MiddleClick
        {
            add => _middleClick += value;
            remove => _middleClick -= value;
        }

        public void AddLeftClick(Event e)
        {
            LeftClick += e.callEvent;
        }

        public void RemoveLeftClick(Event e)
        {
            LeftClick -= e.callEvent;
        }

        public void AddRightClick(Event e)
        {
            RightClick += e.callEvent;
        }

        public void RemoveRightClick(Event e)
        {
            RightClick -= e.callEvent;
        }

        public void AddMiddleClick(Event e)
        {
            MiddleClick += e.callEvent;
        }

        public void RemoveMiddleClick(Event e)
        {
            MiddleClick -= e.callEvent;
        }

        public void AddLocationBind(Event e)
        {
            Broadcast += e.callEvent;

        }

        public struct MouseState
        {
            internal MouseState(int x, int y, Click L, Click R, Click C)
            {
                X = x;
                Y = y;
                Left = L;
                Right = R;
                Center = C;
                Location = new Point(x, y);
            }
            public int X;
            public int Y;
            public Click Left;
            public Click Right;
            public Click Center;
            public Point Location;
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

        void IDisposable.Dispose() => UnhookForm();
    }
}
