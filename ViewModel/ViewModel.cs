using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        private string _TimerTextBlock;
        private TimeSpan _Basetime;
        private bool _IsStarted = false;

        public ViewModel()
        {
            ThreadPoolTimer timer = ThreadPoolTimer.CreatePeriodicTimer(TimerHandler, TimeSpan.FromSeconds(1));
            StartClick = new Helper.ActionCommand(StartClickCommand);
            _Basetime = TimeSpan.FromMinutes(25);
        }

        public Helper.ActionCommand StartClick { get; set; }

        public string TimerTextBlock
        {
            get { return _TimerTextBlock; }
            set
            {
                _TimerTextBlock = value;
                OnPropertyChanged(nameof(TimerTextBlock));
            }
        }

        private async void TimerHandler(ThreadPoolTimer timer)
        {
            var dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
            await dispatcher.RunAsync(
             CoreDispatcherPriority.Normal, () =>
             {
                 // Your UI update code goes here!
                 TimerTextBlock = _Basetime.ToString(@"m\:ss");
                 if(_IsStarted)
                    _Basetime -= TimeSpan.FromSeconds(1);
             });
        }

        private void StartClickCommand()
        {
            _IsStarted ^= true;
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
