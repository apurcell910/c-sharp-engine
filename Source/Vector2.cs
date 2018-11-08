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

        public static bool operator ==(Vector2 l, Vector2 r) => ((IEquatable<Vector2>)l).Equals(r);
        public static bool operator !=(Vector2 l, Vector2 r) => !(l == r);

        public static Vector2 operator -(Vector2 l, Vector2 r) => new Vector2(l.X - r.X, l.Y - r.Y);
        public static Vector2 operator -(Vector2 self) => Zero - self;
        public static Vector2 operator +(Vector2 l, Vector2 r) => new Vector2(l.X + r.X, l.Y + r.Y);
        public static Vector2 operator /(Vector2 l, Vector2 r) => new Vector2(l.X / r.X, l.Y / r.Y);
        public static Vector2 operator *(Vector2 l, Vector2 r) => new Vector2(l.X * r.X, l.Y * r.Y);

        public static Vector2 operator /(Vector2 l, float r) => new Vector2(l.X / r, l.Y / r);
        public static Vector2 operator *(Vector2 l, float r) => new Vector2(l.X * r, l.Y * r);

        public static Vector2 Zero => new Vector2(0, 0);

        public static Vector2 One => new Vector2(1, 1);

        public static Vector2 Half => new Vector2(0.5f, 0.5f);

        public static Vector2 Left => new Vector2(-1, 0);

        public static Vector2 Right => new Vector2(1, 0);

        public static Vector2 Up => new Vector2(0, -1);

        public static Vector2 Down => new Vector2(0, 1);
    }
}
