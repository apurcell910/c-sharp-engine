using System;

namespace SharpSlugsEngine
{
    public struct Vector2 : IEquatable<Vector2>
    {
        //Private set because mutable structs are bad practice
        public int X { get; private set; }
        public int Y { get; private set; }

        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        bool IEquatable<Vector2>.Equals(Vector2 other)
        {
            return X == other.X && Y == other.Y;
        }

        //TODO: Math functions (Stretch goal?)
    }
}
