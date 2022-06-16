using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    public ObjectPool<GameObject> bulletPool;

    [SerializeField] float speed = 50;
    SpriteRenderer image;
    Enemy target;
    Vector3 shootVec;

    float timer = 0;
    
    private void Awake()
    {
        image = GetComponent<SpriteRenderer>();
    }

    public void Shoot()
    {
        var list = Spawner.eList;
        target = null;
        float len = -1;
        Vector3 vec;
        foreach(var e in list)
        {
            if (!e.isActive) continue;
            vec = e.transform.position - GameManager.Player.transform.position;
            this.transform.rotation = Quaternion.FromToRotation(Vector3.up, vec);
            if (len == -1 || vec.magnitude < len)
            {
                target = e;
                len = vec.magnitude;

            }

            if (!target) return;
            shootVec = target.transform.position - GameManager.Player.transform.position;
            shootVec.Normalize();
        }

    }

    void Update()
    {
        transform.position += shootVec * speed* Time.deltaTime;

        var list = Spawner.eList;
        target = null;
        Vector3 vec;
        foreach(var e in list)
        {
            if (!e.isActive) continue;
            vec = e.transform.position - gameObject.transform.position; 
            if(vec.magnitude < 1.0f)
            {

                var rand = Random.Range(1, 100);
                if(rand <= 95)
                {
                    var exp = FindObjectOfType<GameController>().expObj;
                    Instantiate(exp, e.transform.position, Quaternion.identity);
                }
                else if(rand > 95 && rand <= 98)
                {
                    var gHeal = FindObjectOfType<GameController>().gaugeHealObj;
                    Instantiate(gHeal, e.transform.position, Quaternion.identity);
                }
                else
                {
                    var heal = FindObjectOfType<GameController>().healObj;
                    Instantiate(heal, e.transform.position, Quaternion.identity);
                }
                e.Damage();
                Destroy();
                break;
            }
        }

        timer += Time.deltaTime;
        if(timer > 5.0f)
        {
            Destroy();
        }
    }

    public bool isActive = false;
    public void DisActiveForInstantiate()
    {
        image.enabled = false;
        isActive = false;
    }
    public void Create()
    {
        timer = 0;
        image.enabled = true;
        isActive = true;
    }

    public void Destroy()
    {
        image.enabled = false;
        isActive = false;
    }


}
