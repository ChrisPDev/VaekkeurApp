using MediaManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace VaekkeurApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Checks if there is a match between device time and the given time in seconds
        /// </summary>
        /// <param name="alarmTimeInSeconds"></param>
        /// <param name="alarmTone"></param>
        /// <returns></returns>
        public async Task CheckTimeForMatch(int alarmTimeInSeconds, string alarmToneUrl)
        {
            // Device time now
            DateTime time = DateTime.Now;

            // Recalculating device time into total seconds
            int hoursInSeconds = time.Hour * 60 * 60;
            int minutesInSeconds = time.Minute * 60;
            int seconds = time.Second;
            int totalTimeInSeconds = hoursInSeconds + minutesInSeconds + seconds;

            // Check if there is a match between device time and alarm time
            if (totalTimeInSeconds == alarmTimeInSeconds)
            {
                // If match play a sound
                await CrossMediaManager.Current.Play(alarmToneUrl);
            }
        }
    }
}
