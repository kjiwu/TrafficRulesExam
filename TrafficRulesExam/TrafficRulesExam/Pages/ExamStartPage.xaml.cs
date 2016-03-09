using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TrafficRulesExam.CustomContols;
using TrafficRulesExam.Helper;
using TrafficRulesExam.Models;
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
    public sealed partial class ExamStartPage : BasePage
    {
        public ExamStartPage()
        {
            this.InitializeComponent();

            UpdateUI();
        }

        private void UpdateUI()
        {
            ExamItem exam = UserDataHelper.GetSubject().Exam;
            if(null != exam)
            {
                tbkCarType.Text = exam.Information[0].Description;
                tbkQuestionCount.Text = exam.Information[1].Description;
                tbkExamTime.Text = exam.Information[2].Description;
                tbkPass.Text = exam.Information[3].Description;
            }
        }
    }
}
