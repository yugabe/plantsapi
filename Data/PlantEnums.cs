using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plants.API.Data
{
    public enum Level
    {
        VeryLow = 1, Low, Medium, High, VeryHigh
    }
    public enum Month
    {
        January = 1, February, March, April, May, June, July, August, September, October, November, December
    }
    [Flags]
    public enum MonthFlags
    {
        None = 0,
        January = 1,
        February = 2,
        March = 4, 
        April = 8,
        May = 16,
        June = 32,
        July = 64,
        August = 128,
        September = 256,
        October = 512,
        November = 1024,
        December = 2048
    }
}
