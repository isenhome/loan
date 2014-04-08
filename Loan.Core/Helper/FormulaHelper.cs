using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loan.Core.Helper
{
    public class FormulaHelper
    {

        public static double CTR(double impression, double click)
        {
            if (impression == 0) return 0;
            return (Double)click / (Double)impression;
        }

        public static string FormatPercentage(double value)
        {
            return String.Format("{0}%", Math.Round(value * 100, 2), 2);
        }

        public static double TAUVPercentage(double TAUV, double UV)
        {
            if (UV == 0) return 0;
            return (Double)TAUV / (Double)UV;
        }

        public static Int32 TGI(double numerator, double denominator)
        {
            if (denominator == 0) return 0;
            return Convert.ToInt32(Math.Round(numerator * 100 / denominator, 0));
        }
        public static Int32 TGI(double numerator)
        {
            return Convert.ToInt32(numerator * 100);
        }

        public static long CalculateUV(long uv, double percentage)
        {
            return Math.Round(uv * percentage).TryLong();
        }
    }
}
