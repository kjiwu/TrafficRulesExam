using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrafficRulesExam.Models
{
    [DataContract]
    public class Subject
    {
        [DataMember(Name = "exam")]
        public ExamItem Exam { get; set; }
    }
}
