using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TrafficRulesExam.CustomContols;
using TrafficRulesExam.Helper;
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

        private SubjectStartPageViewModel viewModel;
        public SubjectStartPageViewModel ViewModel
        {
            get
            {
                return viewModel;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            viewModel = new SubjectStartPageViewModel(SubjectId);
            UpdateUI();
        }

        private void UpdateUI()
        {
            switch (SubjectId)
            {
                case 1:
                    tbkTitle.Text = "科目一理论考试";
                    break;
                case 4:
                    tbkTitle.Text = "科目四理论考试";
                    break;
            }

            var errorQuestionIds = UserDataHelper.GetErrorQuestionIds();
            btnErrorQuestions.IsEnabled = null == errorQuestionIds ? false : errorQuestionIds.Count > 0;
        }
    }
}
