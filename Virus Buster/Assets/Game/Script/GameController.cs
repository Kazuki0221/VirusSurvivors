using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject skillSelect = null;
    public GameObject expObj = null;
    public GameObject healObj;
    public GameObject gaugeHealObj;

    public static float minutes = 30;
    public static float seconds = 0;
    public static int score = 0;
    TextMeshProUGUI text;
    [SerializeField] TextMeshProUGUI zannkiText;
    Player player;

    [SerializeField] GameObject fade;
    public int wabe = 1;
    void Start()
    {
        text = GameObject.Find("Time").GetComponent<TextMeshProUGUI>();
        player = FindObjectOfType<Player>();
        skillSelect.SetActive(false);
        fade.SetActive(false);

    }

    void Update()
    {
        if (Heart.currentHp > 0 && Spawner.zannki >= 0 && Player.currentHp > 0)
        {
            if (player.expLevel.UpLevel())
            {
                skillSelect.SetActive(true);
                player.activeSkillSelect = true;

            }
            if (skillSelect.activeSelf == false)
            {
                if (seconds <= 0)
                {
                    seconds = 59;
                    minutes--;
                }
                seconds -= Time.deltaTime;
            }
            

            text.text = $"{minutes}:{seconds.ToString("00")}";
            zannkiText.text = $"BeesCount:{Spawner.zannki}";
        }

        if(Heart.currentHp <= 0 || (minutes <= 0 && seconds <= 0) || (Spawner.zannki == 0 && Player.currentHp <= 0))
        {
            fade.SetActive(true);
        }

        if((int)minutes % 5 == 0 && (int)seconds == 0)
        {
            wabe++;
        }
    }

    public void ToResult()
    {
        SceneManager.LoadScene("Result");
    }
}
