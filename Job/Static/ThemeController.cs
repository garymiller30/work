using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Job.Static
{
    public static class ThemeController
    {
        public static ThemeEnum Theme { get; set; } = ThemeEnum.Light;

        public static Color Back { get; set; } = Color.White;
        public static Color Fore { get; set; } = Color.Black;
        public static Color HeaderBack { get; set; } = Color.White;
        public static Color HeaderFore { get; set;} = Color.Black;

        public static event EventHandler ThemeChanged = delegate{ };

        public static void SwitchTheme()
        {
            Theme = Theme == ThemeEnum.Light ? ThemeEnum.Dark : ThemeEnum.Light;

            if (Theme == ThemeEnum.Light)
            {
                Back = Color.White;
                Fore = Color.Black;
                HeaderBack = Color.White;
                HeaderFore = Color.Black;
            }
            else
            {
                Back = Color.DimGray;
                Fore = Color.Ivory;
                HeaderBack = Color.Black;
                HeaderFore = Color.DarkGray;
            }

            ThemeChanged(null, EventArgs.Empty);
        }
    }

    public enum ThemeEnum
    {
        Light,
        Dark,
    }

}
