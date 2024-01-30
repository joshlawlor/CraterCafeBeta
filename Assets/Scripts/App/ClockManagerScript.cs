using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DPUtils.Systems.DateTime;

public class ClockManagerScript : MonoBehaviour
{
    public TextMeshProUGUI Date, Time, Season, Week;

    private void OnEnable(){
        TimeManagerScript.OnDateTimeChanged += UpdateDateTime;
    }

    private void OnDisable(){
        TimeManagerScript.OnDateTimeChanged -= UpdateDateTime;
    }

    private void UpdateDateTime(DateTime dateTime) {
        Date.text = dateTime.DateToString();
        Time.text = dateTime.TimeToString();
        Season.text = dateTime.Season.ToString();
        Week.text = $"WEEK: {dateTime.CurrentWeek.ToString()}";
        
    }
}
