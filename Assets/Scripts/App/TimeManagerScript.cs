using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DPUtils.Systems.SaveSystem;


namespace DPUtils.Systems.DateTime
{


    public class TimeManagerScript : MonoBehaviour
    {
        public CurrentDateTimeInfo CurrentDateTimeInfo { get { return new CurrentDateTimeInfo(DateTime.Hour, DateTime.Minutes, DateTime.IsAM, (int)DateTime.Day, DateTime.Date, DateTime.Year, DateTime.Season, DateTime.TotalNumDays, DateTime.TotalNumWeeks); } }
        public TimeInfo CurrentTimeInfo { get { return new TimeInfo(DateTime.Hour, DateTime.IsAM, DateTime.Minutes); } }

        private bool isGamePaused = false;


        [Header("Date & Time Settings")]
        [Range(1, 28)]
        public int dateInMonth;

        [Range(1, 4)]
        public int season;

        [Range(1, 99)]
        public int year;

        [Range(0, 24)]
        public int hour;

        [Range(0, 6)]
        public int minutes;

        public int TickMinutesIncrease = 10;

        private DateTime DateTime;

        [Header("Tick Settings")]
        public float TimeBetweenTicks = 1;
        private float currentTimeBetweenTicks = 0;

        public static UnityAction<DateTime> OnDateTimeChanged;

        private void Awake()
        {
            string saveFilePath = "Assets/SaveFiles/saveData.dat";

            if (File.Exists(saveFilePath))
            {
                SaveData loadedData = SaveData.LoadFromFile(saveFilePath);
                if (loadedData != null)
                {
                    DateTime = new DateTime(
                        loadedData.CurrentDateTimeInfo.Date,
                        (int)loadedData.CurrentDateTimeInfo.Season,
                        loadedData.CurrentDateTimeInfo.Year,
                        loadedData.CurrentDateTimeInfo.Hour,
                        loadedData.CurrentDateTimeInfo.Minutes
                    );

                    Debug.Log("Loaded DateTime from save file.");
                    return;
                }
            }

            // If no save file found or failed to load, create a new DateTime
            DateTime = new DateTime(dateInMonth, season, year, hour, minutes * 10);
            Debug.Log("Created new DateTime.");
        }
        private void Start()
        {
            OnDateTimeChanged?.Invoke(DateTime);
        }

        private void Update()
        {
            currentTimeBetweenTicks += Time.deltaTime;

            if (currentTimeBetweenTicks >= TimeBetweenTicks)
            {
                currentTimeBetweenTicks = 0;
                Tick();
            }
        }

        void Tick()
        {
            // Check if the game is not paused
            if (!isGamePaused)
            {
                AdvanceTime();
            }
        }

        void AdvanceTime()
        {
            DateTime.AdvanceMinutes(TickMinutesIncrease);
            OnDateTimeChanged?.Invoke(DateTime);
        }
        public void PauseDateTime()
        {
            // Set the isGamePaused flag to true when pausing the game
            isGamePaused = true;
        }

        public void ResumeDateTime()
        {
            // Set the isGamePaused flag to false when resuming the game
            isGamePaused = false;
        }

        public void SetDateTime(CurrentDateTimeInfo currentDateTimeInfo)
        {
            // Set internal DateTime based on the provided CurrentDateTimeInfo
            DateTime = new DateTime(
                currentDateTimeInfo.Date,
                (int)currentDateTimeInfo.Season,
                currentDateTimeInfo.Year,
                currentDateTimeInfo.Hour,
                currentDateTimeInfo.Minutes
            );

            // Log the updated DateTime
            // Debug.Log($"DateTime set: {DateTime}");
        }

    }




    [System.Serializable]
    public struct TimeInfo
    {
        public int Hour { get; private set; }
        public int Minutes { get; private set; }
        public bool IsAM { get; private set; }

        public TimeInfo(int hour, bool isAM, int minutes)
        {
            Hour = hour;
            IsAM = isAM;
            Minutes = minutes;
        }
    }

    [Serializable]
    public struct CurrentDateTimeInfo
    {
        public int Hour { get; private set; }
        public int Minutes { get; private set; }
        public bool IsAM { get; private set; }
        public int Day { get; private set; }
        public int Date { get; private set; }
        public int Year { get; private set; }
        public Season Season { get; private set; }

        public int TotalNumDays { get; private set; }
        public int TotalNumWeeks { get; private set; }

        public CurrentDateTimeInfo(int hour, int minutes, bool isAM, int day, int date, int year, Season season, int totalNumDays, int totalNumWeeks)
        {
            Hour = hour;
            Minutes = minutes;
            IsAM = isAM;
            Day = day;
            Date = date;
            Year = year;
            Season = season;
            TotalNumDays = totalNumDays;
            TotalNumWeeks = totalNumWeeks;
        }
    }
    [System.Serializable]
    public struct DateTime
    {
        #region Fields
        private Days day;
        private int date;
        private int year;

        private int hour;
        private int minutes;
        public bool IsAM => hour >= 0 && hour < 12;

        private Season season;

        private int totalNumDays;
        private int totalNumWeeks;
        #endregion

        #region Properties
        public Days Day => day;
        public int Date => date;
        public int Hour => hour;
        public int Minutes => minutes;
        public Season Season => season;
        public int Year => year;
        public int TotalNumDays => totalNumDays;
        public int TotalNumWeeks => totalNumWeeks;
        public int CurrentWeek => totalNumWeeks % 16 == 0 ? 16 : totalNumWeeks % 16;
        #endregion

        #region Constructors
        public DateTime(int date, int season, int year, int hour, int minutes)
        {
            this.day = (Days)(date % 7);
            if (day == 0) day = (Days)7;
            this.date = date;
            this.season = (Season)season;
            this.year = year;

            this.hour = hour;
            this.minutes = minutes;

            totalNumDays = (int)this.season > 0 ? date + (28 * (int)this.season) : date;
            totalNumDays = year > 1 ? totalNumDays + (112 * (year - 1)) : totalNumDays;

            totalNumWeeks = 1 + totalNumDays / 7;
        }
        #endregion

        #region Time Advancement

        public void AdvanceMinutes(int SecondsToAdvanceBy)
        {
            if (minutes + SecondsToAdvanceBy >= 60)
            {
                minutes = (minutes + SecondsToAdvanceBy) % 60;
                AdvanceHour();
            }
            else
            {
                minutes += SecondsToAdvanceBy;
            }
        }

        private void AdvanceHour()
        {
            if ((hour + 1) == 24)
            {
                hour = 0;
                AdvanceDay();
            }
            else
            {
                hour++;
            }
        }

        private void AdvanceDay()
        {
            if (day + 1 > (Days)7)
            {
                day = (Days)1;
                totalNumWeeks++;
            }
            else
            {
                day++;
            }

            date++;

            if (date % 29 == 0)
            {
                AdvanceSeason();
                date = 1;
            }

            totalNumDays++;
        }

        private void AdvanceSeason()
        {
            if (Season == Season.Winter)
            {
                season = Season.Spring;
                AdvanceYear();
            }
            else season++;
        }

        private void AdvanceYear()
        {
            date = 1;
            year++;
        }

        #endregion

        #region Bool Checks

        public bool isNight()
        {
            return hour > 18 || hour < 6;
        }

        public bool isMorning()
        {
            return hour >= 6 && hour <= 12;
        }

        public bool isAfternoon()
        {
            return hour > 12 && hour < 18;
        }

        public bool isWeekend()
        {
            return day > Days.Fri ? true : false;
        }

        public bool IsParticularDay(Days _day)
        {
            return day == _day;
        }

        #endregion

        #region Key Dates
        public DateTime NewYearsDay(int year)
        {
            if (year == 0) year = 1;
            return new DateTime(1, 0, year, 6, 0);
        }

        public DateTime SummerSolstice(int year)
        {
            if (year == 0) year = 1;
            return new DateTime(28, 1, year, 6, 0);
        }

        public DateTime WinterSolstice(int year)
        {
            if (year == 0) year = 1;
            return new DateTime(28, 3, year, 6, 0);

        }
        #endregion

        #region To Strings

        public override string ToString()
        {
            return $"Date: {DateToString()} Season: {season} Time: {TimeToString()} " +
            $"\nTotal Days: {totalNumDays} | Total Weeks: {totalNumWeeks}";
        }

        public string DateToString()
        {
            return $"{Day} {Date} {Year.ToString("D2")}";
        }

        public string TimeToString()
        {
            int adjustedHour = 0;

            if (hour == 0)
            {
                adjustedHour = 12;
            }
            else if (hour == 24)
            {
                adjustedHour = 12;
            }
            else if (hour >= 13)
            {
                adjustedHour = hour - 12;
            }
            else
            {
                adjustedHour = hour;
            }

            string AmPm = hour == 0 || hour < 12 ? "AM" : "PM";

            return $"{adjustedHour.ToString("D2")}:{minutes.ToString("D2")} {AmPm}";
        }

        #endregion
    }

    [System.Serializable]
    public enum Days
    {
        NULL = 0,
        Mon = 1,
        Tue = 2,
        Wed = 3,
        Thur = 4,
        Fri = 5,
        Sat = 6,
        Sun = 7
    }

    [System.Serializable]
    public enum Season
    {
        Spring = 0,
        Summer = 1,
        Autumn = 2,
        Winter = 3
    }




}