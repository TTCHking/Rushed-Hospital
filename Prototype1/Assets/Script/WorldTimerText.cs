using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldTimerText : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public Timer timer;

    private void FixedUpdate()
    {
        if (timer.timerCount > 0 && timer.isStartTimer)
        {
            int min = (int)timer.timerCount / 60;
            int sec = (int)timer.timerCount % 60;

            timerText.text = string.Format("{0:00}:{1:00}", min, sec);
        }
    }
}
