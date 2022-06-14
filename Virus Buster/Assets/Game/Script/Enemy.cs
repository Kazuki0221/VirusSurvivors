using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    public ObjectPool<GameObject> objectPool;

    public float speed = 10;
    SpriteRenderer image;

    private void Awake()
    {
        image = GetComponent<SpriteRenderer>();
    }
    void Update()
    {

        if (!isActive) return;
        Vector3 sub = GameManager.Heart.transform.position - transform.position;
        sub.Normalize();

        transform.position += sub * speed * Time.deltaTime;
        transform.rotation = Quaternion.FromToRotation(Vector3.up, sub);

    }

    public void Damage()
    {
        GameController.score++;
        Destroy();
        Spawner.eList.Remove(this);
    }

    public bool isActive = false;
    public void DisActiveForInstantiate()
    {
        image.enabled = false;
        isActive = false;
    }
    public void Create()
    {
        image.enabled = true;
        isActive = true;
    }

    public void Destroy()
    {
        FindObjectOfType<Spawner>().objectPool.Release(gameObject);
        image.enabled = false;
        isActive = false;
    }


}
