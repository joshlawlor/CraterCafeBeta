using UnityEngine;
using TMPro;
using DPUtils.Systems.DateTime;

public class ClockManagerScript : MonoBehaviour
{
    public RectTransform ClockFace;

    private float startingRotation;
    public TextMeshProUGUI Date, Time, Season, Week, TotalDays;

    public void Awake()
    {

        startingRotation = ClockFace.localEulerAngles.z;
    }

    private void OnEnable()
    {
        TimeManagerScript.OnDateTimeChanged += UpdateDateTime;
    }

    private void OnDisable()
    {
        TimeManagerScript.OnDateTimeChanged -= UpdateDateTime;
    }

    private void UpdateDateTime(DateTime dateTime)
    {
        Date.text = dateTime.DateToString();
        Time.text = dateTime.TimeToString();
        Season.text = dateTime.Season.ToString();
        Week.text = $"WEEK: {dateTime.CurrentWeek.ToString()}";
        TotalDays.text = $"DAY: {dateTime.TotalNumDays.ToString()}";


        float t = (float)dateTime.Hour / 24f;

        float newRotation = Mathf.Lerp(0, 360, t);
        ClockFace.localEulerAngles = new Vector3(0, 0, newRotation + startingRotation);



    }
}
