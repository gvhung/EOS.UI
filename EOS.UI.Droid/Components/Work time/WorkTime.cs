using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using EOS.UI.Droid.Themes;
using EOS.UI.Shared.Themes.DataModels;
using EOS.UI.Shared.Themes.Enums;
using EOS.UI.Shared.Themes.Helpers;
using EOS.UI.Shared.Themes.Interfaces;
using static EOS.UI.Droid.Helpers.Constants;

namespace EOS.UI.Droid.Components
{
    public class WorkTime : RecyclerView, IEOSThemeControl
    {
        #region fields

        private int _sectionWidth;

        #endregion

        #region properties

        private WorkTimeAdapter WorkTimeAdapter => GetAdapter() as WorkTimeAdapter;

        #endregion

        #region constructors

        public WorkTime(Context context) : base(context)
        {
            Initialize();
        }

        public WorkTime(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize(attrs);
        }

        public WorkTime(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            Initialize(attrs);
        }

        protected WorkTime(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        #endregion

        #region customization

        public WeekStartEnum WeekStart
        {
            get => WorkTimeAdapter.WeekStart;
            set => WorkTimeAdapter.WeekStart = value;
        }

        public Typeface TitleFont
        {
            get => WorkTimeAdapter.TitleFont;
            set => WorkTimeAdapter.TitleFont = value;
        }

        public Typeface DayTextFont
        {
            get => WorkTimeAdapter.DayTextFont;
            set => WorkTimeAdapter.DayTextFont = value;
        }

        public int TitleTextSize
        {
            get => WorkTimeAdapter.TitleTextSize;
            set => WorkTimeAdapter.TitleTextSize = value;
        }

        public int DayTextSize
        {
            get => WorkTimeAdapter.DayTextSize;
            set => WorkTimeAdapter.DayTextSize = value;
        }

        public Color TitleColor
        {
            get => WorkTimeAdapter.TitleColor;
            set => WorkTimeAdapter.TitleColor = value;
        }

        public Color DayTextColor
        {
            get => WorkTimeAdapter.DayTextColor;
            set => WorkTimeAdapter.DayTextColor = value;
        }

        public Color CurrentDayBackgroundColor
        {
            get => WorkTimeAdapter.CurrentDayBackgroundColor;
            set => WorkTimeAdapter.CurrentDayBackgroundColor = value;
        }

        public Color CurrentDayTextColor
        {
            get => WorkTimeAdapter.CurrentDayTextColor;
            set => WorkTimeAdapter.CurrentDayTextColor = value;
        }

        public Color DividerColor
        {
            get => WorkTimeAdapter.DividerColor;
            set => WorkTimeAdapter.DividerColor = value;
        }

        public Color CurrentDividerColor
        {
            get => WorkTimeAdapter.CurrentDividerColor;
            set => WorkTimeAdapter.CurrentDividerColor = value;
        }

        public Color DayEvenBackgroundColor
        {
            get => WorkTimeAdapter.DayEvenBackgroundColor;
            set => WorkTimeAdapter.DayEvenBackgroundColor = value;
        }

        public List<WorkTimeCalendarItem> Items
        {
            get => WorkTimeAdapter.Items;
            set => WorkTimeAdapter.Items = value;
        }

        #endregion

        #region utility method

        private void Initialize(IAttributeSet attrs = null)
        {
            var padding = (int)(WorkTimeConstants.Padding * Resources.DisplayMetrics.Density);
            SetPadding(padding, padding, padding, padding);
            SetLayoutManager(new LinearLayoutManager(Context, LinearLayoutManager.Horizontal, false));

            _sectionWidth = (Resources.DisplayMetrics.WidthPixels - 2 * padding) / WorkTimeConstants.DaysCount;

            SetAdapter(new WorkTimeAdapter(Context, _sectionWidth));

            if(attrs != null)
                InitializeAttributes(attrs);

            UpdateAppearance();
        }

        private void InitializeAttributes(IAttributeSet attrs)
        {
            //TODO: Implement set attrs logic
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
            WorkTimeAdapter.UpdateAppearance();
        }

        public void ResetCustomization()
        {

            WorkTimeAdapter.ResetCustomization();
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
