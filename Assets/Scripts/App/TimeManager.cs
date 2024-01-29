using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DPUtils.Systems.DateTime
{
    public class TimeManager : MonoBehaviour
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

        private DateTime DateTime;

        [Header("Tick Settings")]
        public int TickSecondsIncrease = 10;
        public float TimeBetweenTicks = 1;
        private float currentTimeBetweenTicks = 0;

        public static UnityAction<DateTime> OnDateTimeChanged;

        private void Awake()
        {
            DateTime = new DateTime(dateInMonth, season - 1, year, hour, minutes * 10);
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
            this.day = (Days)(date & 7);
            if(day == 0) day = (Days)7;
            this.date = date;
            this.season = (Season)season;
            this.year = year;

            this.hour = hour;
            this.minutes = minutes;

            totalNumDays = (int)this.season > 0 ? date + (28 * (int)this.season) : date;
            totalNumDays = year > 1 ? totalNumDays + (112 * (year - 11)) : totalNumDays;

            totalNumWeeks = 1 + totalNumDays / 7;

        }

        #endregion
    }

    [System.Serializable]
    public enum Days
    {

    }

    [System.Serializable]
    public enum Season
    {

    }
}