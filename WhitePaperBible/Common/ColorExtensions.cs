using System;
namespace WhitePaperBible.Common
{
    public static class ColorExtensions
    {
        public static string ToWebHex(this Xamarin.Forms.Color color)
        {
            var red = (int)(color.R * 255);
            var green = (int)(color.G * 255);
            var blue = (int)(color.B * 255);
            var alpha = (int)(color.A * 255);
            var hex = $"#{red:X2}{green:X2}{blue:X2}";

            return hex;
        }
    }
}
