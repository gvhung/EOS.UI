using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Com.Airbnb.Lottie;
using Com.Airbnb.Lottie.Model;
using Com.Airbnb.Lottie.Value;
using EOS.UI.Droid.Interfaces;

namespace EOS.UI.Droid.Components
{
    /// <summary>
    /// Main menu custom internal control
    /// On click lmplemented scale animation and lottie animation for image 
    /// </summary>
    internal class MainMenuButton: FrameLayout
    {
        #region constants

        private string BlackoutTag = "Blackout";

        #endregion

        #region fields

        private const float StartScale = 1f;
        private const float EndScale = 0.9f;
        private const float PivotScale = 0.5f;
        private const int ScaleDimention = 150;
        private const int LottieViewSideSize = 24;

        private const float ShadowRadiusValue = 6f;

        private const string _forwardAnimationPath = "Animations/menu_animation_forward.json";
        private const string _backAnimationPath = "Animations/menu_animation_back.json";
        private LottieAnimationView _lottieView;

        private IIsOpened _mainControl;

        private ICircleMenuClickable _mainMenuClickable;

        #endregion

        #region properties

        private string AnimationName => _mainControl != null && _mainControl.IsOpened ? _backAnimationPath : _forwardAnimationPath;

        public Color BlackoutColor { get; set; }

        public override bool Enabled
        {
            get => base.Enabled;
            set
            {
                base.Enabled = value;

                var view = FindViewWithTag(BlackoutTag);
                (view.Background as GradientDrawable).SetColor(value ? Color.Transparent : BlackoutColor);
            }
        }

        public Color UnfocusedBackgroundColor
        {
            set => (Background as GradientDrawable).SetColor(value);
        }

        private Color _unfocusedIconColor;
        public Color UnfocusedIconColor
        {
            get => _unfocusedIconColor;
            set
            {
                _unfocusedIconColor = value;
                _lottieView.AddValueCallback(new KeyPath("**"),
                   LottieProperty.ColorFilter,
                   new LottieValueCallback(new SimpleColorFilter(value)));
            }
        }

        #endregion

        #region .ctors

        public MainMenuButton(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Initialize();
        }

        public MainMenuButton(Context context) : base(context)
        {
            Initialize();
        }

        public MainMenuButton(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize(attrs);
        }

        public MainMenuButton(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Initialize(attrs);
        }

        public MainMenuButton(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Initialize(attrs);
        }

        #endregion

        #region utility methods

        private void Initialize(IAttributeSet attrs = null)
        {
            var inflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);
            var view = inflater.Inflate(Resource.Layout.MainMenu, this);

            var roundedDrawable = new GradientDrawable();
            roundedDrawable.SetColor(Color.White);
            roundedDrawable.SetShape(ShapeType.Oval);
            SetBackgroundDrawable(roundedDrawable);
            Elevation = ShadowRadiusValue * Resources.DisplayMetrics.Density;

            SetupLottieView(view);
            _lottieView.SetAnimation(AnimationName);

            AddView(CreateBlackoutView());
        }

        public void AnimateClick()
        {
            if(Enabled)
            {
                _lottieView.SetAnimation(AnimationName);
                SetCustomColorToLottieView();
                _lottieView.PlayAnimation();
            }
        }

        public void SetIIsOpenedItem(IIsOpened isOpened)
        {
            _mainControl = isOpened;
        }

        public void SetICircleMenuClickable(ICircleMenuClickable circleMenuClickable)
        {
            _mainMenuClickable = circleMenuClickable;
        }

        private void StartTouchAnimation(float startScale, float endScale)
        {
            var scaleInAnimation = new ScaleAnimation(startScale, endScale, startScale, endScale, Dimension.RelativeToSelf, PivotScale, Dimension.RelativeToSelf, PivotScale);
            scaleInAnimation.Duration = ScaleDimention;
            scaleInAnimation.FillAfter = true;
            StartAnimation(scaleInAnimation);
        }

        private void SetCustomColorToLottieView()
        {
            _lottieView.AddValueCallback(new KeyPath("**"),
                   LottieProperty.ColorFilter,
                   new LottieValueCallback(new SimpleColorFilter(UnfocusedIconColor)));
        }

        private void SetupLottieView(View view)
        {
            _lottieView = new LottieAnimationView(Context);
            var layoutParameters = new RelativeLayout.LayoutParams((int)(LottieViewSideSize * Resources.DisplayMetrics.Density), (int)(LottieViewSideSize * Resources.DisplayMetrics.Density));
            layoutParameters.AddRule(LayoutRules.CenterInParent);
            _lottieView.LayoutParameters = layoutParameters;
            view.FindViewById<RelativeLayout>(Resource.Id.container).AddView(_lottieView);
        }

        private View CreateBlackoutView()
        {
            var view = new View(Context);
            var layoutParameters = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);
            layoutParameters.AddRule(LayoutRules.CenterInParent);
            view.LayoutParameters = layoutParameters;
            var roundedDrawable = new GradientDrawable();
            roundedDrawable.SetColor(Color.Transparent);
            roundedDrawable.SetShape(ShapeType.Oval);
            view.SetBackgroundDrawable(roundedDrawable);
            view.Tag = BlackoutTag;
            return view;
        }

        #endregion

        #region overrides

        public override bool OnTouchEvent(MotionEvent e)
        {
            if(Enabled && !_mainMenuClickable.Locked)
            {
                if(e.Action == MotionEventActions.Down)
                    StartTouchAnimation(StartScale, EndScale);
                if(e.Action == MotionEventActions.Up || e.Action == MotionEventActions.Cancel)
                    StartTouchAnimation(EndScale, StartScale);
            }
            return base.OnTouchEvent(e);
        }

        #endregion
    }
}
