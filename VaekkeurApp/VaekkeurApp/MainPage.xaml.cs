using MediaManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace VaekkeurApp
{
    public partial class MainPage : ContentPage
    {
        DateTime _triggerTime;
        public MainPage()
        {
            InitializeComponent(); 
            CrossMediaManager.Current.Dispose();
        }

        bool OnTimerTick()
        {
            if (_switch.IsToggled && DateTime.Now >= _triggerTime)
            {
                _switch.IsToggled = false;
                DisplayAlert("Timer Alert", "The '" + _entry.Text + "' timer has elapsed", "OK");
            }
            return true;
        }        

        void CreateBtn(object sender, EventArgs args)
        {
            if (CreateAlarm.IsVisible == true)
            {
                CreateAlarm.IsVisible = false;
            } else if (CreateAlarm.IsVisible == false) {
                CreateAlarm.IsVisible = true;
            }
        }


        /// <summary>
        /// Checks if there is a match between device time and the given time in seconds
        /// </summary>
        /// <param name="alarmTimeInSeconds"></param>
        /// <param name="alarmTone"></param>
        /// <returns></returns>

        public async void CheckTimeForMatch(int alarmTimeInSeconds, string alarmTone)
        {
            // Device time now
            DateTime time = DateTime.Now;

            // Recalculating device time into total seconds
            int hoursInSeconds = time.Hour * 60 * 60;
            int minutesInSeconds = time.Minute * 60;
            int seconds = time.Second;
            int totalTimeInSeconds = hoursInSeconds + minutesInSeconds + seconds;

            // Check if there is a match between device time and alarm time
            if (totalTimeInSeconds >= alarmTimeInSeconds - 5 && totalTimeInSeconds <= alarmTimeInSeconds + 5)
            {
                CrossMediaManager.Current.Init();
                // If match play a sound
                await CrossMediaManager.Current.PlayFromResource(alarmTone);
            }
        }

        void OnTimePickerPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Time")
            {
                SetTriggerTime();
            }
        }
        void OnSwitchToggled(object sender, ToggledEventArgs args)
        {
            SetTriggerTime();
        }

        void SetTriggerTime()
        {
            if (_switch.IsToggled)
            {
                _triggerTime = DateTime.Today + TodayTime.Time;
                if(_triggerTime < DateTime.Now)
                {
                    _triggerTime += TimeSpan.FromDays(1);
                }
            }
        }
        private void TestButton_Clicked(object sender, EventArgs e)
        {
            CheckTimeForMatch(Int32.Parse(TestEntry.Text), "asset:///Battlefield.mp3"); // Sat til at køre kl 15:00 i sekunder. Timer * 60 * 60 = Sekunder
        }
    }
}
