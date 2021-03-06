using System;
using System.Linq;
using CoreAnimation;
using CoreGraphics;
using EOS.UI.iOS.Extensions;
using EOS.UI.iOS.Themes;
using EOS.UI.Shared.Themes.DataModels;
using EOS.UI.Shared.Themes.Helpers;
using EOS.UI.Shared.Themes.Interfaces;
using Foundation;
using UIKit;
using static EOS.UI.iOS.Helpers.Constants;

namespace EOS.UI.iOS.Components
{
    public partial class Section : UITableViewHeaderFooterView, IEOSThemeControl
    {
        #region fields

        private bool _subscribed;
        private CALayer _underlineLayer;

        public static readonly NSString Key = new NSString("Section");
        public static readonly UINib Nib;

        #endregion

        #region constructors

        static Section()
        {
            Nib = UINib.FromName("Section", NSBundle.MainBundle);
        }

        public Section(IntPtr handle) : base(handle)
        {

        }

        #endregion

        #region customization

        public Action SectionAction { get; set; }

        private FontStyleItem _titleFontStyle;
        public FontStyleItem TitleFontStyle
        {
            get => _titleFontStyle;
            set
            {
                _titleFontStyle = value;
                SetTitleFontStyle();
                IsEOSCustomizationIgnored = true;
            }
        }

        private FontStyleItem _buttonFontStyle;
        public FontStyleItem ButtonFontStyle
        {
            get => _buttonFontStyle;
            set
            {
                _buttonFontStyle = value;
                SetButtonFontStyle();
                IsEOSCustomizationIgnored = true;
            }
        }

        private string _sectionTitleText;
        public string SectionTitleText
        {
            get => _sectionTitleText;
            set
            {
                _sectionTitleText = value;
                var attributedString = new NSAttributedString(value ?? string.Empty);
                sectionName.AttributedText = attributedString;
                SetTitleFontStyle();
                IsEOSCustomizationIgnored = true;
            }
        }

        private string _buttonText;
        public string ButtonText
        {
            get => _buttonText;
            set
            {
                _buttonText = value;
                sectionButton.SetAttributedTitle(new NSAttributedString(value ?? string.Empty), UIControlState.Normal);
                SetButtonFontStyle();
                IsEOSCustomizationIgnored = true;
            }
        }

        public float SectionTextSize
        {
            get => TitleFontStyle.Size;
            set
            {
                TitleFontStyle.Size = value;
                TitleFontStyle.Font = TitleFontStyle.Font.WithSize(value);
                SetTitleFontStyle();
                IsEOSCustomizationIgnored = true;
            }
        }

        public float ButtonTextSize
        {
            get => ButtonFontStyle.Size;
            set
            {
                ButtonFontStyle.Size = value;
                ButtonFontStyle.Font = ButtonFontStyle.Font.WithSize(value);
                SetButtonFontStyle();
                IsEOSCustomizationIgnored = true;
            }
        }

        public float SectionTextLetterSpacing
        {
            get => TitleFontStyle.LetterSpacing;
            set
            {
                TitleFontStyle.LetterSpacing = value;
                SetTitleFontStyle();
                IsEOSCustomizationIgnored = true;
            }
        }

        public float ButtonTextLetterSpacing
        {
            get => ButtonFontStyle.LetterSpacing;
            set
            {
                ButtonFontStyle.LetterSpacing = value;
                SetButtonFontStyle();
                IsEOSCustomizationIgnored = true;
            }
        }

        public UIFont SectionTextFont
        {
            get => TitleFontStyle.Font;
            set
            {
                TitleFontStyle.Font = value.WithSize(SectionTextSize);
                SetTitleFontStyle();
                IsEOSCustomizationIgnored = true;
            }
        }

        public UIFont ButtonTextFont
        {
            get => ButtonFontStyle.Font;
            set
            {
                ButtonFontStyle.Font = value.WithSize(ButtonTextSize);
                SetButtonFontStyle();
                IsEOSCustomizationIgnored = true;
            }
        }

        public UIColor SectionTextColor
        {
            get => TitleFontStyle.Color;
            set
            {
                TitleFontStyle.Color = value;
                SetTitleFontStyle();
                IsEOSCustomizationIgnored = true;
            }
        }

        public UIColor ButtonTextColor
        {
            get => ButtonFontStyle.Color;
            set
            {
                ButtonFontStyle.Color = value;
                SetButtonFontStyle();
                IsEOSCustomizationIgnored = true;
            }
        }

        private bool _hasBorder;
        public bool HasBorder
        {
            get => _hasBorder;
            set
            {
                _hasBorder = value;
                ToggleBorderVisibility();
            }
        }

        public new UIColor BackgroundColor
        {
            get => UIColor.FromCGColor(Layer.BackgroundColor);
            set
            {
                Layer.BackgroundColor = value?.CGColor;
                IsEOSCustomizationIgnored = true;
            }
        }

        private int _borderWidth;
        public int BorderWidth
        {
            get => _borderWidth;
            set
            {
                _borderWidth = value;
                ToggleBorderVisibility();
                IsEOSCustomizationIgnored = true;
            }
        }

        private UIColor _borderColor;
        public UIColor BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                ToggleBorderVisibility();
                IsEOSCustomizationIgnored = true;
            }
        }

        public void SetPaddings(int left, int top, int right, int bottom)
        {
            paddingTop.Constant = top;
            paddingBottom.Constant = bottom;
            paddingLeft.Constant = left;
            paddingRight.Constant = right;
            IsEOSCustomizationIgnored = true;
        }

        public bool HasButton
        {
            get => sectionButton.Hidden;
            set
            {
                sectionButton.Hidden = !value;
                IsEOSCustomizationIgnored = true;
            }
        }

        #endregion

        #region utility methods

        public void Initialize()
        {
            if (sectionButton != null)
            {
                sectionButton.SetAttributedTitle(new NSAttributedString(ButtonText ?? string.Empty), UIControlState.Normal);
                sectionName.AttributedText = new NSAttributedString(SectionTitleText ?? string.Empty);
                sectionButton.LineBreakMode = UILineBreakMode.TailTruncation;

                if (ButtonFontStyle != null)
                    SetButtonFontStyle();
                if (TitleFontStyle != null)
                    SetTitleFontStyle();

                if (!_subscribed)
                {
                    sectionButton.TouchDown += delegate
                    {
                        SectionAction?.Invoke();
                    };
                    _subscribed = true;
                }
            }
            UpdateAppearance();
        }

        private void SetButtonTextColor(UIColor color)
        {
            if (color != null)
            {
                var attrString = new NSMutableAttributedString(sectionButton.GetAttributedTitle(UIControlState.Normal));
                var range = new NSRange(0, attrString.Length);
                attrString.AddAttribute(UIStringAttributeKey.ForegroundColor, color, range);
                sectionButton.SetAttributedTitle(attrString, UIControlState.Normal);
            }
        }

        private void ToggleBorderVisibility()
        {
            UpdateDivider(HasBorder);
            Layer.MasksToBounds = true;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (_underlineLayer == null)
            {
                _underlineLayer = new CALayer
                {
                    BackgroundColor = BorderColor == null ? UIColor.Clear.CGColor : BorderColor.CGColor,
                    Frame = new CGRect(0, 0, Frame.Size.Width, BorderWidth),
                    Name = InputConstants.BorderName
                };
                Layer.AddSublayer(_underlineLayer);
                Layer.MasksToBounds = true;
            }

            UpdateDivider(HasBorder);
        }

        private void UpdateDivider(bool isVisible)
        {
            var underlineLayer = Layer.Sublayers.FirstOrDefault(item => item.Name == InputConstants.BorderName);
            if (underlineLayer != null)
            {
                underlineLayer.BackgroundColor = BorderColor == null || !isVisible ? UIColor.Clear.CGColor : BorderColor.CGColor;
                underlineLayer.Frame = new CGRect(0, 0, Frame.Size.Width, isVisible ? BorderWidth : 0);
            }
        }

        #endregion

        #region IEOSThemeControl implementation

        public bool IsEOSCustomizationIgnored { get; set; }

        public IEOSThemeProvider GetThemeProvider()
        {
            return EOSThemeProvider.Instance;
        }

        public void UpdateAppearance()
        {
            if (!IsEOSCustomizationIgnored)
            {
                var provider = GetThemeProvider();
                HasBorder = provider.GetEOSProperty<bool>(this, EOSConstants.HasSectionBorder);
                HasButton = provider.GetEOSProperty<bool>(this, EOSConstants.HasSectionAction);
                SectionTitleText = provider.GetEOSProperty<string>(this, EOSConstants.SectionTitle);
                ButtonText = provider.GetEOSProperty<string>(this, EOSConstants.SectionActionTitle);
                TitleFontStyle = provider.GetEOSProperty<FontStyleItem>(this, EOSConstants.R2C3);
                ButtonFontStyle = provider.GetEOSProperty<FontStyleItem>(this, EOSConstants.R2C1S);
                BackgroundColor = provider.GetEOSProperty<UIColor>(this, EOSConstants.NeutralColor5);
                BorderColor = provider.GetEOSProperty<UIColor>(this, EOSConstants.NeutralColor4);
                BorderWidth = provider.GetEOSProperty<int>(this, EOSConstants.BorderWidth);
                var leftPadding = provider.GetEOSProperty<int>(this, EOSConstants.LeftPadding);
                var topPadding = provider.GetEOSProperty<int>(this, EOSConstants.TopPadding);
                var rightPadding = provider.GetEOSProperty<int>(this, EOSConstants.RightPadding);
                var bottomPadding = provider.GetEOSProperty<int>(this, EOSConstants.BottomPadding);
                SetPaddings(leftPadding, topPadding, rightPadding, bottomPadding);
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

        private void SetTitleFontStyle()
        {
            if (TitleFontStyle == null)
                return;
            sectionName.SetTextSize(SectionTextSize);
            sectionName.SetLetterSpacing(SectionTextLetterSpacing);
            sectionName.Font = SectionTextFont;
            sectionName.TextColor = SectionTextColor;
        }

        private void SetButtonFontStyle()
        {
            if (ButtonFontStyle == null)
                return;
            sectionButton.SetTextSize(ButtonTextSize);
            sectionButton.SetLetterSpacing(ButtonTextLetterSpacing);
            sectionButton.SetFont(ButtonTextFont);
            SetButtonTextColor(ButtonTextColor);
        }

        #endregion
    }
}
