using System;
using System.Drawing;

namespace SharpSlugsEngine
{
    public struct Vector2 : IEquatable<Vector2>
    {
        //Private set because mutable structs are bad practice
        public float X { get; private set; }
        public float Y { get; private set; }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        bool IEquatable<Vector2>.Equals(Vector2 other)
        {
            return X == other.X && Y == other.Y;
        }
        
        /// <summary>
        /// Rotates the current vector by <paramref name="r"/> degrees about the point <paramref name="origin"/>
        /// </summary>
        /// <returns>A new Vector2 containing the rotated point</returns>
        public Vector2 Rotate(Vector2 origin, float r)
        {
            r *= (float)Math.PI / 180;

            return new Vector2((float)(Math.Cos(r) * (X - origin.X) - Math.Sin(r) * (Y - origin.Y) + origin.X),
                (float)(Math.Sin(r) * (X - origin.X) + Math.Cos(r) * (Y - origin.Y) + origin.Y));
        }

        public Vector2 Normalize()
        {
            float len = Length;

            if (len == 0)
            {
                return this;
            }

            return new Vector2(X / len, Y / len);
        }

        public float Length => (float)Math.Sqrt(X * X + Y * Y);

        public override bool Equals(object obj)
        {
            if (obj is Vector2 vec)
            {
                return ((IEquatable<Vector2>)this).Equals(vec);
            }

            return false;
        }

        public override int GetHashCode() => new { X, Y }.GetHashCode();

        public override string ToString() => $"(X: {X}, Y: {Y})";

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

        //Static math functions
        /// <summary>
        /// Computes the Z component of the cross product of <paramref name="v1"/> and <paramref name="v2"/>
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static float CrossProduct(Vector2 v1, Vector2 v2)
        {
            return v1.X * v2.Y - v2.X * v1.Y;
        }
        
        /// <summary>
        /// Computes the Z component of the cross product of <paramref name="v2"/> - <paramref name="v1"/> and <paramref name="v3"/> - <paramref name="v1"/>
        /// </summary>
        /// <returns></returns>
        public static float CrossProduct(Vector2 v1, Vector2 v2, Vector2 v3)
        {
            return CrossProduct(v2 - v1, v3 - v1);
        }

        //Vector2 macros
        public static Vector2 Zero => new Vector2(0, 0);
        public static Vector2 One => new Vector2(1, 1);
        public static Vector2 Half => new Vector2(0.5f, 0.5f);
        public static Vector2 Left => new Vector2(-1, 0);
        public static Vector2 Right => new Vector2(1, 0);
        public static Vector2 Up => new Vector2(0, -1);
        public static Vector2 Down => new Vector2(0, 1);
    }
}
