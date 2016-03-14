using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TrafficRulesExam.Helper;
using TrafficRulesExam.Models;
using TrafficRulesExam.ViewModels;

namespace TrafficRulesExam.Pages
{
    public class GetQuestionCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        Action _param;

        public void Execute(object parameter)
        {
            if(null != _param)
            {
                _param();
            }
        }

        public GetQuestionCommand(Action param)
        {
            _param = param;
        }
    }

    public class ExercisePageViewModel : BaseViewModel
    {
        public ExercisePageViewModel()
        {
            _subjectId = UserDataHelper.SubjectId;
            CurrentIndex = UserDataHelper.GetSubjectExeOrder();
        }

        private int _subjectId;

        private int _currentIndex = 0;
        public int CurrentIndex
        {
            get
            {
                return _currentIndex;
            }
            set
            {
                _currentIndex = value;
            }
        }

        private QuestionItem _currentQuestion;

        private List<QuestionItem> _questions;
        public List<QuestionItem> Questions
        {
            get
            {
                return _questions;
            }
            set
            {
                _questions = value;
            }
        }

        private string _page = "";
        public string Page
        {
            get
            {
                return _page;
            }
            set
            {
                _page = value;
                RaisePropertyChanged("Page");          
            }
        }

        public event Action LoadQuestionCompleted;
        public event Action<QuestionItem> QuestionChanged;

        public async void LoadQuestion()
        {
            if (null == UserDataHelper.GetSubject())
            {
                await HttpHelper.GetExam(_subjectId, subject =>
                {
                    Questions = subject.Exam.Questions;
                    this.Page = String.Format("{0}/{1}", _currentIndex + 1, Questions.Count);
                    UserDataHelper.SetSubject(_subjectId, subject);

                    if (null != LoadQuestionCompleted)
                    {
                        LoadQuestionCompleted();
                    }
                });
            }
            else
            {
                Questions = UserDataHelper.GetSubject().Exam.Questions;
                this.Page = String.Format("{0}/{1}", _currentIndex + 1, Questions.Count);

                if (null != LoadQuestionCompleted)
                {
                    LoadQuestionCompleted();
                }
            }
        }

        public void Next()
        {
            _currentIndex++;
            if (_currentIndex < Questions.Count)
            {
                _currentQuestion = Questions[_currentIndex];
                if(null != QuestionChanged)
                {
                    QuestionChanged(_currentQuestion);
                }
            }
            else
            {
                _currentIndex = Questions.Count - 1;
            }

            this.Page = String.Format("{0}/{1}", _currentIndex + 1, Questions.Count);                       
        }

        public void Perior()
        {
            _currentIndex--;
            if (_currentIndex >= 0)
            {
                _currentQuestion = Questions[_currentIndex];
                if (null != QuestionChanged)
                {
                    QuestionChanged(_currentQuestion);
                }
            }
            else
            {
                _currentIndex = 0;
            }
            this.Page = String.Format("{0}/{1}", _currentIndex + 1, Questions.Count);
        }

        public ICommand GetNextCommand
        {
            get
            {
                return new GetQuestionCommand(Next);              
            }
        }

        public ICommand GetPeriorCommand
        {
            get
            {
                return new GetQuestionCommand(Perior);
            }
        }
    }
}
