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

    //スキル関連
    [SerializeField] GameObject skill = null;
    Vector2 skillArea;
    [SerializeField] Image gauge;
    public bool activeSkillSelect = false;
    public static float skillTime = 10f;
    [SerializeField] float addGaugeAmount = 1.0f;
    float currentGauge = 0f;
    float fullGauge = 100f;
    public static GameObject cutIn;

    //UI
    public static int currentHp;
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
        skillArea = skill.transform.localScale;
        skill.SetActive(false);
        anim = GetComponent<Animator>();
        cutIn = GameObject.Find("CutIn");
        cutIn.SetActive(false);
    }

    void Update()
    {
        if(GameController.minutes >= 0 && GameController.seconds >= 0 && activeSkillSelect == false && Heart.currentHp > 0  && Spawner.zannki >= 0)
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

            if (Input.GetKeyDown(KeyCode.Space) && gauge.fillAmount >= 1)
            {
                Skill();
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
            else if(currentHp <= 0 && expLevel.Exp >= 0 && Spawner.zannki > 0)
            {
                AddExp(-50);
                ReSpawn();
            }
            hpBar.value = (float)currentHp / (float)hp;
        }
        //経験値
        if(collision.gameObject.tag == "Exp")
        {
            AddExp(1);
            Destroy(collision.gameObject);
            if (gauge.fillAmount < 1 && skillTime >= 5f)
            {
                currentGauge += addGaugeAmount;
                gauge.fillAmount = currentGauge / fullGauge;
                text.text = $"{100 * gauge.fillAmount}%";
            }
        }
        //回復
        if(collision.gameObject.tag == "Heal")
        {
            currentHp += hp / 3;
            if(currentHp > hp)
            {
                currentHp = hp;
            }
            hpBar.value = (float)currentHp / (float)hp;
            Destroy(collision.gameObject);
        }
        //スキルゲージアップ
        if(collision.gameObject.tag == "GaugeHeal")
        {
            currentGauge += fullGauge / 5;
            if(currentGauge > fullGauge)
            {
                currentGauge = fullGauge;
            }
            gauge.fillAmount = currentGauge / fullGauge;
            text.text = $"{100 * gauge.fillAmount}%";
            Destroy(collision.gameObject);
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
        skillArea += Vector2.one;
        skill.transform.localScale = skillArea;
        activeSkillSelect = false;
    }

    public void ReSpawn()
    {
        Spawner.zannki--;
        currentHp = hp;
        hpBar.value = 1;
        currentGauge = 0;
        gauge.fillAmount = 0;
        text.text = $"{100 * gauge.fillAmount}";
        transform.position = new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
    }

    void Skill()
    {
        skill.SetActive(true);
        gauge.fillAmount = 0;
        currentGauge = 0;
        text.text = $"{100 * gauge.fillAmount}%";
    }

}
