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
        private string _timerTextBlock;
        private TimeSpan _longTimer;
        private TimeSpan _shortTimer;
        private int _longTimerTimeSpanInMinutes = 2;
        private int _shortTimerTimeSpanInMinutes = 1;
        private bool _isStarted = false;

        public ViewModel()
        {
            ThreadPoolTimer timer = ThreadPoolTimer.CreatePeriodicTimer(TimerHandler, TimeSpan.FromSeconds(1));
            StartClick = new Helper.ActionCommand(StartClickCommand);
            _longTimer = TimeSpan.FromMinutes(_longTimerTimeSpanInMinutes);
            _shortTimer = TimeSpan.FromMinutes(_shortTimerTimeSpanInMinutes);
        }

        public Helper.ActionCommand StartClick { get; set; }

        public string TimerTextBlock
        {
            get { return _timerTextBlock; }
            set
            {
                _timerTextBlock = value;
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
                 TimerTextBlock = _longTimer.ToString(@"m\:ss");
                 if (_isStarted)
                 {
                     _longTimer -= TimeSpan.FromSeconds(1);
                     if (_longTimer <= TimeSpan.Zero)
                     {
                         TimerTextBlock = _shortTimer.ToString(@"m\:ss");
                         _shortTimer -= TimeSpan.FromSeconds(1);
                         if (_shortTimer <= TimeSpan.Zero)
                         {
                             _longTimer = TimeSpan.FromMinutes(_longTimerTimeSpanInMinutes);
                             _shortTimer = TimeSpan.FromMinutes(_shortTimerTimeSpanInMinutes);
                         }
                     }
                 }
             });
        }

        private void StartClickCommand()
        {
            _longTimer = TimeSpan.FromMinutes(_longTimerTimeSpanInMinutes);
            _isStarted ^= true;
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
