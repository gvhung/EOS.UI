using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using EOS.UI.iOS.Controls;
using EOS.UI.iOS.Extensions;
using EOS.UI.iOS.Sandbox.Helpers;
using EOS.UI.iOS.Sandbox.Storyboards;
using EOS.UI.Shared.Themes.Themes;
using UIKit;
using static EOS.UI.iOS.Sandbox.Helpers.Constants;

namespace EOS.UI.iOS.Sandbox
{
    public partial class GhostButtonView : BaseViewController
    {
        public const string Identifier = "GhostButtonView";
        private List<EOSSandboxDropDown> _dropDowns;

        public GhostButtonView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var ghostButton = new GhostButton();
            ghostButton.SetTitle("DEFAULT TEXT", UIControlState.Normal);
            containerView.ConstrainLayout(() => ghostButton.Frame.GetCenterX() == containerView.Frame.GetCenterX() &&
                                          ghostButton.Frame.GetCenterY() == containerView.Frame.GetCenterY() &&
                                          ghostButton.Frame.Left == containerView.Frame.Left &&
                                          ghostButton.Frame.Right == containerView.Frame.Right, ghostButton);

            _dropDowns = new List<EOSSandboxDropDown>()
            {
                themeDropDown,
                fontDropDown,
                letterSpacingDropDown,
                enabledTextColorDropDown,
                disabledTextColorDropDown,
                pressedTextColorDropDown,
                textSizeDropDown,
            };

            View.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                _dropDowns.ForEach(dropDown => dropDown.CloseInputControl());
            }));

            var rect = new CGRect(0, 0, 100, 100);

            themeDropDown.InitSource(
                Constants.Themes,
                (theme) =>
                {
                    ghostButton.GetThemeProvider().SetCurrentTheme(theme);
                    ghostButton.ResetCustomization();
                    _dropDowns.Except(new[] { themeDropDown }).ToList().ForEach(dropDown => dropDown.ResetValue());
                    UpdateApperaence();
                },
                Fields.Theme,
                rect);
            themeDropDown.SetTextFieldText(ghostButton.GetThemeProvider().GetCurrentTheme() is LightEOSTheme  ? "Light" : "Dark");

            fontDropDown.InitSource(
                Fonts,
                font => ghostButton.Font = font,
                Fields.Font,
                rect);

            letterSpacingDropDown.InitSource(
                LetterSpacingValues,
                spacing => ghostButton.LetterSpacing = spacing,
                Fields.LetterSpacing,
                rect);

            enabledTextColorDropDown.InitSource(
                color => ghostButton.EnabledTextColor = color,
                Fields.EnabledTextColor,
                rect);

            disabledTextColorDropDown.InitSource(
                color => ghostButton.DisabledTextColor = color,
                Fields.DisabledTextColor,
                rect);

            pressedTextColorDropDown.InitSource(
                color => ghostButton.PressedStateTextColor = color,
                Fields.PressedTextColor,
                rect);

            textSizeDropDown.InitSource(
                FontSizeValues,
                size => ghostButton.TextSize = size,
                Fields.TextSize,
                rect);

            stateSwitch.ValueChanged += (sender, e) =>
            {
                ghostButton.Enabled = stateSwitch.On;
            };

            resetButton.TouchUpInside += (sender, e) =>
            {
                _dropDowns.Except(new [] { themeDropDown }).ToList().ForEach(dropDown => dropDown.ResetValue());
                ghostButton.ResetCustomization();
            };
        }
    }
}