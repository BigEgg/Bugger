﻿using System;
using System.Collections.Generic;

namespace Bugger.Applications
{
    /// <summary>
    /// Provides helper methods for collections.
    /// </summary>
    public static class CollectionHelper
    {
        /// <summary>
        /// Gets the next element in the collection or default when no next element can be found.
        /// </summary>
        /// <typeparam name="T">The type of the items.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="current">The current item.</param>
        /// <returns>The next element in the collection or default when no next element can be found.</returns>
        /// <exception cref="ArgumentNullException">collection must not be <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The collection does not contain the specified current item.</exception>
        public static T GetNextElementOrDefault<T>(IEnumerable<T> collection, T current)
        {
            if (collection == null) { throw new ArgumentNullException("collection"); }

            bool found = false;
            IEnumerator<T> enumerator = collection.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (EqualityComparer<T>.Default.Equals(enumerator.Current, current))
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                throw new ArgumentException("The collection does not contain the item current.");
            }

            if (enumerator.MoveNext())
            {
                return enumerator.Current;
            }
            else
            {
                return default(T);
            }
        }
    }
}
