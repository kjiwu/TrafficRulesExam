using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrafficRulesExam.Models
{
    [DataContract]
    public class ExamInfoItem
    {
        [DataMember(Name = "label")]
        public string Label { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }
    }
}
