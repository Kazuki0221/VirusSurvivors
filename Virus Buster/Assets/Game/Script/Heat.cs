using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heat : MonoBehaviour
{
    private void OnEnable()
    {
        Player.cutIn.SetActive(true);
    }

    private void OnDisable()
    {
        Player.skillTime = 10f;
        //Player.cutIn.SetActive(false);
    }

    void Update()
    {
        if(gameObject.activeSelf)
        {
            Player.skillTime -= Time.deltaTime;
            if (Player.skillTime <= 0)
            {
                this.gameObject.SetActive(false);
            }
            if(10 - Player.skillTime > 1)
            {
                Player.cutIn.SetActive(false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            var e = collision.gameObject.GetComponent<Enemy>();
            e.Destroy();
            e.transform.position = new Vector2(100, 100);
        }
    }
}
