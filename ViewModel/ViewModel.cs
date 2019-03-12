using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.System.Threading;
using Windows.UI.Core;

namespace ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        private int counter;

        public ViewModel()
        {
            ThreadPoolTimer timer = ThreadPoolTimer.CreatePeriodicTimer(TimerHandler, TimeSpan.FromSeconds(1));


        }

        private async void TimerHandler(ThreadPoolTimer timer)
        {
            var dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
            await dispatcher.RunAsync(
             CoreDispatcherPriority.Normal, () =>
             {
                 // Your UI update code goes here!
                 DateTime datetime = DateTime.Now;
                 TimerTextBlock = datetime.ToString();
             });
            //counter++;
            //TimerTextBlock = counter.ToString();
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private string _TimerTextBlock;

        public string TimerTextBlock
        {
            get { return _TimerTextBlock; }
            set
            {
                _TimerTextBlock = value;
                OnPropertyChanged(nameof(TimerTextBlock));
            }
        }
    }
}
