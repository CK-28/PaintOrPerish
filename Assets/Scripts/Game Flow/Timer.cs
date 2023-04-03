/*Sets the Timer value at the top of the in-game UI*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timeValue = 100;
    public TMP_Text timeText;

    // Update is called once per frame
    void Update()
    {
        if (timeValue > 0)
        {
            timeValue = timeValue - Time.deltaTime;
        }
        else
        {
            // Needed to stop it from showing as past 0
            timeValue = 0;
        }

        DisplayTime(timeValue);
    }

    void DisplayTime(float timeToDisplay)
    {
        // Eliminates chance of negative values due to frames
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }
        else if (timeToDisplay > 0)         // Needed to show the last second as 1 instead of 0. (Better for player awareness)
        {
            timeToDisplay = timeToDisplay + 1;
        }

        // Calculating minutes and seconds for displaying
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public float returnTimeLeft()
    {
        return timeValue;
    }
}
