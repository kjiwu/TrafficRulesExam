using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.Profile;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace TrafficRulesExam.CustomContols
{
    public class BasePage : Page
    {
        public BasePage()
        {
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Frame rootFrame = Window.Current.Content as Frame;
            string deviceFamily = AnalyticsInfo.VersionInfo.DeviceFamily;

            bool needDisplayBackButton = (rootFrame.BackStack.Count > 0) &&
                                         (deviceFamily.Equals("Windows.Desktop"));
            if (needDisplayBackButton)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }

            SystemNavigationManager.GetForCurrentView().BackRequested += BasePage_BackRequested;
        }

        private void BasePage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if((false == e.Handled) && this.Frame.CanGoBack)
            {
                e.Handled = true;
                this.Frame.GoBack();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            string deviceFamily = AnalyticsInfo.VersionInfo.DeviceFamily;
            switch (deviceFamily)
            {
                case "Windows.Desktop":
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                    break;
            }

            SystemNavigationManager.GetForCurrentView().BackRequested -= BasePage_BackRequested;
        }

        protected int _subjectId = -1;
        public int SubjectId
        {
            get { return _subjectId; }
            set
            {
                _subjectId = value;
            }
        }
    }
}
