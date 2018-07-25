﻿using Android.Content;
using Android.Support.Animation;
using Android.Views;
using static Android.Support.Animation.DynamicAnimation;

namespace EOS.UI.Droid.Components
{
    internal class OpenSpringAnimationEndListener: View, IOnAnimationEndListener
    {
        private CircleMenu _menu;

        internal OpenSpringAnimationEndListener(Context context, CircleMenu menu) : base(context)
        {
            _menu = menu;
        }

        #region IOnAnimationEndListener implementation

        public void OnAnimationEnd(DynamicAnimation animation, bool canceled, float value, float velocity)
        {
            _menu.HandleOnOpenSpringAnimationEnd();
        }

        #endregion
    }
}
