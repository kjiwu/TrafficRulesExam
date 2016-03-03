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
    }
}
