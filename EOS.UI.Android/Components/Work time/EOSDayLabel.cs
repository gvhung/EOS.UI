using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using EOS.UI.Shared.Themes.Helpers;
using EOS.UI.Shared.Themes.Interfaces;
using UIFrameworks.Android.Themes;
using UIFrameworks.Shared.Themes.Helpers;
using UIFrameworks.Shared.Themes.Interfaces;

namespace EOS.UI.Android.Components
{
    internal class EOSDayLabel : TextView, IEOSThemeControl, ISelectable
    {
        #region constructors

        public EOSDayLabel(Context context) : base(context)
        {
            Initialize();
        }

        public EOSDayLabel(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize(attrs);
        }

        public EOSDayLabel(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Initialize(attrs);
        }

        public EOSDayLabel(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Initialize(attrs);
        }

        protected EOSDayLabel(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Initialize();
        }

        #endregion

        #region customization

        private Typeface _titleFont;
        public Typeface TitleFont
        {
            get
            {
                if(_titleFont == null)
                {
                    _titleFont = Typeface.CreateFromAsset(Context.Assets, GetThemeProvider().GetEOSProperty<string>(this, EOSConstants.WorkTimeTitleFont));
                    Typeface = _titleFont;
                }
                return _titleFont;
            }
            set
            {
                _titleFont = value;
                Typeface = value;
                IsEOSCustomizationIgnored = true;
            }
        }

        private int _titleTextSize;
        public int TitleTextSize
        {
            get
            {
                if(_titleTextSize == 0)
                {
                    _titleTextSize = GetThemeProvider().GetEOSProperty<int>(this, EOSConstants.WorkTimeTitleSize);
                    TextSize = _titleTextSize;
                }
                return _titleTextSize;
            }
            set
            {
                _titleTextSize = value;
                TextSize = value;
                IsEOSCustomizationIgnored = true;
            }
        }

        private Color _titleColor;
        public Color TitleColor
        {
            get
            {
                if(_titleColor == Color.Transparent)
                {
                    _titleColor = GetThemeProvider().GetEOSProperty<Color>(this, EOSConstants.NeutralColor2);
                    if(!IsSelected)
                        SetTextColor(_titleColor);
                }
                return _titleColor;
            }
            set
            {
                _titleColor = value;
                IsEOSCustomizationIgnored = true;
                if(!IsSelected)
                    SetTextColor(value);
            }
        }

        private Color _currentDayTextColor;
        public Color CurrentDayTextColor
        {
            get
            {
                if(_currentDayTextColor == Color.Transparent)
                {
                    _currentDayTextColor = GetThemeProvider().GetEOSProperty<Color>(this, EOSConstants.NeutralColor6);
                    if(IsSelected)
                        SetTextColor(_currentDayTextColor);
                }
                return _currentDayTextColor;
            }
            set
            {
                _currentDayTextColor = value;
                IsEOSCustomizationIgnored = true;
                if(IsSelected)
                    SetTextColor(value);
            }
        }

        #endregion

        #region utility method

        private void Initialize(IAttributeSet attrs = null)
        {
            if(attrs != null)
                InitializeAttributes(attrs);
        }

        private void InitializeAttributes(IAttributeSet attrs)
        {
            //TODO: Implement set attrs logic
        }

        #endregion

        #region ISelectable implementation

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                SetTextColor(IsSelected ? CurrentDayTextColor : TitleColor);
            }
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
                TitleFont = Typeface.CreateFromAsset(Context.Assets, GetThemeProvider().GetEOSProperty<string>(this, EOSConstants.WorkTimeTitleFont));
                TitleTextSize = GetThemeProvider().GetEOSProperty<int>(this, EOSConstants.WorkTimeTitleSize);
                TitleColor = GetThemeProvider().GetEOSProperty<Color>(this, EOSConstants.NeutralColor2);
                CurrentDayTextColor = GetThemeProvider().GetEOSProperty<Color>(this, EOSConstants.NeutralColor6);
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
