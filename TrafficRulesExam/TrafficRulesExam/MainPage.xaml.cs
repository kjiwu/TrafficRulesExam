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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TrafficRulesExam
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await HttpHelper.GetQuestionImage(34, async () =>
            {
                var decodeStream = await StorageHelper.GetImageFile("_34.gif");
                SoftwareBitmapSource softBitmap = new SoftwareBitmapSource();
                await softBitmap.SetBitmapAsync(decodeStream);
                imageDecoder.Source = softBitmap;
                imageDecoder.Width = decodeStream.PixelWidth;
                imageDecoder.Height = decodeStream.PixelHeight;
            });

            var stream = await DatabaseHelper.GetQuestionImage(34);
            if (null != stream)
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.SetSource(stream);
                imageOrigin.Source = bitmap;
                imageOrigin.Width = bitmap.PixelWidth;
                imageOrigin.Height = bitmap.PixelHeight;
            }
        }
    }
}
