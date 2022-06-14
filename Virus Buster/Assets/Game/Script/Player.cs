using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using Cinemachine;

public class Player : MonoBehaviour
{
    //パラメータ
    [SerializeField] int hp = 10;
    public float speed = 0.1f;
    [SerializeField] float shootTime = 0.3f;
    
    Rigidbody2D rb = default;
    float timer = 0.0f;

    //弾
    [SerializeField] GameObject bullet = null;
    List<Bullet> bList = new List<Bullet>();
    ObjectPool<GameObject> bulletPool;
    int defaultCapacity = 100;
    int maxSize = 100;

    [SerializeField] GameObject skill = null;
    [SerializeField] Image gauge;
    public bool activeSkillSelect = false;
    public static float skillTime = 5f;
    [SerializeField] float addGaugeAmount = 1.0f;
    float currentGauge = 0f;
    float fullGauge = 100f;

    //UI
    int currentHp;
    public Slider hpBar;
    [SerializeField]TextMeshProUGUI text;

    CinemachineVirtualCamera cam;
    Animator anim;

    //レベル関連
    public ExpLevel expLevel = new ExpLevel();
    static readonly int []leveTable = { 0, 10, 200, 300 };
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

        gauge = GameObject.Find("Gauge").GetComponent<Image>();
        text = GameObject.Find("GaugeText").GetComponent<TextMeshProUGUI>();
        currentGauge = 0;
        gauge.fillAmount = 0;
        skill.GetComponent<Heat>();
        skill.SetActive(false);
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(GameController.minutes >= 0 && GameController.seconds >= 0 && activeSkillSelect == false)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            Vector2 dir = new Vector2(h, v);
            rb.velocity = dir.normalized * speed;
            anim.SetFloat("Horizontal", h);

            timer += Time.deltaTime;
            if (timer > shootTime)
            {
                var go = CreateBullet().GetComponent<Bullet>();
                go.Shoot();

                timer -= shootTime;
            }

            if (Input.GetKeyDown(KeyCode.Space) && gauge.fillAmount == 1)
            {
                skill.SetActive(true);
                gauge.fillAmount = 0;
                currentGauge = 0;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
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
        if(collision.gameObject.tag == "Enemy" && !skill.activeSelf)
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
            Destroy(collision.gameObject);
            Debug.Log($"Level:{expLevel.Level}, Exp:{expLevel.Exp}");
            if (gauge.fillAmount < 1 && skillTime >= 5f)
            {
                currentGauge += addGaugeAmount;
                gauge.fillAmount = currentGauge / fullGauge;
                text.text = $"{100 * gauge.fillAmount}%";
            }
        }
    }

    public void AddExp(int exp)
    {
        expLevel.AddExp(exp, leveTable);
    }

    public void UpSpeed()
    {
        speed++;
        activeSkillSelect = false;
    }

    public void UpHp()
    {
        hp++;
        hpBar.value = 1;
        currentHp = hp;
        activeSkillSelect = false;
    }

    public void UpGauge()
    {
        addGaugeAmount++;
        activeSkillSelect = false;
    }

}
