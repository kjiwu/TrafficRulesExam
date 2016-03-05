using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TrafficRulesExam.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TrafficRulesExam.Pages
{
    public class NavigateToCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        Action<int> _param;

        public void Execute(object parameter)
        {
            if(null != _param)
            {
                _param(Convert.ToInt32(parameter));
            }
        }

        public NavigateToCommand(Action<int> param)
        {
            _param = param;
        }
    }

    public class SubjectStartPageViewModel : BaseViewModel        
    {
        public SubjectStartPageViewModel(int subjectId)
        {
            this.subjectId = subjectId;
        }

        private int subjectId;


        public ICommand NavigateToCommand
        {
            get
            {
                return new NavigateToCommand(NavigateToHandler);
            }
        }

        private void NavigateToHandler(int tag)
        {
            Frame root = Window.Current.Content as Frame;
            switch (tag)
            {
                case 1:
                    root.Navigate(typeof(ExercisePage), subjectId);
                    break;
                case 2:
                    root.Navigate(typeof(ErrorExercisePage), subjectId);
                    break;
                case 3:
                    root.Navigate(typeof(ExamStartPage), subjectId);
                    break;
            }
        }
    }
}
