using System;
using System.Globalization;

namespace Artificial_Intelligence
{
    static class Functions
    {
        public static bool Settingsopened = false;

        public static float FToC(float C)
        {
            float F = (((C - 32) * 5) / 9);
            return F;
        }
        public static float KelvinToCelcius(double Kelvin)
        {
            float Answer;

            Answer = (float)(Kelvin - 273.15);
            
            return Answer;
        }
        public static float KelvinToFahrenheit(double Kelvin)
        {
            float Answer;

            Answer = (float)(Kelvin * 9 / 5 - 459.67);

            return Answer;
        }
        public static string ToTitleCase(string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }
        public static bool ToBoolean(this string value)
        {
            switch (value.ToLower())
            {
                case "true":
                    return true;
                case "t":
                    return true;
                case "1":
                    return true;
                case "0":
                    return false;
                case "false":
                    return false;
                case "f":
                    return false;
                default:
                    throw new InvalidCastException("You can't cast a weird value to a bool!");
            }
        }
    }
}
