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
    //ObjectPool<GameObject> expPool;

    public static float minutes = 30;
    public static float seconds = 0;
    public static int score = 0;
    TextMeshProUGUI text;
    [SerializeField] TextMeshProUGUI zannkiText;
    Player player;

    [SerializeField] GameObject fade;
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
            if (!skillSelect.activeSelf)
            {
                if (seconds <= 0)
                {
                    seconds = 59;
                    minutes--;
                }
                seconds -= Time.deltaTime;
            }
            else
            {
                foreach (var e in Spawner.eList)
                {
                    e.speed = 0;
                }
            }

            text.text = $"{minutes}:{seconds.ToString("00")}";
            zannkiText.text = $"BeesCount:{Spawner.zannki}";
        }

        if(Heart.currentHp <= 0 || (minutes <= 0 && seconds <= 0) || (Spawner.zannki == 0 && Player.currentHp <= 0))
        {
            fade.SetActive(true);
        }
    }

    public void ToResult()
    {
        SceneManager.LoadScene("Result");
    }
}
