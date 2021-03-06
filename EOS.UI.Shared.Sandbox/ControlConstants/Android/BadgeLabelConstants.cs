﻿#if __ANDROID__
using System;
using System.Collections.Generic;
using Android.Graphics;
using EOS.UI.Droid.Themes;
using EOS.UI.Shared.Themes.DataModels;
using EOS.UI.Shared.Themes.Extensions;
using EOS.UI.Shared.Themes.Helpers;
using static EOS.UI.Shared.Sandbox.Helpers.Constants;

namespace EOS.UI.Shared.Sandbox.ControlConstants.Android
{
    public static class BadgeLabelConstants
    {
        private static FontStyleItem FontStyle => (FontStyleItem) EOSThemeProvider.Instance.GetCurrentTheme().ThemeValues[EOSConstants.R2C5S];
        
        public static Dictionary<string, Color> BackgroundColors =>
            Colors.MainColorsCollection.AddDefault(Fields.Default, (Color)EOSThemeProvider.Instance.GetCurrentTheme().ThemeValues[EOSConstants.BrandPrimaryColor]);
        public static Dictionary<string, Color> FontColors =>
            Colors.FontColorsCollection.AddDefault(Fields.Default, FontStyle.Color);
        public static Dictionary<string, float> LetterSpacings =>
            Sizes.LetterSpacingCollection.AddDefault(Fields.Default, FontStyle.LetterSpacing);
        public static Dictionary<string, float> TextSizes =>
            Sizes.TextSizeCollection.AddDefault(Fields.Default, FontStyle.Size);
        public static Dictionary<string, int> CornerRadiusCollection =>
            Sizes.CornerRadiusCollection.AddDefault(Fields.Default, (int)((float)EOSThemeProvider.Instance.GetCurrentTheme().ThemeValues[EOSConstants.LabelCornerRadius]));
        public static Dictionary<string, string> BadgeLabelFonts =>
            Fonts.GetButtonBadgeFonts().AddDefault(Fields.Default, "Fonts/Roboto-Bold.ttf");
    }
}
#endif