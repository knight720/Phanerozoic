using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phanerozoic.Core.Helpers
{
    public static class EnumerableHelper
    {
        /// <summary>
        /// List To String
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns></returns>
        public static string EnumerableToString<T>(this IEnumerable<T> enumerable)
        {
            var stringBuilder = new StringBuilder();
            var stringArray = enumerable.Select(i => i.ToString()).ToArray();
            return string.Join(", ", stringArray);
        }
    }
}