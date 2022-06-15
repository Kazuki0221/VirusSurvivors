using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class Result : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeResuslt;
    [SerializeField] TextMeshProUGUI scoreResult;
    void Start()
    {
        timeResuslt.text = $"Time    {29 - GameController.minutes}:{(59 - GameController.seconds).ToString("00")}";
        scoreResult.text = $"Score   {GameController.score}";
    }


    public void ToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
