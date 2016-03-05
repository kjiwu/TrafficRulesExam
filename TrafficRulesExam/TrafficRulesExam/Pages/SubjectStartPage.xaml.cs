using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TrafficRulesExam.CustomContols;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TrafficRulesExam.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SubjectStartPage : BasePage
    {
        public SubjectStartPage()
        {
            this.InitializeComponent();
        }

        private int subjectId;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            subjectId = Convert.ToInt32(e.Parameter);
            UpdateUI();
        }

        private void UpdateUI()
        {
            switch (subjectId)
            {
                case 1:
                    tbkTitle.Text = "科目一理论考试";
                    break;
                case 4:
                    tbkTitle.Text = "科目四理论考试";
                    break;
            }
            
        }
    }
}
