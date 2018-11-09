namespace SharpSlugsEngine.Physics
{
    public abstract class Collider
    {
        /// <summary>
        /// Whether or not the collider should act as a trigger (ie non-solid)
        /// </summary>
        public bool IsTrigger { get; set; }

        private Vector2 _position;
        /// <summary>
        /// The current world position of this collider
        /// </summary>
        public Vector2 Position
        {
            get => _position;
            set
            {
                if (!float.IsNaN(value.X) && !float.IsNaN(value.Y) && !float.IsInfinity(value.X) && !float.IsInfinity(value.Y))
                {
                    _position = value;
                }
            }
        }

        private float _rotation;
        /// <summary>
        /// The current rotation of this collider
        /// </summary>
        public float Rotation
        {
            get => _rotation;
            set
            {
                if (!float.IsNaN(value) && !float.IsInfinity(value))
                {
                    _rotation = value;
                }

                while (_rotation < 0) _rotation += 360;
                while (_rotation >= 360) _rotation -= 360;
            }
        }

        private Vector2 _scale = Vector2.One;
        /// <summary>
        /// The current scale multiplier of this collider
        /// </summary>
        public Vector2 Scale
        {
            get => _scale;
            set
            {
                if (value.X > 0 && value.Y > 0 && !float.IsNaN(value.X) && !float.IsNaN(value.Y) && !float.IsInfinity(value.X) && !float.IsInfinity(value.Y))
                {
                    _scale = value;
                }
            }
        }

        public abstract bool IsTouching(Collider other);

        public delegate void ColliderEvent(Collider other);

        protected event ColliderEvent _colliderEnter;
        /// <summary>
        /// Called when another collider enters this collider
        /// </summary>
        public event ColliderEvent ColliderEnter
        {
            add => _colliderEnter += value;
            remove => _colliderEnter -= value;
        }

        protected event ColliderEvent _colliderLeave;
        /// <summary>
        /// Called when another collider leaves this collider
        /// </summary>
        public event ColliderEvent ColliderLeave
        {
            add => _colliderLeave += value;
            remove => _colliderLeave -= value;
        }

        protected event ColliderEvent _colliderStay;
        /// <summary>
        /// Called continuously while another collider is touching this collider
        /// </summary>
        public event ColliderEvent ColliderStay
        {
            add => _colliderStay += value;
            remove => _colliderStay -= value;
        }
    }
}
