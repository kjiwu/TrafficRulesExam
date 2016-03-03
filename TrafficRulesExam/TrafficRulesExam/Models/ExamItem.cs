using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace TrafficRulesExam.Models
{
    [DataContract]
    public class ExamItem : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string name)
        {
            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "exercisetitle")]
        public string ExerciseTitle { get; set; }

        [DataMember(Name = "errorTitle")]
        public string ErrorTitle { get; set; }

        [DataMember(Name = "examId")]
        public string ExamId { get; set; }

        [DataMember(Name = "mockCount")]
        public string MockCount { get; set; }

        [DataMember(Name = "passCount")]
        public string PassCount { get; set; }

        [DataMember(Name = "timeLimit")]
        public string TimeLimit { get; set; }

        [DataMember(Name = "information")]
        public List<ExamInfoItem> Information { get; set; }

        [DataMember(Name = "questions")]
        public List<QuestionItem> Questions { get; set; }
    }
}
