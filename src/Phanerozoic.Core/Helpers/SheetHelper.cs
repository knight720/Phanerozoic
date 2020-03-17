using System.Collections.Generic;
using System.Text;

namespace Phanerozoic.Core.Helpers
{
    public static class SheetHelper
    {
        /// <summary>
        /// Index 轉 Sheet Column
        /// https://stackoverflow.com/questions/21229180/convert-column-index-into-corresponding-column-letter
        /// </summary>
        /// <param name="column">index</param>
        /// <returns>Sheet Column</returns>
        public static string ColumnToLetter(int column)
        {
            int temp = 0;
            var letter = new StringBuilder();
            while (column > 0)
            {
                temp = (column - 1) % 26;
                letter.Insert(0, (char)(temp + 65));
                column = (column - temp - 1) / 26;
            }
            return letter.ToString();
        }

        /// <summary>
        /// 單一值轉 Google Sheets Range Values
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IList<IList<object>> ObjectToValues(object obj)
        {
            IList<IList<object>> values = new List<IList<object>>
            {
                new List<object>
                {
                    obj
                }
            };

            return values;
        }

        /// <summary>
        /// Sheet Cell Object To Integer
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ObjectToInt(object obj)
        {
            if (obj != null)
            {
                var str = obj.ToString();
                int value;
                return int.TryParse(str, out value) ? value : default;
            }
            return default;
        }

        public static decimal ObjectToDecimal(this object obj)
        {
            if (obj != null)
            {
                var str = obj.ToString();
                decimal value;
                return decimal.TryParse(str, out value) ? value : default;
            }
            return default;
        }

        public static decimal? ObjectToNullableDecimal(this object obj)
        {
            if (obj != null)
            {
                var str = obj.ToString();
                decimal value;
                return decimal.TryParse(str, out value) ? value : default(decimal?);
            }
            return default;
        }
    }
}