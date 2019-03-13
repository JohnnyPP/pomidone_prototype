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
        private int _workTimerTimeSpanInMinutes = 3;
        private int _shortBreakTimerTimeSpanInMinutes = 2;
        private int _longBreakTimerTimeSpanInMinutes = 1;
        private bool _isStarted = false;
        private bool _incrementCounter = false;

        private int _workCounter;
        private int _workCounterFast;
        private int _shortBreakCounter;
        private int _shortBreakCounterFast;
        private int _longBreakCounter;

        public ViewModel()
        {
            ThreadPoolTimer timer = ThreadPoolTimer.CreatePeriodicTimer(TimerHandler, TimeSpan.FromSeconds(1));
            StartClick = new Helper.ActionCommand(StartClickCommand);
            _longTimer = TimeSpan.FromMinutes(_workTimerTimeSpanInMinutes);
            _shortTimer = TimeSpan.FromMinutes(_shortBreakTimerTimeSpanInMinutes);
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
                         _workCounterFast++;
                         if (_workCounterFast == _workTimerTimeSpanInMinutes * 60)
                         {
                             _workCounter++;
                             _workCounterFast = 0;
                         }
                         TimerTextBlock = _shortTimer.ToString(@"m\:ss");
                         _shortTimer -= TimeSpan.FromSeconds(1);
                         if (_shortTimer <= TimeSpan.Zero)
                         {
                             _shortBreakCounterFast++;
                             if (_shortBreakCounterFast == _shortBreakTimerTimeSpanInMinutes * 60)
                             {
                                 _shortBreakCounter++;
                                 _shortBreakCounterFast = 0;
                                 _workCounter++;
                                 _workCounterFast = 0;
                             }
                             _longTimer = TimeSpan.FromMinutes(_workTimerTimeSpanInMinutes);
                             _shortTimer = TimeSpan.FromMinutes(_shortBreakTimerTimeSpanInMinutes);
                         }
                     }
                 }
             });
        }

        private void StartClickCommand()
        {
            _longTimer = TimeSpan.FromMinutes(_workTimerTimeSpanInMinutes);
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