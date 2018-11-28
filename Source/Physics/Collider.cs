using System.Drawing;

namespace SharpSlugsEngine.Physics
{
    /// <summary>
    /// Generic collision class composed of <see cref="PTriangle"/>s
    /// </summary>
    public abstract class Collider
    {
        private bool isPhysicsObjectInternal;
        private Vector2 positionInternal;
        private Vector2 velocityInternal;
        private float rotationInternal;
        private Vector2 scaleInternal = Vector2.One;

        /// <summary>
        /// Used for all events relating to <see cref="Collider"/>s
        /// </summary>
        /// <param name="self">The <see cref="Collider"/> calling the event</param>
        /// <param name="other">The <see cref="Collider"/> detected as touching the calling <see cref="Collider"/></param>
        public delegate void ColliderEvent(Collider self, Collider other);

        /// <summary>
        /// Called when another collider enters this collider
        /// </summary>
        public event ColliderEvent ColliderEnter
        {
            add => ColliderEnterInternal += value;
            remove => ColliderEnterInternal -= value;
        }

        /// <summary>
        /// Called when another collider leaves this collider
        /// </summary>
        public event ColliderEvent ColliderLeave
        {
            add => ColliderLeaveInternal += value;
            remove => ColliderLeaveInternal -= value;
        }

        /// <summary>
        /// Called continuously while another collider is touching this collider
        /// </summary>
        public event ColliderEvent ColliderStay
        {
            add => ColliderStayInternal += value;
            remove => ColliderStayInternal -= value;
        }

        /// <summary>
        /// The backing field for <see cref="ColliderEnter"/>
        /// </summary>
        internal event ColliderEvent ColliderEnterInternal; // TODO: Not implemented

        /// <summary>
        /// The backing field for <see cref="ColliderLeave"/>
        /// </summary>
        internal event ColliderEvent ColliderLeaveInternal; // TODO: Not implemented

        /// <summary>
        /// The backing field for <see cref="ColliderStay"/>
        /// </summary>
        internal event ColliderEvent ColliderStayInternal; // TODO: Not implemented

        /// <summary>
        /// Gets the triangles that make up this collider
        /// </summary>
        public PTriangle[] Triangles => (PTriangle[])TrianglesInternal.Clone();

        /// <summary>
        /// Gets the vertices that make up the edges of this collider
        /// </summary>
        public Vector2[] Vertices => (Vector2[])VerticesInternal.Clone();
        
        /// <summary>
        /// Gets or sets a value indicating whether or not the collider should act as a trigger (ie non-solid)
        /// </summary>
        public bool IsTrigger { get; set; } // TODO: Not implemented
        
        /// <summary>
        /// Gets or sets a value indicating whether or not the collider should receive physics updates (Gravity, velocity, etc)
        /// </summary>
        public bool IsPhysicsObject // TODO: Not implemented
        {
            get => isPhysicsObjectInternal;
            set
            {
                isPhysicsObjectInternal = value;

                if (!isPhysicsObjectInternal)
                {
                    velocityInternal = Vector2.Zero;
                }
            }
        }
        
        /// <summary>
        /// Gets or sets the current world position of this collider
        /// </summary>
        public Vector2 Position
        {
            get => positionInternal;
            set
            {
                if (!float.IsNaN(value.X) && !float.IsNaN(value.Y) && !float.IsInfinity(value.X) && !float.IsInfinity(value.Y))
                {
                    if (TrianglesInternal != null && VerticesInternal != null)
                    {
                        for (int i = 0; i < TrianglesInternal.Length; i++)
                        {
                            TrianglesInternal[i] = new PTriangle(
                                TrianglesInternal[i].VertexOne + value - positionInternal, 
                                TrianglesInternal[i].VertexTwo + value - positionInternal, 
                                TrianglesInternal[i].VertexThree + value - positionInternal);
                        }

                        for (int i = 0; i < VerticesInternal.Length; i++)
                        {
                            VerticesInternal[i] += value - positionInternal;
                        }
                    }

                    positionInternal = value;
                }
            }
        }
        
        /// <summary>
        /// Gets or sets the current velocity of this collider
        /// </summary>
        public Vector2 Velocity // TODO: Not implemented
        {
            get => velocityInternal;
            set
            {
                if (isPhysicsObjectInternal && !float.IsNaN(value.X) && !float.IsNaN(value.Y) && !float.IsInfinity(value.X) && !float.IsInfinity(value.Y))
                {
                    velocityInternal = value;
                }
            }
        }
        
        /// <summary>
        /// Gets or sets the current rotation of this collider
        /// </summary>
        public float Rotation
        {
            get => rotationInternal;
            set
            {
                if (!float.IsNaN(value) && !float.IsInfinity(value))
                {
                    while (value < 0)
                    {
                        value += 360;
                    }

                    while (value >= 360)
                    {
                        value -= 360;
                    }

                    if (VerticesInternal != null && TrianglesInternal != null && VerticesInternal.Length > 0 && TrianglesInternal.Length > 0)
                    {
                        // Find center point of collider
                        Vector2 centerPoint = GetCenter();

                        // Difference in current and new rotation
                        float rot = value - rotationInternal;

                        for (int i = 0; i < VerticesInternal.Length; i++)
                        {
                            VerticesInternal[i] = VerticesInternal[i].Rotate(centerPoint, rot);
                        }

                        for (int i = 0; i < TrianglesInternal.Length; i++)
                        {
                            TrianglesInternal[i] = new PTriangle(
                                TrianglesInternal[i].VertexOne.Rotate(centerPoint, rot), 
                                TrianglesInternal[i].VertexTwo.Rotate(centerPoint, rot),
                                TrianglesInternal[i].VertexThree.Rotate(centerPoint, rot));
                        }
                    }

                    rotationInternal = value;
                }
            }
        }
        
        /// <summary>
        /// Gets or sets the current scale multiplier of this collider
        /// </summary>
        public Vector2 Scale // TODO: Not implemented
        {
            get => scaleInternal;
            set
            {
                if (value.X > 0 && value.Y > 0 && !float.IsNaN(value.X) && !float.IsNaN(value.Y) && !float.IsInfinity(value.X) && !float.IsInfinity(value.Y))
                {
                    scaleInternal = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the triangles the make up this collider.
        /// Note: This is the actual <see cref="PTriangle"/> array, modify with caution
        /// </summary>
        protected PTriangle[] TrianglesInternal { get; set; }

        /// <summary>
        /// Gets or sets the vertices that make up the edges of this collider.
        /// Note: This is the actual <see cref="Vector2"/> array, modify with caution
        /// </summary>
        protected Vector2[] VerticesInternal { get; set; }

        /// <summary>
        /// Calculates whether or not this <see cref="Collider"/> is touching the <see cref="Collider"/> "<paramref name="other"/>"
        /// </summary>
        /// <param name="other">The <see cref="Collider"/> to check collision against</param>
        /// <returns>A bool indicating whether or not the <see cref="Collider"/>s are touching</returns>
        public bool IsTouching(Collider other)
        {
            if (TrianglesInternal == null || TrianglesInternal.Length == 0 || other == null || other.TrianglesInternal == null || other.TrianglesInternal.Length == 0)
            {
                return false;
            }

            foreach (PTriangle selfTri in TrianglesInternal)
            {
                foreach (PTriangle otherTri in other.TrianglesInternal)
                {
                    if (selfTri.IsTouching(otherTri))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Calculates whether or not this <see cref="Collider"/> is touching the <see cref="Vector2"/> "<paramref name="point"/>"
        /// </summary>
        /// <param name="point">The <see cref="Vector2"/> to check collision against</param>
        /// <returns>A bool indicating whether or not the <see cref="Vector2"/> is within this <see cref="Collider"/></returns>
        public bool IsTouching(Vector2 point)
        {
            if (TrianglesInternal == null || TrianglesInternal.Length == 0)
            {
                return false;
            }

            foreach (PTriangle tri in TrianglesInternal)
            {
                if (tri.ContainsPoint(point))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Calculates a bounding box for this <see cref="Collider"/>
        /// </summary>
        /// <returns>a <see cref="RectangleF"/> containing the bounding box in world coordinates</returns>
        public RectangleF GetBoundingBox()
        {
            if (VerticesInternal == null || VerticesInternal.Length == 0)
            {
                return default(RectangleF);
            }

            float minX, maxX, minY, maxY;
            minX = maxX = VerticesInternal[0].X;
            minY = maxY = VerticesInternal[0].Y;

            foreach (Vector2 vert in VerticesInternal)
            {
                if (vert.X < minX)
                {
                    minX = vert.X;
                }
                else if (vert.X > maxX)
                {
                    maxX = vert.X;
                }

                if (vert.Y < minY)
                {
                    minY = vert.Y;
                }
                else if (vert.Y > maxY)
                {
                    maxY = vert.Y;
                }
            }

            return new RectangleF(minX, minY, maxX - minX, maxY - minY);
        }

        /// <summary>
        /// Calculates the center point of this <see cref="Collider"/> as the average of all points in <see cref="Vertices"/>
        /// </summary>
        /// <returns>The center point of this <see cref="Collider"/></returns>
        public Vector2 GetCenter()
        {
            Vector2 centerPoint = Vector2.Zero;
            
            if (VerticesInternal == null || VerticesInternal.Length == 0)
            {
				return centerPoint;
			}
            
            foreach (Vector2 vert in VerticesInternal)
            {
                centerPoint += vert;
            }

            return centerPoint / VerticesInternal.Length;
        }
    }
}
