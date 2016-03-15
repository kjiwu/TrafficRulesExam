using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace TrafficRulesExam.Models
{
    [DataContract]
    public class QuestionItem : INotifyPropertyChanged
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

        [DataMember(Name = "sectionId")]
        public int SectionId { get; set; }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "question")]
        public string Question { get; set; }

        [DataMember(Name = "image")]
        public int Image { get; set; }

        [DataMember(Name = "options")]
        public List<string> Options { get; set; }

        [DataMember(Name = "answer")]
        public List<int> Answer { get; set; }

        [DataMember(Name = "explain")]
        public string Explain { get; set; }


        public static string GetExplain(QuestionItem question)
        {
            StringBuilder sb = new StringBuilder();
            foreach (int index in question.Answer)
            {
                sb.Append(question.Options[index - 1] + "\r\n");
            }

            sb.Append("\r\n解释：");

            if (String.IsNullOrEmpty(question.Explain))
            {
                sb.Append("没有解释，记住！");
            }
            else
            {
                sb.Append(question.Explain);
            }

            return sb.ToString();
        }
    }
}
