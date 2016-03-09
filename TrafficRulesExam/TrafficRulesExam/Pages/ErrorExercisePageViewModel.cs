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
    public class ErrorExercisePageViewModel : BaseViewModel
    {
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
        public QuestionItem CurrentQuestion
        {
            get
            {
                return _currentQuestion;
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
        
        public event Action<QuestionItem> QuestionChanged;

        List<int> errorQuestionIds;

        public void LoadQuestion()
        {
            errorQuestionIds = UserDataHelper.GetErrorQuestionIds();
            if (null != errorQuestionIds)
            {
                this.Page = String.Format("{0}/{1}", _currentIndex + 1, errorQuestionIds.Count);
                _currentQuestion = UserDataHelper.GetSubject().Exam.Questions.Where(x => x.Id == errorQuestionIds[0]).FirstOrDefault();
            }
        }

        public void Next()
        {
            _currentIndex++;
            if (_currentIndex < errorQuestionIds.Count)
            {
                int questionId = errorQuestionIds[_currentIndex];
                _currentQuestion = UserDataHelper.GetSubject().Exam.Questions.Where(x => x.Id == questionId).FirstOrDefault();

                if (null != QuestionChanged)
                {
                    QuestionChanged(_currentQuestion);
                }
            }
            else
            {
                _currentIndex = errorQuestionIds.Count - 1;
            }

            this.Page = String.Format("{0}/{1}", _currentIndex + 1, errorQuestionIds.Count);
            UserDataHelper.SaveSubjectExeOrder(_currentIndex);
        }

        public void Perior()
        {
            _currentIndex--;
            if (_currentIndex >= 0)
            {
                int questionId = errorQuestionIds[_currentIndex];
                _currentQuestion = UserDataHelper.GetSubject().Exam.Questions.Where(x => x.Id == questionId).FirstOrDefault();

                if (null != QuestionChanged)
                {
                    QuestionChanged(_currentQuestion);
                }
            }
            else
            {
                _currentIndex = 0;
            }
            this.Page = String.Format("{0}/{1}", _currentIndex + 1, errorQuestionIds.Count);
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
