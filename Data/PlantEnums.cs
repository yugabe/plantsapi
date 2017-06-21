using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plants.API.Data
{
    public enum LightRequirements
    {
        Sunny = 1, Half_Shady, Shady
    }
    public enum Month
    {
        Jan = 1, Febr, March, April, May, June, July, Aug, Sept, Oct, Nov, Dec
    }
    public enum WaterRequirements
    {
        Low = 1, Medium, High
    }
    public enum NutritionRequirements
    {
        None = 1, Weekly, Biweekly, Monthly
    }

    [Flags]
    public enum MonthFlags
    {
        None = 0,
        Jan = 1,
        Febr = 2,
        March = 4,
        April = 8,
        May = 16,
        June = 32,
        July = 64,
        Aug = 128,
        Sept = 256,
        Oct = 512,
        Nov = 1024,
        Dec = 2048
    }
}
