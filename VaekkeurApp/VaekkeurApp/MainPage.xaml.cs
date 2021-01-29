using MediaManager;
using MediaManager.Playback;
using MediaManager.Player;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VaekkeurApp.Model;
using Xamarin.Forms;


namespace VaekkeurApp
{
    public partial class MainPage : ContentPage
    {
        private List<Alarm> Alarms;
        DateTime _triggerTime;
        public MainPage()
        {
            Alarms = new List<Alarm>();
            PopulateList();
            InitializeComponent();
            CrossMediaManager.Current.Dispose();
            AlarmList.ItemsSource = Alarms;
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

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null) return; // has been set to null, do not 'process' tapped event
            Debug.WriteLine("Tapped: " + e.Item);
            ((ListView)sender).SelectedItem = null; // de-select the row
        }

        private void PopulateList()
        {
            Alarms.Add(new Alarm() { Title = "Morgen", Time = "06:00", Switch = false });
            Alarms.Add(new Alarm() { Title = "Aften", Time = "18:00", Switch = false });
            Alarms.Add(new Alarm() { Title = "Vigtigt Møde", Time = "12:00", Switch = false });
        }
        void CreateBtn(object sender, EventArgs args)
        {
            if (CreateAlarm.IsVisible == false)
            {
                CreateAlarm.IsVisible = true;
                _create.IsVisible = false;
                _save.IsVisible = true;
            }
        }

        void SaveBtn(object sender, EventArgs args)
        {
            if (CreateAlarm.IsVisible == true)
            {
                CreateAlarm.IsVisible = false;
                _create.IsVisible = true;
                _save.IsVisible = false;
            }
        }

        public async void CheckTimeForMatch(string alarmTime)
        {
            DateTime timeNow = DateTime.Now;
            string timeNowFormatted = timeNow.ToString("HH:mm");

            if (timeNowFormatted == alarmTime)
            {
                CrossMediaManager.Current.Init();
                CrossMediaManager.Current.RepeatMode = RepeatMode.All;
                await CrossMediaManager.Current.PlayFromResource("asset:///Battlefield.mp3");
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
        private void TestStart_Clicked(object sender, EventArgs e)
        {
            CheckTimeForMatch(TestEntry.Text);
        }
        private void TestStop_Clicked(object sender, EventArgs e)
        {
            MediaPlayerState state = CrossMediaManager.Current.State;
            if (state == MediaPlayerState.Playing)
            {
                CrossMediaManager.Current.Stop();
            }
        }
        private void TestSnooze_Clicked(object sender, EventArgs e)
        {
            MediaPlayerState state = CrossMediaManager.Current.State;
            if (state == MediaPlayerState.Playing)
            {
                CrossMediaManager.Current.Stop();
                CrossMediaManager.Current.Dispose();

                Device.StartTimer(TimeSpan.FromSeconds(300), () =>
                {
                    CrossMediaManager.Current.Init();
                    CrossMediaManager.Current.RepeatMode = RepeatMode.All;
                    CrossMediaManager.Current.PlayFromResource("asset:///Battlefield.mp3");

                    return false;
                });
            }
        }
    }
}
