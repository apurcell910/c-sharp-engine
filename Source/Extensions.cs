using System;
using System.Collections;
using System.IO;

namespace SharpSlugsEngine
{
    /// <summary>
    /// Contains various extension methods used by the library
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Writes the specified bytes into the <see cref="MemoryStream"/>
        /// </summary>
        /// <param name="self">The <see cref="MemoryStream"/> to write into</param>
        /// <param name="buffer">The bytes to write</param>
        public static void Write(this MemoryStream self, params byte[] buffer)
        {
            self.Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Returns the index before <paramref name="i"/> in <paramref name="self"/>, wrapping if necessary
        /// </summary>
        /// <param name="self">The <see cref="IList"/> object to pull indices from</param>
        /// <param name="i">The index to search before</param>
        /// <returns>The calculated index</returns>
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
        /// <param name="self">The <see cref="IList"/> object to pull indices from</param>
        /// <param name="i">The index to search after</param>
        /// <returns>The calculated index</returns>
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
        /// <typeparam name="T">The <see cref="Type"/> to cast the returned object to</typeparam>
        /// <param name="self">The <see cref="IList"/> object to pull objects from</param>
        /// <param name="i">The index to search before</param>
        /// <returns>The item at the index before <paramref name="i"/> in <paramref name="self"/></returns>
        public static T GetPrevItem<T>(this IList self, int i)
        {
            return (T)self[self.GetPrevIndex(i)];
        }

        /// <summary>
        /// Returns the item after index <paramref name="i"/> in <paramref name="self"/>, wrapping if necessary
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> to cast the returned object to</typeparam>
        /// <param name="self">The <see cref="IList"/> object to pull objects from</param>
        /// <param name="i">The index to search after</param>
        /// <returns>The item at the index after <paramref name="i"/> in <paramref name="self"/></returns>
        public static T GetNextItem<T>(this IList self, int i)
        {
            return (T)self[self.GetNextIndex(i)];
        }
    }
}
