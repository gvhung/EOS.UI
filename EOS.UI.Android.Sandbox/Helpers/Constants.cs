﻿using System.Collections.Generic;
using Android.Graphics;
using EOS.UI.Shared.Themes.Helpers;

namespace EOS.UI.Android.Sandbox.Helpers
{
    public class Constants
    {
        public static class Colors
        {
            public const string Black = "black";
            public const string White = "white";
            public const string Gray = "gray";
            public const string Green = "green";
            public const string Blue = "blue";
            public const string Red = "red";
            public const string Yellow = "yellow";
            public const string Brown = "brown";

            public static Dictionary<string, Color> ColorsCollection = new Dictionary<string, Color>()
            {
                { string.Empty, Color.Transparent },
                { Black, Color.Black },
                { White, Color.White },
                { Gray, Color.Gray },
                { Green, Color.Green },
                { Blue, Color.Blue },
                { Red, Color.Red },
                { Yellow, Color.Yellow },
                { Brown, Color.Brown }
            };
        }

        public static class Fonts
        {
            public const string Roboto = "Roboto";
            public const string Berkshireswash = "Berkshireswash";
            public const string Amita = "Amita";
            public const string AcademyEngraved = "AcademyEngraved";

            public static Dictionary<string, string> FontsCollection = new Dictionary<string, string>()
            {
                { string.Empty, string.Empty },
                { Roboto, "Fonts/Roboto.ttf" },
                { Berkshireswash, "Fonts/Berkshireswash.ttf" },
                { Amita, "Fonts/Amita.ttf" },
                { AcademyEngraved, "Fonts/academyEngraved.ttf" },
            };
        }

        public static class Sizes
        {
            public static Dictionary<string, float> LetterSpacingCollection = new Dictionary<string, float>()
            {
                { string.Empty, 0f },
                { "0.1", 0.1f },
                { "0.2", 0.2f },
                { "0.3", 0.3f },
                { "0.4", 0.4f },
                { "0.5", 0.5f },
                { "0.6", 0.6f },
                { "0.7", 0.7f },
                { "0.8", 0.8f },
                { "0.9", 0.9f },
                { "1", 1f },
            };

            public static Dictionary<string, float> TextSizeCollection = new Dictionary<string, float>()
            {
                { string.Empty, 0f },
                { "10", 10f },
                { "12", 12f },
                { "14", 14f },
                { "16", 16f },
                { "18", 18f },
                { "20", 20f },
                { "22", 22f },
                { "24", 24f },
                { "26", 26f },
                { "28", 28f },
                { "30", 30f },
                { "32", 32f },
                { "34", 34f },
                { "36", 36f },
                { "38", 38f },
                { "40", 40f },
            };

            public static Dictionary<string, float> CornerRadusCollection = new Dictionary<string, float>()
            {
                { string.Empty, 0f },
                { "10", 10f },
                { "14", 14f },
                { "18", 18f },
                { "22", 22f },
                { "26", 26f },
                { "30", 30f },
                { "34", 34f },
                { "38", 38f },
                { "42", 42f },
                { "46", 46f },
                { "50", 50f },
                { "54", 54f },
                { "58", 58f },
                { "62", 62f },
                { "66", 66f },
                { "70", 70f },
            };
        }

        public static class ThemeTypes
        {
            public const string Light = "light";
            public const string Dark = "dark";

            public static Dictionary<string, EOSThemeEnumeration> ThemeCollection = new Dictionary<string, EOSThemeEnumeration>()
            {
                { string.Empty, EOSThemeEnumeration.Light },
                { Light, EOSThemeEnumeration.Light },
                { Dark, EOSThemeEnumeration.Dark },
            };
        }
    }
}