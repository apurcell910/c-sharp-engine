using System;
using System.Drawing;

namespace SharpSlugsEngine
{
    /// <summary>
    /// A 2D Vector with various helper math functions
    /// </summary>
    public struct Vector2 : IEquatable<Vector2>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2"/> struct with given x and y components
        /// </summary>
        /// <param name="x">The x component of the new struct instance</param>
        /// <param name="y">The y component of the new struct instance</param>
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Gets a <see cref="Vector2"/> with components (0, 0)
        /// </summary>
        public static Vector2 Zero => new Vector2(0, 0);

        /// <summary>
        /// Gets a <see cref="Vector2"/> with components (1, 1)
        /// </summary>
        public static Vector2 One => new Vector2(1, 1);

        /// <summary>
        /// Gets a <see cref="Vector2"/> with components (0.5, 0.5)
        /// </summary>
        public static Vector2 Half => new Vector2(0.5f, 0.5f);

        /// <summary>
        /// Gets a <see cref="Vector2"/> with components (-1, 0)
        /// </summary>
        public static Vector2 Left => new Vector2(-1, 0);

        /// <summary>
        /// Gets a <see cref="Vector2"/> with components (1, 0)
        /// </summary>
        public static Vector2 Right => new Vector2(1, 0);

        /// <summary>
        /// Gets a <see cref="Vector2"/> with components (0, -1)
        /// </summary>
        public static Vector2 Up => new Vector2(0, -1);

        /// <summary>
        /// Gets a <see cref="Vector2"/> with components (0, 1)
        /// </summary>
        public static Vector2 Down => new Vector2(0, 1);

        /// <summary>
        /// Gets the x component of this <see cref="Vector2"/>
        /// </summary>
        public float X { get; private set; }

        /// <summary>
        /// Gets the y component of this <see cref="Vector2"/>
        /// </summary>
        public float Y { get; private set; }

        /// <summary>
        /// Gets the length of this <see cref="Vector2"/>
        /// </summary>
        public float Length => (float)Math.Sqrt((X * X) + (Y * Y));

        // Terrible hack to make StyleCop not want documentation here because documenting
        // all of these operators is just needless spam
        #region Not generated code
        public static implicit operator Vector2(PointF self) => new Vector2(self.X, self.Y);
        public static implicit operator PointF(Vector2 self) => new PointF(self.X, self.Y);

        public static implicit operator Vector2(Point self) => new Vector2(self.X, self.Y);
        public static explicit operator Point(Vector2 self) => new Point((int)self.X, (int)self.Y);

        public static implicit operator Vector2(SizeF self) => new Vector2(self.Width, self.Height);
        public static implicit operator SizeF(Vector2 self) => new SizeF(self.X, self.Y);

        public static implicit operator Vector2(Size self) => new Vector2(self.Width, self.Height);
        public static explicit operator Size(Vector2 self) => new Size((int)self.X, (int)self.Y);

        public static bool operator ==(Vector2 l, Vector2 r) => ((IEquatable<Vector2>)l).Equals(r);
        public static bool operator !=(Vector2 l, Vector2 r) => !(l == r);

        public static Vector2 operator -(Vector2 l, Vector2 r) => new Vector2(l.X - r.X, l.Y - r.Y);
        public static Vector2 operator -(Vector2 self) => Zero - self;
        public static Vector2 operator +(Vector2 l, Vector2 r) => new Vector2(l.X + r.X, l.Y + r.Y);
        public static Vector2 operator /(Vector2 l, Vector2 r) => new Vector2(l.X / r.X, l.Y / r.Y);
        public static Vector2 operator *(Vector2 l, Vector2 r) => new Vector2(l.X * r.X, l.Y * r.Y);

        public static Vector2 operator /(Vector2 l, float r) => new Vector2(l.X / r, l.Y / r);
        public static Vector2 operator *(Vector2 l, float r) => new Vector2(l.X * r, l.Y * r);
        #endregion

        /// <summary>
        /// Computes the Z component of the cross product of <paramref name="v1"/> and <paramref name="v2"/>
        /// </summary>
        /// <param name="v1">The first <see cref="Vector2"/></param>
        /// <param name="v2">The second <see cref="Vector2"/></param>
        /// <returns>The cross product of <paramref name="v1"/> and <paramref name="v2"/></returns>
        public static float CrossProduct(Vector2 v1, Vector2 v2)
        {
            return (v1.X * v2.Y) - (v2.X * v1.Y);
        }

        /// <summary>
        /// Computes the Z component of the cross product of <paramref name="v1"/> - <paramref name="sub"/> and <paramref name="v2"/> - <paramref name="sub"/>
        /// </summary>
        /// <param name="sub">The scaling factor of <paramref name="v1"/> and <paramref name="v2"/></param>
        /// <param name="v1">The first <see cref="Vector2"/></param>
        /// <param name="v2">The second <see cref="Vector2"/></param>
        /// <returns>The cross product of <paramref name="v1"/> - <paramref name="sub"/> and <paramref name="v2"/> - <paramref name="sub"/></returns>
        public static float CrossProduct(Vector2 sub, Vector2 v1, Vector2 v2)
        {
            return CrossProduct(v1 - sub, v2 - sub);
        }

        /// <summary>
        /// Checks equality between this <see cref="Vector2"/> and "<paramref name="other"/>"
        /// </summary>
        /// <param name="other">The <see cref="Vector2"/> to check equality against</param>
        /// <returns>Whether or not "<paramref name="other"/>" has equal components as this <see cref="Vector2"/></returns>
        bool IEquatable<Vector2>.Equals(Vector2 other)
        {
            return X == other.X && Y == other.Y;
        }
        
        /// <summary>
        /// Rotates the current vector by <paramref name="r"/> degrees about the point <paramref name="origin"/>
        /// </summary>
        /// <param name="origin">The point to rotate this <see cref="Vector2"/> around</param>
        /// <param name="r">The amount of degrees to rotate by</param>
        /// <returns>A new Vector2 containing the rotated point</returns>
        public Vector2 Rotate(Vector2 origin, float r)
        {
            r *= (float)Math.PI / 180;

            return new Vector2(
                (float)((Math.Cos(r) * (X - origin.X)) - (Math.Sin(r) * (Y - origin.Y))),
                (float)((Math.Sin(r) * (X - origin.X)) + (Math.Cos(r) * (Y - origin.Y)))) + origin;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> with the same component ratio as this one, but with <see cref="Length"/> equal to 1
        /// </summary>
        /// <returns>The newly created <see cref="Vector2"/></returns>
        public Vector2 Normalize()
        {
            float len = Length;

            if (len == 0)
            {
                return this;
            }

            return new Vector2(X / len, Y / len);
        }

        /// <summary>
        /// Checks equality between this <see cref="Vector2"/> and "<paramref name="obj"/>"
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to check equality against</param>
        /// <returns>Whether or not "<paramref name="obj"/>" has equal components as this <see cref="Vector2"/></returns>
        public override bool Equals(object obj)
        {
            if (obj is Vector2 vec)
            {
                return ((IEquatable<Vector2>)this).Equals(vec);
            }

            return false;
        }

        /// <summary>
        /// Calculates a hash of this <see cref="Vector2"/>
        /// </summary>
        /// <returns>The calculated hash</returns>
        public override int GetHashCode()
        {
            return new { X, Y }.GetHashCode();
        }

        /// <summary>
        /// Converts this <see cref="Vector2"/> to a string
        /// </summary>
        /// <returns>A string containing the components of this <see cref="Vector2"/></returns>
        public override string ToString() => $"(X: {X}, Y: {Y})";
    }
}
