using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Util;
using EOS.UI.Droid.Themes;
using EOS.UI.Shared.Themes.DataModels;
using EOS.UI.Shared.Themes.Helpers;
using EOS.UI.Shared.Themes.Interfaces;

namespace EOS.UI.Droid.Controls
{
    public class SimpleLabel : AppCompatTextView, IEOSThemeControl
    {
        #region constructors

        public SimpleLabel(Context context) : base(context)
        {
            Initialize();
        }

        public SimpleLabel(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize(attrs);
        }

        public SimpleLabel(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            Initialize(attrs);
        }

        public SimpleLabel(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        #endregion

        #region customization

        public override Typeface Typeface
        {
            get => base.Typeface;
            set
            {
                //Should check FontStyle
                //Set method works with base(context) constructor, which works ahead of FontStyle set
                if (FontStyle != null)
                {
                    IsEOSCustomizationIgnored = true;
                    FontStyle.Typeface = value;
                    SetFontStyle();
                }
                base.Typeface = value;
            }
        }

        public override void SetTypeface(Typeface tf, [GeneratedEnum] TypefaceStyle style)
        {
            if(FontStyle != null)
                IsEOSCustomizationIgnored = true;

            base.SetTypeface(tf, style);
        }

        public override float LetterSpacing
        {
            get => base.LetterSpacing;
            set
            {
                //Should check FontStyle
                //Set method works with base(context) constructor, which works ahead of FontStyle set
                if (FontStyle != null)
                {
                    IsEOSCustomizationIgnored = true;
                    FontStyle.LetterSpacing = value;
                    SetFontStyle();
                }
                base.LetterSpacing = value;
            }
        }

        public Color TextColor
        {
            get => FontStyle.Color;
            set
            {
                //Should check FontStyle
                //Set method works with base(context) constructor, which works ahead of FontStyle set
                if (FontStyle != null)
                {
                    IsEOSCustomizationIgnored = true;
                    FontStyle.Color = value;
                    SetFontStyle();
                }
                base.SetTextColor(value);
            }
        }

        public override void SetTextColor(Color color)
        {
            TextColor = color;
        }

        public override float TextSize
        {
            get => base.TextSize;
            set
            {
                //Should check FontStyle
                //Set method works with base(context) constructor, which works ahead of FontStyle set
                if (FontStyle != null)
                {
                    IsEOSCustomizationIgnored = true;
                    FontStyle.Size = value;
                    SetFontStyle();
                }
                base.TextSize = value;
            }
        }

        private FontStyleItem _fontStyle;
        public FontStyleItem FontStyle
        {
            get => _fontStyle;
            set
            {
                _fontStyle = value;
                SetFontStyle();
                IsEOSCustomizationIgnored = true;
            }
        }

        private void SetFontStyle()
        {
            base.Typeface = FontStyle.Typeface;
            base.TextSize = FontStyle.Size;
            base.SetTextColor(FontStyle.Color);
            base.LetterSpacing = FontStyle.LetterSpacing;
        }

        #endregion

        #region utility methods

        private void Initialize(IAttributeSet attrs = null)
        {
            SetMaxLines(1);
            Ellipsize = TextUtils.TruncateAt.End;
            Gravity = Android.Views.GravityFlags.Center;
            if(attrs != null)
                InitializeAttributes(attrs);
            UpdateAppearance();
        }

        private void InitializeAttributes(IAttributeSet attrs)
        {
            var styledAttributes = Context.ObtainStyledAttributes(attrs, Resource.Styleable.SimpleButton, 0, 0);

            var font = styledAttributes.GetString(Resource.Styleable.SimpleButton_eos_font);
            if(!string.IsNullOrEmpty(font))
                Typeface = Typeface.CreateFromAsset(Context.Assets, font);

            var letterSpacing = styledAttributes.GetFloat(Resource.Styleable.SimpleButton_eos_letterspacing, -1);
            if(letterSpacing > 0)
                LetterSpacing = letterSpacing;

            var textColor = styledAttributes.GetColor(Resource.Styleable.SimpleButton_eos_textcolor, Color.Transparent);
            if(textColor != Color.Transparent)
                TextColor = textColor;

            var textSize = styledAttributes.GetFloat(Resource.Styleable.SimpleButton_eos_textsize, -1);
            if(textSize > 0)
                TextSize = textSize;
        }

        #endregion

        #region IEOSThemeControl implementation

        public bool IsEOSCustomizationIgnored { get; private set; }

        public IEOSThemeProvider GetThemeProvider()
        {
            return EOSThemeProvider.Instance;
        }

        public void UpdateAppearance()
        {
            if(!IsEOSCustomizationIgnored)
            {
                FontStyle = GetThemeProvider().GetEOSProperty<FontStyleItem>(this, EOSConstants.R2C1S);
                IsEOSCustomizationIgnored = false;
            }
        }

        public void ResetCustomization()
        {
            IsEOSCustomizationIgnored = false;
            UpdateAppearance();
        }

        public IEOSStyle GetCurrentEOSStyle()
        {
            return null;
        }

        public void SetEOSStyle(EOSStyleEnumeration style)
        {

        }

        #endregion
    }
}
