using System;
using Android.Animation;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Util;
using Com.Airbnb.Lottie;
using EOS.UI.Droid.Themes;
using EOS.UI.Shared.Helpers;
using EOS.UI.Shared.Themes.DataModels;
using EOS.UI.Shared.Themes.Helpers;
using EOS.UI.Shared.Themes.Interfaces;
using Java.Util;

namespace EOS.UI.Droid.Controls
{
    public class SimpleButton : AppCompatButton, IEOSThemeControl
    {
        #region fields

        private const int LottieAnimationSize = 24;
        private float _pivot = 0.5f;
        private LottieDrawable _animationDrawable;
        private const string _animationKey = "Animations/preloader-snake.json";
        private int _baseTopPadding;
        private int _baseBottomPadding;
        private int _baseLeftPadding;
        private int _baseRightPadding;
        private string _text;
        private bool _shouldRedraw = true;

        public bool InProgress { get; private set; }

        #endregion

        #region constructors

        public SimpleButton(Context context) : base(context)
        {
            Initialize();
        }

        public SimpleButton(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize(attrs);
        }

        public SimpleButton(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            Initialize(attrs);
        }

        public SimpleButton(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        #endregion

        #region customization

        public override bool Enabled
        {
            get => base.Enabled;
            set
            {
                if(Enabled != value)
                    UpdateEnabledState(value);
                base.Enabled = value;
            }
        }

        private Color _backgroundColor;
        public Color BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                IsEOSCustomizationIgnored = true;
                _backgroundColor = value;
                if(Enabled)
                    Background = CreateRippleDrawable(BackgroundColor);
            }
        }

        private Color _disabledBackgroundColor;
        public Color DisabledBackgroundColor
        {
            get => _disabledBackgroundColor;
            set
            {
                IsEOSCustomizationIgnored = true;
                _disabledBackgroundColor = value;
                if(!Enabled)
                    Background = CreateGradientDrawable(_disabledBackgroundColor);
            }
        }

        private Color _pressedBackgroundColor;
        public Color PressedBackgroundColor
        {
            get => _pressedBackgroundColor;
            set
            {
                IsEOSCustomizationIgnored = true;
                _pressedBackgroundColor = value;
                if(Enabled)
                    Background = CreateRippleDrawable(BackgroundColor);
            }
        }

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
                    _shouldRedraw = true;
                    FontStyle.Typeface = value;
                    SetFontStyle();
                }
                base.Typeface = value;
            }
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
                    _shouldRedraw = true;
                    FontStyle.LetterSpacing = value;
                    SetFontStyle();
                }
                base.LetterSpacing = value;
            }
        }

        public override float TextSize
        {
            get => base.TextSize;
            set
            {
                IsEOSCustomizationIgnored = true;
                //Should check FontStyle
                //Set method works with base(context) constructor, which works ahead of FontStyle set
                if (FontStyle != null)
                {
                    _shouldRedraw = true;
                    FontStyle.Size = value;
                    SetFontStyle();
                }
                base.TextSize = value;
            }
        }

        public Color TextColor
        {
            get => FontStyle.Color;
            set
            {
                IsEOSCustomizationIgnored = true;
                FontStyle.Color = value;
                SetFontStyle();
                if(Enabled)
                    base.SetTextColor(value);
            }
        }

        public override void SetTextColor(Color color)
        {
            base.SetTextColor(Enabled ? TextColor : DisabledTextColor);
        }

        private Color _disabledTextColor;
        public Color DisabledTextColor
        {
            get => _disabledTextColor;
            set
            {
                IsEOSCustomizationIgnored = true;
                _disabledTextColor = value;
                SetFontStyle();
            }
        }

        private Color _rippleColor;
        public Color RippleColor
        {
            get => _rippleColor;
            set
            {
                _rippleColor = value;
                if (Enabled)
                    Background = CreateRippleDrawable(BackgroundColor);
                IsEOSCustomizationIgnored = true;
            }
        }

        private float _cornerRadius;
        public float CornerRadius
        {
            get => _cornerRadius;
            set
            {
                _cornerRadius = value;
                Background = Enabled ? CreateRippleDrawable(BackgroundColor) : CreateGradientDrawable(DisabledBackgroundColor);
                IsEOSCustomizationIgnored = true;
            }
        }

        private ShadowConfig _shadowConfig;
        public ShadowConfig ShadowConfig
        {
            get => _shadowConfig;
            set
            {
                _shadowConfig = value;
                if (value == null)
                {
                    ResetShadow();
                }
                else
                {
                    SetupShadow(_shadowConfig);
                }
                IsEOSCustomizationIgnored = true;
            }
        }

        private void ResetShadow()
        {
            StateListAnimator = null;
            Elevation = 0;
            TranslationZ = 0;
        }

        private void SetupShadow(ShadowConfig shadow)
        {
            UpdateStateListAnimator(shadow);
        }

        private void UpdateStateListAnimator(ShadowConfig shadow)
        {
            var stateList = new StateListAnimator();

            var elevationHolderToPressed = PropertyValuesHolder.OfFloat("Elevation", shadow.Blur, shadow.Blur/2);
            var translationZHolderToPressed = PropertyValuesHolder.OfFloat("TranslationZ", shadow.Blur, shadow.Blur / 2);
            var pressedAnimation = ObjectAnimator.OfPropertyValuesHolder(this, elevationHolderToPressed, translationZHolderToPressed);
            pressedAnimation.SetDuration(100);

            var elevationHolderToDisabled = PropertyValuesHolder.OfFloat("Elevation", shadow.Blur, 0);
            var translationZHolderToDisabled = PropertyValuesHolder.OfFloat("TranslationZ", shadow.Blur, 0);
            var disabledAnimation = ObjectAnimator.OfPropertyValuesHolder(this, elevationHolderToDisabled, translationZHolderToDisabled);
            disabledAnimation.SetDuration(0);

            var elevationHolderToNormal = PropertyValuesHolder.OfFloat("Elevation", shadow.Blur / 2, shadow.Blur);
            var translationZHolderToNormal = PropertyValuesHolder.OfFloat("TranslationZ", shadow.Blur / 2, shadow.Blur);
            var normalAnimation = ObjectAnimator.OfPropertyValuesHolder(this, elevationHolderToNormal, translationZHolderToNormal);
            normalAnimation.SetDuration(1);

            stateList.AddState(new int[1] { Android.Resource.Attribute.StatePressed }, pressedAnimation);
            stateList.AddState(new int[1] { Android.Resource.Attribute.StateEnabled }, normalAnimation);
            stateList.AddState(new int[1] { -Android.Resource.Attribute.StateEnabled }, disabledAnimation);

            StateListAnimator = stateList;
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
            base.SetTextColor(Enabled ? FontStyle.Color : _disabledTextColor);
            base.LetterSpacing = FontStyle.LetterSpacing;
        }

        #endregion

        #region utility methods

        private void Initialize(IAttributeSet attrs = null)
        {
            _animationDrawable = new LottieDrawable();
            _animationDrawable.Loop(true);

            LottieComposition.Factory.FromAssetFileName(Context, _animationKey, (composition) =>
            {
                _animationDrawable.SetComposition(composition);

                //calculate scale of animation drawable for normalize to 24dp
                if(_animationDrawable.Scale == 1)
                    _animationDrawable.Scale = (LottieAnimationSize * Resources.DisplayMetrics.Density) / _animationDrawable.IntrinsicHeight;
            });

            var denisty = Resources.DisplayMetrics.Density;
            SetAllCaps(false);
            SetLines(1);
            Ellipsize = TextUtils.TruncateAt.End;
            if (attrs != null)
                InitializeAttributes(attrs);

            UpdateAppearance();
            Background = CreateRippleDrawable(BackgroundColor);
        }

        private void SaveCurrentPaddings()
        {
            _baseBottomPadding = PaddingBottom;
            _baseTopPadding = PaddingTop;
            _baseLeftPadding = PaddingLeft;
            _baseRightPadding = PaddingRight;
        }

        private void InitializeAttributes(IAttributeSet attrs)
        {
            var styledAttributes = Context.ObtainStyledAttributes(attrs, Resource.Styleable.SimpleButton, 0, 0);

            var backgroundColor = styledAttributes.GetColor(Resource.Styleable.SimpleButton_eos_backgroundcolor, Color.Transparent);
            if(backgroundColor != Color.Transparent)
                BackgroundColor = backgroundColor;

            var disabledBackgroundColor = styledAttributes.GetColor(Resource.Styleable.SimpleButton_eos_backgroundcolor_disabled, Color.Transparent);
            if(disabledBackgroundColor != Color.Transparent)
                DisabledBackgroundColor = disabledBackgroundColor;

            var pressedBackgroundColor = styledAttributes.GetColor(Resource.Styleable.SimpleButton_eos_backgroundcolor_pressed, Color.Transparent);
            if(pressedBackgroundColor != Color.Transparent)
                PressedBackgroundColor = pressedBackgroundColor;

            var font = styledAttributes.GetString(Resource.Styleable.SimpleButton_eos_font);
            if(!string.IsNullOrEmpty(font))
                Typeface = Typeface.CreateFromAsset(Context.Assets, font);

            var letterSpacing = styledAttributes.GetFloat(Resource.Styleable.SimpleButton_eos_letterspacing, -1);
            if(letterSpacing > 0)
                LetterSpacing = letterSpacing;

            var textColor = styledAttributes.GetColor(Resource.Styleable.SimpleButton_eos_textcolor, Color.Transparent);
            if(textColor != Color.Transparent)
                TextColor = textColor;

            var disabledTextColor = styledAttributes.GetColor(Resource.Styleable.SimpleButton_eos_textcolor_disabled, Color.Transparent);
            if(disabledTextColor != Color.Transparent)
                DisabledTextColor = disabledTextColor;

            var textSize = styledAttributes.GetFloat(Resource.Styleable.SimpleButton_eos_textsize, -1);
            if(textSize > 0)
                TextSize = textSize;

            var cornerRadius = styledAttributes.GetFloat(Resource.Styleable.SimpleButton_eos_cornerradius, -1);
            if(cornerRadius > 0)
                CornerRadius = cornerRadius;
        }

        private Drawable CreateRippleDrawable(Color contentColor)
        {
            return new RippleDrawable(
                CreateRippleColorStateList(),
                CreateGradientDrawable(contentColor),
                CreateRoundedMaskDrawable()
            );
        }

        private ColorStateList CreateRippleColorStateList()
        {
            return new ColorStateList(
                new int[][] { 
                    new int[] { Android.Resource.Attribute.StatePressed }, 
                    new int[] { } 
               },
               new int[]
               {
                   PressedBackgroundColor,
                   RippleColor,
               });
        }

        private GradientDrawable CreateGradientDrawable(Color color)
        {
            var drawable = new GradientDrawable();
            drawable.SetShape(ShapeType.Rectangle);
            drawable.SetColor(color);
            drawable.SetCornerRadius(CornerRadius);
            return drawable;
        }

        private ShapeDrawable CreateRoundedMaskDrawable()
        {
            var outerRadii = new float[8];
            Arrays.Fill(outerRadii, CornerRadius);
            var shapeDrawable = new ShapeDrawable(new RoundRectShape(outerRadii, null, null));
            //shapeDrawable.Paint.Color = PressedBackgroundColor;
            return shapeDrawable;
        }

        private void UpdateEnabledState(bool enabled)
        {
            base.SetTextColor(enabled ? TextColor : DisabledTextColor);
            Background = enabled ? CreateRippleDrawable(BackgroundColor) : CreateGradientDrawable(DisabledBackgroundColor);
        }

        public void StartProgressAnimation()
        {
            if(Enabled && !InProgress)
            {
                SaveCurrentPaddings();
                SetStartAnimationValues();

                if(_shouldRedraw)
                {
                    SetStopAnimationValues();
                    SetStartAnimationValues();
                    _shouldRedraw = false;
                }

                _animationDrawable.PlayAnimation();

                InProgress = true;
            }
        }

        private void SetStartAnimationValues()
        {
            _text = Text;

            //calculate padding around lottie drawable which saved normal button size 
            //after replacing text with lottie drawable
            var paddingX = (int)((Width - _animationDrawable.IntrinsicWidth) / 2f);
            var paddingY = (int)((Height - _animationDrawable.IntrinsicHeight) / 2f);

            Text = string.Empty;
            SetCompoundDrawables(_animationDrawable, null, null, null);
            SetPadding(paddingX, paddingY, paddingX, paddingY);
        }

        private void SetStopAnimationValues()
        {
            SetCompoundDrawables(null, null, null, null);
            SetPadding(_baseLeftPadding, _baseTopPadding, _baseRightPadding, _baseBottomPadding);
            Text = _text;
        }

        public void StopProgressAnimation()
        {
            _animationDrawable.Stop();
            InProgress = false;

            SetStopAnimationValues();
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
                FontStyle = GetThemeProvider().GetEOSProperty<FontStyleItem>(this, EOSConstants.R3C5S);
                DisabledTextColor = GetThemeProvider().GetEOSProperty<FontStyleItem>(this, EOSConstants.R3C4S).Color;
                BackgroundColor = GetThemeProvider().GetEOSProperty<Color>(this, EOSConstants.BrandPrimaryColor);
                DisabledBackgroundColor = GetThemeProvider().GetEOSProperty<Color>(this, EOSConstants.NeutralColor4S);
                PressedBackgroundColor = GetThemeProvider().GetEOSProperty<Color>(this, EOSConstants.BrandPrimaryColorVariant1);
                RippleColor = GetThemeProvider().GetEOSProperty<Color>(this, EOSConstants.BrandPrimaryColorVariant1);
                CornerRadius = GetThemeProvider().GetEOSProperty<float>(this, EOSConstants.ButtonCornerRadius) * Resources.DisplayMetrics.Density;
                ShadowConfig = GetThemeProvider().GetEOSProperty<ShadowConfig>(this, EOSConstants.SimpleButtonShadow);

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
