using System;
using System.Collections;

namespace SharpSlugsEngine
{
    public static class Extensions
    {
        /// <summary>
        /// Returns the index before <paramref name="i"/> in <paramref name="self"/>, wrapping if necessary
        /// </summary>
        public static int GetPrevIndex(this IList self, int i)
        {
            if (i < 0 || i >= self.Count)
            {
                throw new ArgumentOutOfRangeException("Argument i out of range");
            }

            return i == 0 ? self.Count - 1 : i - 1;
        }

        /// <summary>
        /// Returns the index after <paramref name="i"/> in <paramref name="self"/>, wrapping if necessary
        /// </summary>
        public static int GetNextIndex(this IList self, int i)
        {
            if (i < 0 || i >= self.Count)
            {
                throw new ArgumentOutOfRangeException("Argument i out of range");
            }

            return i == self.Count - 1 ? 0 : i + 1;
        }

        /// <summary>
        /// Returns the item before index <paramref name="i"/> in <paramref name="self"/>, wrapping if necessary
        /// </summary>
        public static T GetPrevItem<T>(this IList self, int i)
        {
            return (T)self[self.GetPrevIndex(i)];
        }

        /// <summary>
        /// Returns the item after index <paramref name="i"/> in <paramref name="self"/>, wrapping if necessary
        /// </summary>
        public static T GetNextItem<T>(this IList self, int i)
        {
            return (T)self[self.GetNextIndex(i)];
        }
    }
}
