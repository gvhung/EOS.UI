using System;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using EOS.UI.Droid.Themes;
using EOS.UI.Shared.Themes.DataModels;
using EOS.UI.Shared.Themes.Helpers;
using EOS.UI.Shared.Themes.Interfaces;

namespace EOS.UI.Droid.Controls
{
    public class CircleProgress : LinearLayout, IEOSThemeControl
    {
        private bool _isRunning;
        private ProgressBar _progressBar;
        private TextView _percentText;
        private TextView _percentLabel;
        private ImageView _checkmarkImage;
        private View _centralRectangle;
        private const string _zeroPercents = "0";

        public event EventHandler Started;
        public event EventHandler Stopped;
        public event EventHandler Finished;

        public bool IsEOSCustomizationIgnored { get; private set; }

        private int _progress;
        public int Progress
        {
            get => _progress;
            set
            {
                if(value > 100)
                    return;
                ((Activity)Context).RunOnUiThread(() =>
                {
                    _progress = value;
                    if(_checkmarkImage.Visibility == ViewStates.Visible)
                        _checkmarkImage.Visibility = ViewStates.Invisible;
                    if(ShouldShowProgress)
                    {
                        _percentText.Visibility = ViewStates.Visible;
                        _percentLabel.Visibility = ViewStates.Visible;
                    }
                    _progressBar.Progress = _progress;
                    _percentText.Text = value.ToString();//$"{value}%";
                    if(_progress == 100)
                    {
                        ShowCheckmark();
                    }
                });
            }
        }

        private Color _color;
        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                IsEOSCustomizationIgnored = true;
                ((_progressBar.ProgressDrawable as LayerDrawable).GetDrawable(1) as RotateDrawable).SetColorFilter(_color, PorterDuff.Mode.SrcIn);
                _centralRectangle.SetBackgroundColor(_color);
                _percentText.SetTextColor(_color);
                _percentLabel.SetTextColor(_color);
                FontStyle.Color = value;
            }
        }

        private Color _alternativeColor;
        public Color AlternativeColor
        {
            get => _alternativeColor;
            set
            {
                _alternativeColor = value;
                (_checkmarkImage.Background as GradientDrawable).SetColor(ColorStateList.ValueOf(_alternativeColor));
                IsEOSCustomizationIgnored = true;
            }
        }

        private Color _fillColor;
        public Color FillColor
        {
            get => _fillColor;
            set
            {
                _fillColor = value;
                ((_progressBar.ProgressDrawable as LayerDrawable).GetDrawable(0) as GradientDrawable).SetColor(value);
                IsEOSCustomizationIgnored = true;
            }
        }

        private bool _showProgress;
        public bool ShowProgress
        {
            get => _showProgress;
            set
            {
                _showProgress = value;
                _percentText.Visibility = ShouldShowProgress ? ViewStates.Visible : ViewStates.Invisible;
                _percentLabel.Visibility = ShouldShowProgress ? ViewStates.Visible : ViewStates.Invisible;
                IsEOSCustomizationIgnored = true;
            }
        }

        public Typeface Typeface
        {
            get => _percentText.Typeface;
            set
            {
                _percentText.Typeface = value;
                _percentLabel.Typeface = value;
                FontStyle.Typeface = value;
                SetFontStyle();
                IsEOSCustomizationIgnored = true;
            }
        }

        public float TextSize
        {
            get => _percentText.TextSize;
            set
            {
                _percentText.TextSize = value;
                _percentLabel.TextSize = value;
                FontStyle.Size = value;
                SetFontStyle();
                IsEOSCustomizationIgnored = true;
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
            _percentText.Typeface = FontStyle.Typeface;
            _percentText.TextSize = FontStyle.Size;
            _percentText.SetTextColor(FontStyle.Color);
            _percentText.LetterSpacing = FontStyle.LetterSpacing;

            _percentLabel.Typeface = FontStyle.Typeface;
            _percentLabel.TextSize = FontStyle.Size;
            _percentLabel.SetTextColor(FontStyle.Color);
            _percentLabel.LetterSpacing = FontStyle.LetterSpacing;
        }

        protected CircleProgress(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public CircleProgress(Context context) : base(context)
        {
            Initalize();
        }

        public CircleProgress(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initalize(attrs);
        }

        public CircleProgress(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Initalize(attrs);
        }

        public CircleProgress(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Initalize(attrs);
        }

        private void Initalize(IAttributeSet attrs = null)
        {
            var inflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);
            var view = inflater.Inflate(Resource.Layout.CircleProgress, this);
            _progressBar = view.FindViewById<ProgressBar>(Resource.Id.circularProgressbar);
            _percentText = view.FindViewById<TextView>(Resource.Id.percentText);
            _percentLabel = view.FindViewById<TextView>(Resource.Id.percentLabel);
            _checkmarkImage = view.FindViewById<ImageView>(Resource.Id.checkmark);
            _centralRectangle = view.FindViewById<View>(Resource.Id.centralRectangle);
            _checkmarkImage.Visibility = ViewStates.Invisible;
            _percentText.Text = _zeroPercents;
            Orientation = Android.Widget.Orientation.Vertical;
            SetGravity(GravityFlags.CenterHorizontal);

            if(attrs != null)
                InitializeAttributes(attrs);

            UpdateAppearance();
        }

        private void InitializeAttributes(IAttributeSet attrs)
        {
            var styledAttributes = Context.ObtainStyledAttributes(attrs, Resource.Styleable.CircleProgress, 0, 0);

            var color = styledAttributes.GetColor(Resource.Styleable.CircleProgress_eos_color, Color.Transparent);
            if(color != Color.Transparent)
                Color = color;

            var alernativeColor = styledAttributes.GetColor(Resource.Styleable.CircleProgress_eos_alternativecolor, Color.Transparent);
            if(alernativeColor != Color.Transparent)
                AlternativeColor = alernativeColor;

            var fillColor = styledAttributes.GetColor(Resource.Styleable.CircleProgress_eos_fillcolor, Color.Transparent);
            if(fillColor != Color.Transparent)
                FillColor = fillColor;

            var showProgress = styledAttributes.GetBoolean(Resource.Styleable.CircleProgress_eos_showprogress, true);
            if(!showProgress)
                ShowProgress = showProgress;

            var font = styledAttributes.GetString(Resource.Styleable.CircleProgress_eos_font);
            if(!string.IsNullOrEmpty(font))
                Typeface = Typeface.CreateFromAsset(Context.Assets, font);

            var textSize = styledAttributes.GetFloat(Resource.Styleable.CircleProgress_eos_textsize, -1);
            if(textSize > 0)
                TextSize = textSize;
        }

        public IEOSStyle GetCurrentEOSStyle()
        {
            return null;
        }

        public IEOSThemeProvider GetThemeProvider()
        {
            return EOSThemeProvider.Instance;
        }

        public void ResetCustomization()
        {
            IsEOSCustomizationIgnored = false;
            UpdateAppearance();
        }

        public void SetEOSStyle(EOSStyleEnumeration style)
        {
        }

        public void UpdateAppearance()
        {
            if(!IsEOSCustomizationIgnored)
            {
                var provider = GetThemeProvider();
                FontStyle = GetThemeProvider().GetEOSProperty<FontStyleItem>(this, EOSConstants.R1C1);
                Color = provider.GetEOSProperty<Color>(this, EOSConstants.BrandPrimaryColor);
                AlternativeColor = provider.GetEOSProperty<Color>(this, EOSConstants.BrandPrimaryColor);
                FillColor = provider.GetEOSProperty<Color>(this, EOSConstants.NeutralColor4);
                ShowProgress = provider.GetEOSProperty<bool>(this, EOSConstants.CircleProgressShown);
                IsEOSCustomizationIgnored = false;
            }
        }

        private bool ShouldShowProgress => ShowProgress && _progressBar.Progress != 100;

        private void ShowCheckmark()
        {
            Finished?.Invoke(this, EventArgs.Empty);
            _checkmarkImage.Visibility = ViewStates.Visible;
            _percentText.Visibility = ViewStates.Invisible;
            _percentLabel.Visibility = ViewStates.Invisible;
            _isRunning = false;
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if(e.Action == MotionEventActions.Up || e.Action == MotionEventActions.Cancel)
            {
                if(!_isRunning)
                {
                    Started?.Invoke(this, EventArgs.Empty);
                    _isRunning = true;
                }
                else
                {
                    Stopped?.Invoke(this, EventArgs.Empty);
                    _isRunning = false;
                }
            }
            return true;
        }
    }
}
