using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject playerObj = null;
    public static int zannki = 3;

    [SerializeField] GameObject enemy;
    Vector3 poolPos = new Vector3(0, 0, 0);
    float rad = 0.0f;
    float timer = 0.0f;

    public ObjectPool<GameObject> objectPool;

    public static List<Enemy> eList = new List<Enemy>();

    int defaultCapacity = 10;
    int maxSize = 10;
    private void Awake()
    {
        objectPool = new ObjectPool<GameObject>(
            CreatePool,
            OnTakeFromPool,       
            OnReturnedToPool,    
            OnDestroyPoolObject,  
            true,                
            defaultCapacity,      
            maxSize

            );
    }
    
    void Start()
    {
        PlayerSpawn();

        GameManager.Instance.SetList();
        for (int i = 0; i < 5; ++i)
        {
            Spawn();
        }
    }

    void Update()
    {
        if (Heart.currentHp > 0 && zannki >= 0 && Player.currentHp > 0 && FindObjectOfType<Player>().activeSkillSelect == false)
        {
            timer += Time.deltaTime;
            if (timer > 0.1f)
            {
                var gc = FindObjectOfType<GameController>();
                for(int i = 0; i < gc.wabe; i++)
                {
                    Spawn();
                }

                timer -= 0.1f;
            }
        }
    }

    //�v�[�������s�����Ă���Ƃ��ɌĂяo�����B
    GameObject CreatePool()
    {
        var go = Instantiate(enemy, new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0), Quaternion.identity);
        var e = go.GetComponent<Enemy>();
        e.DisActiveForInstantiate();
        if (!e.isActive)
        {
            e.Create();
        }
        var pool = e;
        pool.objectPool = objectPool;
        return go;
    }

    //�v�[������I�u�W�F�N�g���擾����Ƃ��ɌĂяo�����B
    void OnTakeFromPool (GameObject go)
    {
        go.SetActive(true);
        eList.Add(go.GetComponent<Enemy>());
    }

    //�v�[������I�u�W�F�N�g����������Ƃ��Ăяo�����
    void OnReturnedToPool(GameObject go)
    {
        go.SetActive(false);
    }

    //�ő吔�𒴂�����Ăяo�����
    void OnDestroyPoolObject(GameObject go)
    {
        eList.Remove(go.GetComponent<Enemy>());
        Destroy(go);
    }

    void Spawn()
    {
        var script = objectPool.Get();
        poolPos.x = GameManager.Player.transform.position.x + 50 * Mathf.Cos(rad);
        poolPos.y = GameManager.Player.transform.position.y + 50 * Mathf.Sin(rad);
        script.transform.position = poolPos;
        rad += 1f;
    }

    void PlayerSpawn()
    {
        var px = Random.Range(-3f, 3f);
        var py = Random.Range(-3f, 3f);
        Instantiate(playerObj, new Vector2(px, py), playerObj.transform.rotation);
    }

    
}
