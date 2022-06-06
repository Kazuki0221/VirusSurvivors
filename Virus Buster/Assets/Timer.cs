using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float minutes = 30;
    public float seconds = 0;
    TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        if(seconds <= 0)
        {
            seconds = 59;
            minutes--;
        }
        seconds -= Time.deltaTime;


        text.text = $"{minutes}:{seconds.ToString("00")}";
    }
}
