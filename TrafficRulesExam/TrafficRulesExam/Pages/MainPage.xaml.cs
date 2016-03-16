using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using TrafficRulesExam.Helper;
using System.Diagnostics;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using System.Threading.Tasks;
using TrafficRulesExam.Models;
using TrafficRulesExam.CustomContols;
using Windows.UI;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TrafficRulesExam
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : BasePage
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        public MainPageViewModel ViewModel
        {
            get
            {
                return new MainPageViewModel();
            }
        }

        private async void SymbolIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog()
            {
                Content = "这个是驾考宝典的第一版",
                Title = "驾考宝典",
                Background = new SolidColorBrush(Colors.Cornsilk),
                PrimaryButtonText = "确定",
                FullSizeDesired = false,
            };
            await dialog.ShowAsync();
        }
    }
}
