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
        public MainPage()
        {
            Alarms = new List<Alarm>();
            PopulateList();
            InitializeComponent();
            CrossMediaManager.Current.Dispose();
            AlarmList.ItemsSource = Alarms;
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null) return; // has been set to null, do not 'process' tapped event
            Debug.WriteLine("Tapped: " + e.Item);
            ((ListView)sender).SelectedItem = null; // de-select the row
        }

        DateTime morgen = DateTime.Parse("08:00:00 AM");
        DateTime aften = DateTime.Parse("18:00:00 PM");
        DateTime Vigtigt = DateTime.Parse("12:00:00 PM");

        private void PopulateList()
        {



            Alarms.Add(new Alarm() { Name = "Morgen", Time = morgen.ToString("HH:mm"), isActive = false });
            Alarms.Add(new Alarm() { Name = "Aften", Time = aften.ToString("HH:mm"), isActive = false });
            Alarms.Add(new Alarm() { Name = "Vigtigt Møde", Time = Vigtigt.ToString("HH:mm"), isActive = false });
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
