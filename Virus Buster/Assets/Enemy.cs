using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    public ObjectPool<GameObject> objectPool;

    [SerializeField] float speed = 10;
    SpriteRenderer image;

    float time = 0;

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

    }

    public void Damage()
    {
        Destroy();
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
        image.enabled = false;
        isActive = false;
    }


}
