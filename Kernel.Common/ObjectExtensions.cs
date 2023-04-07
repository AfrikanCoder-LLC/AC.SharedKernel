using NSpecifications;


namespace Kernel.Common
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Determines whether an object of type T is contained within the given list.
        /// </summary>
        /// <param name="input">The object.</param>
        /// <param name="list">The list.</param>
        /// <returns>The answer</returns>
        public static bool In<T>(this T input, IEnumerable<T> list)
        {
            return list.Contains(input);
        }

        /// <summary>
        /// Determines whether an object of type T is contained within the given list.
        /// </summary>
        /// <param name="input">The object.</param>
        /// <param name="list">The list.</param>
        /// <returns>The answer</returns>
        public static bool In<T>(this T input, params T[] list)
        {
            List<T> l = list.ToList();
            return l.Contains(input);
        }

        /// <summary>
        /// Determines whether an object of type T is contained within the given list.
        /// </summary>
        /// <param name="input">The object.</param>
        /// <param name="actionableCoReSpec"></param>
        /// <param name="list">The list.</param>
        /// <returns>The answer</returns>
        public static bool In<T>(this T input, ASpec<T> actionableCoReSpec, params T[] list)
        {
            List<T> l = list.ToList();
            return l.Contains(input);
        }
        /// <summary>
        /// Determines whether an object of type T is NOT contained within the given list.
        /// </summary>
        /// <param name="input">The object.</param>
        /// <param name="list">The list.</param>
        /// <returns>The answer</returns>
        public static bool NotIn<T>(this T input, IEnumerable<T> list)
        {
            return !list.Contains(input);
        }

        /// <summary>
        /// Determines whether an object of type T is NOT contained within the given list.
        /// </summary>
        /// <param name="input">The object.</param>
        /// <param name="list">The list.</param>
        /// <returns>The answer</returns>
        public static bool NotIn<T>(this T input, params T[] list)
        {
            List<T> l = list.ToList();
            return !l.Contains(input);
        }

        /// <summary>
        /// Check to determine if an object is null
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>The answer</returns>        
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        /// <summary>
        /// Check to determine if an object is not null
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>The answer</returns>        
        public static bool IsNotNull(this object obj)
        {
            return !IsNull(obj);
        }
    }
}
