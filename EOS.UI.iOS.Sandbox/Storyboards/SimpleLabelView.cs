using CoreGraphics;
using EOS.UI.iOS.Controls;
using EOS.UI.iOS.Extensions;
using EOS.UI.iOS.Sandbox.Helpers;
using EOS.UI.iOS.Sandbox.Storyboards;
using EOS.UI.Shared.Themes.Helpers;
using System;
using System.Linq;
using System.Collections.Generic;
using UIFrameworks.Shared.Themes.Helpers;
using UIKit;
using static EOS.UI.iOS.Sandbox.BadgeLabelView;

namespace EOS.UI.iOS.Sandbox
{
    public partial class SimpleLabelView : BaseViewController
    {
        public const string Identifier = "SimpleLabelView";
        private List<UITextField> _textFields;
        private SimpleLabel _simpleLabel;

        public SimpleLabelView (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _simpleLabel = new SimpleLabel
            {
                Text = "Some text"
            };

            _textFields = new List<UITextField>()
            {
                themeField,
                fontField,
                textColorField,
                textSizeField,
                letterSpacingField
            };

            View.AddGestureRecognizer(new UITapGestureRecognizer(() =>
            {
                _textFields.ForEach(field => field.ResignFirstResponder());
            }));

            containerView.ConstrainLayout(() => _simpleLabel.Frame.GetCenterX() == containerView.Frame.GetCenterX() &&
                                                _simpleLabel.Frame.GetCenterY() == containerView.Frame.GetCenterY(), _simpleLabel);

            var frame = new CGRect(0, 0, 100, 150);

            InitThemePicker(frame);
            InitTextSizePicker(frame);
            InitFontPicker(frame);
            InitTextColorPicker(frame);
            InitLetterSpacingPicker(frame);

            resetButton.TouchUpInside += (sender, e) =>
            {
                _simpleLabel.ResetCustomization();
                _textFields.Except(new List<UITextField>() { themeField }).ToList().ForEach(f => f.Text = String.Empty);
            };
        }

        private void InitThemePicker(CGRect frame)
        {
            var themePicker = new UIPickerView(frame)
            {
                ShowSelectionIndicator = true,
                DataSource = new ThemePickerSource()
            };
            var themePickerDelegate = new ThemePickerDelegate();
            themePickerDelegate.DidSelected += (object sender, KeyValuePair<string, EOSThemeEnumeration> e) =>
            {
                themeField.Text = e.Key;
                var provider = _simpleLabel.GetThemeProvider();
                provider.SetCurrentTheme(e.Value);
                _simpleLabel.UpdateAppearance();
                fontField.Text = string.Empty;
                textColorField.Text = string.Empty;
                textSizeField.Text = string.Empty;
            };
            themePicker.Delegate = themePickerDelegate;
            themeField.InputView = themePicker;
            themeField.Text = _simpleLabel.GetThemeProvider().GetCurrentTheme().ThemeValues[EOSConstants.PrimaryColor] == UIColor.White ?
            "Light" : "Dark";
        }

        private void InitTextSizePicker(CGRect frame)
        {
            var textSizePicker = new UIPickerView(frame);
            textSizePicker.ShowSelectionIndicator = true;
            textSizePicker.DataSource = new FontSizesPickerSource();
            var textSizePickerDelegate = new FontSizesPickerDelegate();
            textSizePickerDelegate.DidSelected += (object sender, int e) =>
            {
                _simpleLabel.TextSize = e;
                textSizeField.Text = e.ToString();
            };
            textSizeField.EditingDidBegin += (sender, e) =>
            {
                var size = Constants.FontSizeValues[(int)textSizePicker.SelectedRowInComponent(0)];
                _simpleLabel.TextSize = size;
                textSizeField.Text = size.ToString();
            };
            textSizePicker.Delegate = textSizePickerDelegate;
            textSizeField.InputView = textSizePicker;
        }

        private void InitFontPicker(CGRect frame)
        {
            var fontPicker = new UIPickerView(frame)
            {
                ShowSelectionIndicator = true,
                DataSource = new FontPickerSource()
            };
            var fontPickerDelegate = new FontPickerDelegate();
            fontPickerDelegate.DidSelected += (object sender, UIFont e) =>
            {
                _simpleLabel.Font = e;
                fontField.Text = e.Name;
            };
            fontField.EditingDidBegin += (sender, e) =>
            {
                var font = Constants.Fonts.ElementAt((int)fontPicker.SelectedRowInComponent(0));
                _simpleLabel.Font = font;
                fontField.Text = font.Name;
            };
            fontPicker.Delegate = fontPickerDelegate;
            fontField.InputView = fontPicker;
        }

        private void InitTextColorPicker(CGRect frame)
        {
            var textColorPicker = new UIPickerView(frame)
            {
                ShowSelectionIndicator = true,
                DataSource = new ColorPickerSource()
            };
            var textColorPickerDelegate = new ColorPickerDelegate();
            textColorPickerDelegate.DidSelected += (object sender, KeyValuePair<string, UIColor> e) =>
            {
                _simpleLabel.TextColor = e.Value;
                textColorField.Text = e.Key;
            };
            textColorField.EditingDidBegin += (sender, e) =>
            {
                var colorPair = Constants.Colors.ElementAt((int)textColorPicker.SelectedRowInComponent(0));
                _simpleLabel.TextColor = colorPair.Value;
                textColorField.Text = colorPair.Key;
            };
            textColorPicker.Delegate = textColorPickerDelegate;
            textColorField.InputView = textColorPicker;
        }

        private void InitLetterSpacingPicker(CGRect frame)
        {
            var letterSpacingPicker = new UIPickerView(frame)
            {
                ShowSelectionIndicator = true,
                DataSource = new LetterSpacingPickerSource()
            };
            var letterSpacingPickerDelegate = new LetterSpacingPickerDelegate();
            letterSpacingPickerDelegate.DidSelected += (object sender, int e) =>
            {
                _simpleLabel.LetterSpacing = e;
                letterSpacingField.Text = e.ToString();
            };
            letterSpacingField.EditingDidBegin += (sender, e) =>
            {
                var spacing = Constants.LetterSpacingValues[(int)letterSpacingPicker.SelectedRowInComponent(0)];
                _simpleLabel.LetterSpacing = spacing;
                letterSpacingField.Text = spacing.ToString();
            };
            letterSpacingPicker.Delegate = letterSpacingPickerDelegate;
            letterSpacingField.InputView = letterSpacingPicker;
        }
    }
}