using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace DPUtils.Systems.DateTime
{


    public class TimeManagerScript : MonoBehaviour
    {
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
            DateTime = new DateTime(dateInMonth, season - 1, year, hour, minutes * 10);

            Debug.Log($"Starting Date: {DateTime}");

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
            AdvanceTime();

        }

        void AdvanceTime()
        {
            DateTime.AdvanceMinutes(TickMinutesIncrease);
            OnDateTimeChanged?.Invoke(DateTime);
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