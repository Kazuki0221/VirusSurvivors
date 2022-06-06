using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using Cinemachine;

public class Player : MonoBehaviour
{
    //ÉpÉâÉÅÅ[É^
    [SerializeField] int hp = 3;
    [SerializeField] float atk = 2f;
    [SerializeField] float def = 1f;
    [SerializeField] float speed = 0.1f;
    [SerializeField] float shootTime = 0.3f;
    
    Rigidbody2D rb = default;
    float timer = 0.0f;

    //íe
    [SerializeField] GameObject bullet = null;
    List<Bullet> bList = new List<Bullet>();
    ObjectPool<GameObject> bulletPool;
    int defaultCapacity = 100;
    int maxSize = 100;

    //UI
    int currentHp;
    public Slider hpBar;
    Timer time;

    CinemachineVirtualCamera cam;

    public ExpLevel expLevel = new ExpLevel();
    static readonly int []leveTable = { 0, 100, 200, 300 };
    private void Awake()
    {
        bulletPool = new ObjectPool<GameObject>(
            CreateBullet,
            OnTakeFromBullet,
            OnReturnedToBullet,
            OnDestroyBullet,
            true,
            defaultCapacity,
            maxSize
            );

        GameManager.Instance.SetPlayer(this);
        rb = GetComponent<Rigidbody2D>();
        hpBar.value = 1;
        currentHp = hp;

        cam = FindObjectOfType<CinemachineVirtualCamera>();
        time = FindObjectOfType<Timer>();
    }

    void Update()
    {
        if(time.minutes >= 0 && time.seconds >= 0)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            Vector2 dir = new Vector2(h, v);
            rb.velocity = dir.normalized * speed;
        }
        
        timer += Time.deltaTime;
        if(timer > shootTime)
        {
            var go = CreateBullet().GetComponent<Bullet>();
            go.Shoot();

            timer -= shootTime;
        }

        cam.Follow = this.transform;
        cam.LookAt = this.transform;

    }

    GameObject CreateBullet()
    {
        var go = Instantiate(bullet, this.transform.position, bullet.transform.rotation);
        var script = go.GetComponent<Bullet>();
        script.DisActiveForInstantiate();
        bList.Add(script);
        if(!script.isActive)
        {
            script.Create();
        }
        script.bulletPool = bulletPool;
        return go;
    }

    void OnTakeFromBullet(GameObject go)
    {
        go.SetActive(false);
    }

    void OnReturnedToBullet(GameObject go)
    {
        go.SetActive(true);
    }

    void OnDestroyBullet(GameObject go)
    {
        Destroy(go);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            if(currentHp > 0)
            {
                currentHp--;
            }
            else if(currentHp <= 0 && expLevel.Exp >= 0)
            {
                AddExp(-50);
            }
            hpBar.value = (float)currentHp / (float)hp;
        }

        if(collision.gameObject.tag == "Exp")
        {
            AddExp(1);
            Debug.Log($"Level:{expLevel.Level}, Exp:{expLevel.Exp}");
        }
    }

    public void AddExp(int exp)
    {
        expLevel.AddExp(exp, leveTable);
    }
}
