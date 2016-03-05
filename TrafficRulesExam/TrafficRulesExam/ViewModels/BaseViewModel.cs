using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TrafficRulesExam.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public async void RaisePropertyChanged<T>(Expression<Func<T>> expression)
        {
            await (Window.Current.Content as Frame).Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                MemberExpression me = expression.Body as MemberExpression;
                if (null != me)
                {
                    var handler = PropertyChanged;
                    if (null != handler)
                    {
                        handler(this, new PropertyChangedEventArgs(me.Member.Name));
                    }
                };
            });
        }

        #endregion
    }
}
