using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heat : MonoBehaviour
{

    private void OnDisable()
    {
        Player.skillTime = 5f;
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
