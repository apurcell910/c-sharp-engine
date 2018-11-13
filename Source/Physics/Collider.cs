namespace SharpSlugsEngine.Physics
{
    public abstract class Collider
    {
        /// <summary>
        /// The triangles that make up this collider
        /// </summary>
        public Triangle[] Triangles { get; protected set; }

        public Vector2[] Vertices { get; protected set; }

        /// <summary>
        /// Whether or not the collider should act as a trigger (ie non-solid)
        /// </summary>
        public bool IsTrigger { get; set; }

        private bool _isPhysicsObject;
        /// <summary>
        /// Whether or not the collider should receive physics updates (Gravity, velocity, etc)
        /// </summary>
        public bool IsPhysicsObject
        {
            get => _isPhysicsObject;
            set
            {
                _isPhysicsObject = value;

                if (!_isPhysicsObject)
                {
                    _velocity = Vector2.Zero;
                }
            }
        }

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
                    if (Triangles != null)
                    {
                        for (int i = 0; i < Triangles.Length; i++)
                        {
                            Triangles[i] = new Triangle(Triangles[i].VertexOne + value - _position, Triangles[i].VertexTwo + value - _position, Triangles[i].VertexThree + value - _position);
                        }

                        for (int i = 0; i < Vertices.Length; i++)
                        {
                            Vertices[i] += value - _position;
                        }
                    }

                    _position = value;
                }
            }
        }

        private Vector2 _velocity;
        /// <summary>
        /// The current velocity of this collider
        /// </summary>
        public Vector2 Velocity
        {
            get => _velocity;
            set
            {
                if (!_isPhysicsObject && !float.IsNaN(value.X) && !float.IsNaN(value.Y) && !float.IsInfinity(value.X) && !float.IsInfinity(value.Y))
                {
                    _velocity = value;
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
                    while (value < 0) value += 360;
                    while (value >= 360) value -= 360;

                    if (Vertices != null && Triangles != null && Vertices.Length > 0 && Triangles.Length > 0)
                    {
                        //Find center point of collider
                        Vector2 centerPoint = Vector2.Zero;
                        foreach (Vector2 vert in Vertices)
                        {
                            centerPoint += vert;
                        }

                        centerPoint /= Vertices.Length;

                        //Difference in current and new rotation
                        float rot = value - _rotation;

                        for (int i = 0; i < Vertices.Length; i++)
                        {
                            Vertices[i] = Vertices[i].Rotate(centerPoint, rot);
                        }

                        for (int i = 0; i < Triangles.Length; i++)
                        {
                            Triangles[i] = new Triangle(Triangles[i].VertexOne.Rotate(centerPoint, rot), Triangles[i].VertexTwo.Rotate(centerPoint, rot),
                                Triangles[i].VertexThree.Rotate(centerPoint, rot));
                        }
                    }

                    _rotation = value;
                }
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

        public bool IsTouching(Collider other)
        {
            if (Triangles == null || Triangles.Length == 0 || other == null || other.Triangles == null || other.Triangles.Length == 0)
            {
                return false;
            }

            foreach (Triangle selfTri in Triangles)
            {
                foreach (Triangle otherTri in other.Triangles)
                {
                    if (selfTri.IsTouching(otherTri))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsTouching(Vector2 point)
        {
            if (Triangles == null || Triangles.Length == 0)
            {
                return false;
            }

            foreach (Triangle tri in Triangles)
            {
                if (tri.ContainsPoint(point))
                {
                    return true;
                }
            }

            return false;
        }

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
