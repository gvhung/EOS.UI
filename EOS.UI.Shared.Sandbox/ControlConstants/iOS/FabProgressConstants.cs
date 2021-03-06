#if __IOS__
using System;
using System.Collections.Generic;
using EOS.UI.iOS.Themes;
using EOS.UI.Shared.Helpers;
using EOS.UI.Shared.Themes.Extensions;
using EOS.UI.Shared.Themes.Helpers;
using UIKit;
using static EOS.UI.Shared.Sandbox.Helpers.Constants;

namespace EOS.UI.Shared.Sandbox.ControlConstants.iOS
{
    public static class FabProgressConstants
    {
        private static ShadowConfig ShadowConfiguration => (ShadowConfig) EOSThemeProvider.Instance.GetCurrentTheme().ThemeValues[EOSConstants.FabShadow];

        public static Dictionary<string, UIColor> BackgroundColors =>
            Colors.MainColorsCollection.AddDefault(Fields.Default, (UIColor)EOSThemeProvider.Instance.GetCurrentTheme().ThemeValues[EOSConstants.BrandPrimaryColor]);
        public static Dictionary<string, UIColor> PressedBackgroundColors =>
            Colors.MainColorsCollection.AddDefault(Fields.Default, (UIColor)EOSThemeProvider.Instance.GetCurrentTheme().ThemeValues[EOSConstants.BrandPrimaryColorVariant1]);
        public static Dictionary<string, UIColor> DisabledBackgroundColors =>
            Colors.MainColorsCollection.AddDefault(Fields.Default, (UIColor)EOSThemeProvider.Instance.GetCurrentTheme().ThemeValues[EOSConstants.NeutralColor4S]);
        public static Dictionary<string, UIColor> ShadowColors =>
            Colors.MainColorsCollection.AddDefault(Fields.Default, ShadowConfiguration.Color);
        public static Dictionary<string, int> ShadowOffsetXCollection =>
            Shadow.OffsetCollection.AddDefault(Fields.Default, (int)ShadowConfiguration.Offset.X);
        public static Dictionary<string, int> ShadowOffsetYCollection =>
            Shadow.OffsetCollection.AddDefault(Fields.Default, (int)ShadowConfiguration.Offset.Y);
        public static Dictionary<string, int> ShadowRadiusCollection =>
            Shadow.RadiusCollection.AddDefault(Fields.Default, (int)ShadowConfiguration.Blur);
        public static Dictionary<string, double> ShadowOpacityCollection
        {
            get
            {
                nfloat a, r, g, b;
                ShadowConfiguration.Color.GetRGBA(out r, out g, out b, out a);
                return Shadow.OpacityCollection.AddDefault(Fields.Default, a);
            }
        }
    }
}

#endif
