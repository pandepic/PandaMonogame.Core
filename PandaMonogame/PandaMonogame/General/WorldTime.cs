using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PandaMonogame
{
    public class WorldTime
    {
        public delegate void WORLDTIME_EVENT_FUNC(WorldTime worldTime);

        public WORLDTIME_EVENT_FUNC OnMinuteChange { get; set; } = null;
        public WORLDTIME_EVENT_FUNC OnHourChange { get; set; } = null;
        public WORLDTIME_EVENT_FUNC OnDayChange { get; set; } = null;
        public WORLDTIME_EVENT_FUNC OnWeekChange { get; set; } = null;
        public WORLDTIME_EVENT_FUNC OnMonthChange { get; set; } = null;
        public WORLDTIME_EVENT_FUNC OnYearChange { get; set; } = null;

        public float TimePerMinute { get; set; } = 60000.0f;
        public float MinutesPerHour { get; set; } = 60.0f;
        public float HoursPerDay { get; set; } = 24.0f;

        public int CurrentYear { get; set; } = 0;

        protected float _currentTimePassed = 0.0f;
        public float CurrentMinutes { get; set; } = 0.0f;
        public float CurrentHours { get; set; } = 0.0f;
        public float CurrentDay { get; set; } = 0.0f;
        public int CurrentDayIndex { get; set; } = 0;
        public int CurrentMonthIndex { get; set; } = 0;

        public Dictionary<string, int> Months { get; set; } = new Dictionary<string,int>();
        public List<string> Days { get; set; } = new List<string>();

        public WorldTime()
        {

        }

        public void Update(GameTime gameTime)
        {
            _currentTimePassed += (float)gameTime.ElapsedGameTime.Milliseconds;
            
            while (_currentTimePassed >= TimePerMinute)
            {
                _currentTimePassed -= TimePerMinute;

                CurrentMinutes++;

                OnMinuteChange?.Invoke(this);

                while (CurrentMinutes >= MinutesPerHour)
                {
                    CurrentMinutes -= MinutesPerHour;

                    CurrentHours++;

                    OnHourChange?.Invoke(this);

                    while (CurrentHours > HoursPerDay)
                    {
                        CurrentHours -= HoursPerDay;

                        CurrentDay++;

                        CurrentDayIndex++;

                        OnDayChange?.Invoke(this);

                        if (CurrentDayIndex >= Days.Count)
                        {
                            CurrentDayIndex = 0;

                            OnWeekChange?.Invoke(this);
                        }

                        if (CurrentDay > (float)Months.ElementAt(CurrentMonthIndex).Value)
                        {
                            CurrentMonthIndex++;

                            OnMonthChange?.Invoke(this);
                        }

                        if (CurrentMonthIndex >= Months.Count)
                        {
                            CurrentMonthIndex = 0;

                            CurrentYear++;

                            OnYearChange?.Invoke(this);
                        }
                    }
                }
            }
        }

        // %t - minutes
        // %h - hours
        // %d - current day
        // %D - current day name
        // %m - current month index
        // %M - current month name
        // %y - year
        public string GetTimeAsString(string format)
        {
            string ret = format;

            ret = ret.Replace("%t", CurrentMinutes.ToString());
            ret = ret.Replace("%h", CurrentHours.ToString());
            ret = ret.Replace("%d", CurrentDay.ToString());
            ret = ret.Replace("%D", Days[CurrentDayIndex]);
            ret = ret.Replace("%m", CurrentMonthIndex.ToString());
            ret = ret.Replace("%M", Months.ElementAt(CurrentMonthIndex).Key);
            ret = ret.Replace("%y", CurrentYear.ToString());

            return ret;
        }

        public void InitRealDays()
        {
            Days.Clear();

            Days.Add("Monday");
            Days.Add("Tuesday");
            Days.Add("Wednesday");
            Days.Add("Thursday");
            Days.Add("Friday");
            Days.Add("Saturday");
            Days.Add("Sunday");
        }

        public void InitRealMonths()
        {
            Months.Clear();

            Months.Add("January", 31);
            Months.Add("February", 28);
            Months.Add("March", 31);
            Months.Add("April", 30);
            Months.Add("May", 31);
            Months.Add("June", 30);
            Months.Add("July", 31);
            Months.Add("August", 31);
            Months.Add("September", 30);
            Months.Add("October", 31);
            Months.Add("November", 30);
            Months.Add("December", 31);
        }
    }
}
