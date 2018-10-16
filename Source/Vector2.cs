using System;

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

        public static Vector2 Zero => new Vector2(0, 0);

        public static Vector2 One => new Vector2(1, 1);

        public static Vector2 Half => new Vector2(0.5f, 0.5f);

        public static Vector2 Left => new Vector2(-1, 0);

        public static Vector2 Right => new Vector2(1, 0);

        public static Vector2 Up => new Vector2(0, -1);

        public static Vector2 Down => new Vector2(0, 1);

        //TODO: Math functions (Stretch goal?)
    }
}
