﻿using System;
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
        private string _workTimerTextBlock;
        private string _shortTimerTextBlock;
        private string _longTimerTextBlock;
        private TimeSpan _workTimer;
        private TimeSpan _shortBreakTimer;
        private TimeSpan _longBreakTimer;
        private int _workTimerTimeSpanInMinutes = 3;
        private int _shortBreakTimerTimeSpanInMinutes = 1;
        private int _longBreakTimerTimeSpanInMinutes = 2;
        private bool _isStarted = false;
        private int _workCounter;
        private int _zeroCrossingCounter;
        private int _shortBreakCounter;
        private int _longBreakCounter;
        private int _timeSpan;

        public ViewModel()
        {
            ThreadPoolTimer timer = ThreadPoolTimer.CreatePeriodicTimer(TimerHandler, TimeSpan.FromSeconds(1));
            StartClick = new Helper.ActionCommand(StartClickCommand);
            _workTimer = TimeSpan.FromMinutes(_workTimerTimeSpanInMinutes);
            _shortBreakTimer = TimeSpan.FromMinutes(_shortBreakTimerTimeSpanInMinutes);
            _longBreakTimer = TimeSpan.FromMinutes(_longBreakTimerTimeSpanInMinutes);
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

        public string WorkTimerTextBlock
        {
            get { return _workTimerTextBlock; }
            set
            {
                _workTimerTextBlock = value;
                OnPropertyChanged(nameof(WorkTimerTextBlock));
            }
        }

        public string ShortTimerTextBlock
        {
            get { return _shortTimerTextBlock; }
            set
            {
                _shortTimerTextBlock = value;
                OnPropertyChanged(nameof(ShortTimerTextBlock));
            }
        }

        public string LongTimerTextBlock
        {
            get { return _longTimerTextBlock; }
            set
            {
                _longTimerTextBlock = value;
                OnPropertyChanged(nameof(LongTimerTextBlock));
            }
        }

        private async void TimerHandler(ThreadPoolTimer timer)
        {
            var dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
            await dispatcher.RunAsync(
             CoreDispatcherPriority.Normal, () =>
             {
                 // Your UI update code goes here!
                 TimerTextBlock = _workTimer.ToString(@"m\:ss");
                 WorkTimerTextBlock = _workCounter.ToString();
                 ShortTimerTextBlock = _shortBreakCounter.ToString();
                 LongTimerTextBlock = _longBreakCounter.ToString();
                 if (!_isStarted)
                     return;
                 
                 _workTimer -= TimeSpan.FromSeconds(1);
                 TimerTextBlock = _workTimer.ToString(@"m\:ss");
                 if (_workTimer == TimeSpan.Zero)
                 {
                     TimerTextBlock = _workTimer.ToString(@"m\:ss");
                     _zeroCrossingCounter++;
                  
                     if (_zeroCrossingCounter % 2 == 0)
                     {
                         if (_workCounter % 4 == 0)
                         {
                             _longBreakCounter++;
                         }
                         else
                         {
                             _shortBreakCounter++;
                         }
                         _timeSpan = _workTimerTimeSpanInMinutes;
                     }
                     else
                     {
                         _workCounter++;

                         if (_workCounter % 4 == 0)
                         {
                             _timeSpan = _longBreakTimerTimeSpanInMinutes;
                         }
                         else
                         {
                             _timeSpan = _shortBreakTimerTimeSpanInMinutes;
                         }
                     }
                     _workTimer = TimeSpan.FromMinutes(_timeSpan);
                 }
             });
        }

        private void StartClickCommand()
        {
            _workTimer = TimeSpan.FromMinutes(_workTimerTimeSpanInMinutes);
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