using System;

namespace Phanerozoic.Core.Helpers
{
    public static class StringHelper
    {
        /// <summary>
        /// String to Enum
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static TEnum ToEnum<TEnum>(this string value) where TEnum : struct
        {
            return Enum.Parse<TEnum>(value, true);
        }
    }
}