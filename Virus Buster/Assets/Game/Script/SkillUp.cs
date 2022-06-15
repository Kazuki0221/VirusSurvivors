using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUp : MonoBehaviour
{
    [SerializeField]GameObject[] skills = new GameObject[4];
    [SerializeField]Transform[] pos = new Transform[3];
    private void OnEnable()
    {
        var list = new List<int>();
        for(int i = 0; i < skills.Length; i++)
        {
            list.Add(i);
            skills[i].SetActive(false);
        }
        for(int i = 0; i < pos.Length; i++)
        {
            int index = Random.Range(0, list.Count);
            var num = list[index];
            skills[num].SetActive(true);
            skills[num].transform.position = pos[i].position;
            list.RemoveAt(index);
        }
    }
    private void OnDisable()
    {
        var gameController = FindObjectOfType<GameController>();
        foreach(var e in Spawner.eList)
        {
            //Debug.Log("a");

            e.speed = 3;
        }
    }
    public void PlayerSpeed()
    {
        FindObjectOfType<Player>().UpSpeed();
        this.gameObject.SetActive(false);
    }

    public void PlayerHp()
    {
        FindObjectOfType<Player>().UpHp();
        this.gameObject.SetActive(false);
    }

    public void UpGauge()
    {
        FindObjectOfType<Player>().UpGauge();
        this.gameObject.SetActive(false);
    }

    public void HeartHp()
    {
        FindObjectOfType<Heart>().UpHp();
        this.gameObject.SetActive(false);
    }
}
