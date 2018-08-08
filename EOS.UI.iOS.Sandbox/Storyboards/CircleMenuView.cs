using EOS.UI.iOS.Components;
using EOS.UI.iOS.Sandbox.Storyboards;
using EOS.UI.Shared.Themes.DataModels;
using EOS.UI.Shared.Themes.Extensions;
using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

namespace EOS.UI.iOS.Sandbox
{
    public partial class CircleMenuView : BaseViewController
    {
        public const string Identifier = "CircleMenuView";
        private List<UIImage> _icons;
        private bool _navigationBarDisabled;
        private UIImage _backgroundImage;

        public CircleMenuView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _backgroundImage = NavigationController.NavigationBar.GetBackgroundImage(UIBarMetrics.Default);
            
            _icons = new List<UIImage>()
            {
                UIImage.FromBundle("icReplay"),
                UIImage.FromBundle("icVideo"),
                UIImage.FromBundle("icShutter"),
                UIImage.FromBundle("icTimer"),
                UIImage.FromBundle("icBrush"),
                UIImage.FromBundle("icDuration"),
                UIImage.FromBundle("icEffects"),
                UIImage.FromBundle("icHeal"),
                UIImage.FromBundle("icMasks"),

                UIImage.FromBundle("icSixteenToNine"),
                UIImage.FromBundle("icOneToOne"),
                UIImage.FromBundle("icHDR"),
            };

            var circleMenu = new CircleMenu(this);
            circleMenu.LeftSwiped += (sender, e) => swipeLabel.Text = "Left swipe";
            circleMenu.RightSwiped += (sender, e) => swipeLabel.Text = "Right swipe";
            circleMenu.Clicked += (object sender, int id) =>
            {
                swipeLabel.Text = $"{id.ToString()}id clicked";
                if (id == -1)
                {
                    ToggleNavigationBar();
                }
                else
                {
                    if (id != 2 && id != 3)
                        ShowItemController(_icons[id]);
                }
            };

            circleMenu.CircleMenuItems = CreateSource();
            circleMenu.Attach();
        }

        private List<CircleMenuItemModel> CreateSource()
        {
            var menuModels = new List<CircleMenuItemModel>();
            for (int i = 0; i < 9; ++i)
            {
                var menuModel = new CircleMenuItemModel(i, _icons[i]);
                if (i == 2 || i == 3)
                {
                    for (int j = 9; j < 12; ++j)
                    {
                        var subMenuModel = new CircleMenuItemModel(j, _icons[j]);
                        menuModel.Children.Add(subMenuModel);
                    }
                }
                menuModels.Add(menuModel);
            }
            return menuModels;
        }

        void ShowItemController(UIImage image)
        {
            var storyboard = UIStoryboard.FromName("CircleMenuItemView", null);
            var viewController = (CircleMenuItemView)storyboard.InstantiateViewController("CircleMenuItemView");
            viewController.NavigationItem.Title = "CircleMenuItemView";
            viewController.MenuItemImage = image;
            NavigationController.PushViewController(viewController, true);
        }

        void ToggleNavigationBar()
        {
            if (_navigationBarDisabled)
            {
                NavigationController.NavigationBar.BackgroundColor = UIColor.White;
                NavigationController.NavigationBar.SetBackgroundImage(_backgroundImage, UIBarMetrics.Default);
                
                NavigationController.NavigationBar.BackgroundColor = UIColor.Clear;
                NavigationController.NavigationBar.UserInteractionEnabled = true;
                NavigationController.NavigationBar.TintColor = ColorExtension.FromHex("3C6DF0");
                NavigationController.InteractivePopGestureRecognizer.Enabled = true;
            }
            else
            {
                NavigationController.NavigationBar.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
                NavigationController.NavigationBar.BackgroundColor = UIColor.Clear;
                
                NavigationController.NavigationBar.UserInteractionEnabled = false;
                NavigationController.NavigationBar.TintColor = UIColor.LightGray;
                NavigationController.InteractivePopGestureRecognizer.Enabled = false;
            }
            _navigationBarDisabled = !_navigationBarDisabled;
        }
    }
}