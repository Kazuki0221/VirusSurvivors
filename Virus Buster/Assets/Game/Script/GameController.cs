using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject skillSelect = null;
    public GameObject expObj = null;
    //ObjectPool<GameObject> expPool;

    public static float minutes = 30;
    public static float seconds = 0;
    public static int score = 0;
    TextMeshProUGUI text;
    Player player;
    public float remainESpeed;

    void Start()
    {
        text = GameObject.Find("Time").GetComponent<TextMeshProUGUI>();
        player = FindObjectOfType<Player>();
        skillSelect.SetActive(false);
    }

    void Update()
    {
        if(player.expLevel.UpLevel())
        {
            skillSelect.SetActive(true);
            player.activeSkillSelect = true;
            
        }
        if(!skillSelect.activeSelf)
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
                remainESpeed = e.speed;
                e.speed = 0;
            }
        }

        text.text = $"{minutes}:{seconds.ToString("00")}";
    }
}
