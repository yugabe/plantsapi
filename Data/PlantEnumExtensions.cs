using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plants.API.Data
{
    public static class PlantEnumExtensions
    {
        public static MonthFlags ToFlags(this IEnumerable<Month> months)
            => (MonthFlags)months?.Distinct().Aggregate(0, (a, m) => (a | (1 << (((int)m) - 1))));

        public static string ToStringFirstLower(this Level level) => level.ToString().ToFirstLower();

        public static string ToStringFirstLower(this Month month) => month.ToString().ToFirstLower();

        public static IEnumerable<Month> ToValues(this MonthFlags monthFlags)
            => Enum.GetValues(typeof(Month)).Cast<Month>().Where(m => ((int)monthFlags & (1 << (((int)m) - 1))) != 0);

        public static string[] ToStringsFirstLower(this MonthFlags monthFlags) => monthFlags.ToValues().Select(ToStringFirstLower).ToArray();

        public static string[] ToStringsFirstLower(this MonthFlags? monthFlags) => monthFlags?.ToStringsFirstLower();

        private static string ToFirstLower(this string text) => text == null
            ? null
            : text.Length == 1
                ? text.ToLower()
                : char.ToLower(text.First()) + text.Substring(1);
    }
}
