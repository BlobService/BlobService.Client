using System;

namespace BlobService.Client.Util
{
    internal static class Utility
    {
        /// <summary>
        /// Throw an exception if the value is null.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <exception cref="ArgumentNullException">Thrown if value is null.</exception>
        internal static void AssertNotNull(string paramName, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        /// <summary>
        /// Throws an exception if the string is empty or <c>null</c>.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <exception cref="ArgumentException">Thrown if value is empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown if value is null.</exception>
        internal static void AssertNotNullOrEmpty(string paramName, string value)
        {
            AssertNotNull(paramName, value);

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException(Consts.ArgumentEmptyError, paramName);
            }
        }
    }
}