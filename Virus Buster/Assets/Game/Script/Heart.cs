using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    int maxHP = 100;
    int currentHp;
    Rigidbody2D rb = default;

    [SerializeField] Slider slider = default;
    [SerializeField] TextMeshProUGUI text = default;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        slider.value = 1;
        currentHp = maxHP;
        GameManager.Instance.SetHeart(this);
    }

    void Update()
    {
        text.text = $"HP:{currentHp}/{maxHP}";
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            if(currentHp > 0)
            {
                currentHp--;
            }
            slider.value = (float)currentHp / (float)maxHP;
            collision.gameObject.GetComponent<Enemy>().Destroy();
        }
    }

    public void UpHp()
    {
        maxHP += 10;
        slider.value = 1;
        currentHp = maxHP;
        FindObjectOfType<Player>().activeSkillSelect = false;
    }
}
