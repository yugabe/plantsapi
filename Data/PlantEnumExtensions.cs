using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plants.API.Data
{
    public static class PlantEnumExtensions
    {
        public static MonthFlags ToFlags(this IEnumerable<Month> months)
            => (MonthFlags)(months ?? new Month[] { }).Distinct().Aggregate(0, (a, m) => (a | (1 << (((int)m) - 1))));

        public static string ToStringUpper(this LightRequirements lightReq) => lightReq.ToString().ToUpper();
        public static string ToStringUpper(this WaterRequirements waterReq) => waterReq.ToString().ToUpper();
        public static string ToStringUpper(this NutritionRequirements nutrition) => nutrition.ToString().ToUpper();
        public static string ToStringUpper(this Month month) => month.ToString().ToUpper();

        public static IEnumerable<Month> ToValues(this MonthFlags monthFlags)
            => Enum.GetValues(typeof(Month)).Cast<Month>().Where(m => ((int)monthFlags & (1 << (((int)m) - 1))) != 0);

        public static string[] ToStringsUpper(this MonthFlags monthFlags) => monthFlags.ToValues().Select(m => m.ToString().ToUpper()).ToArray();

        public static string[] ToStringsUpper(this MonthFlags? monthFlags) => monthFlags?.ToStringsUpper();
    }
}
