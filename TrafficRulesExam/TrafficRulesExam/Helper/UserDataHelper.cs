using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace TrafficRulesExam.Helper
{
    public class UserDataHelper
    {
        class UserDataKey
        {
            public static string Subject1SequenceIdKey
            {
                get { return "Subject1SequenceIdKey"; }
            } 
        }

        public static void Save(string key, object value)
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values[key] = value;
        }

        public static object GetValue(string key)
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            return localSettings.Values[key];
        }
    }
}
