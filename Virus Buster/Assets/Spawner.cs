using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject playerObj = null;

    [SerializeField] GameObject enemy;
    Vector3 poolPos = new Vector3(0, 0, 0);
    float rad = 0.0f;
    float timer = 0.0f;


    ObjectPool<GameObject> objectPool;

    
    public static List<Enemy> eList = new List<Enemy>();

    int defaultCapacity = 100;
    int maxSize = 100;


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

        SetCapacity(defaultCapacity);
        GameManager.Instance.SetList();
        for (int i = 0; i < 5; ++i)
        {
            Spawn();
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 0.05f)
        {
            Spawn();
            timer -= 0.05f;
        }
    }

    GameObject CreatePool()
    {
        GameObject go = default;
        for (int i = 0; i < eList.Count; i++)
        {
            int index = i % eList.Count;
            if (eList[index].isActive) continue;

            go = Instantiate(enemy, new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0), Quaternion.identity);
            eList[index] = go.GetComponent<Enemy>();
            eList[index].DisActiveForInstantiate();

            eList[index].Create();
            var pool = eList[index];
            pool.objectPool = objectPool;
            break;
        }

        return go;
    }

    void OnTakeFromPool (GameObject go)
    {
        go.SetActive(false);
    }
    void OnReturnedToPool(GameObject go)
    {
        go.SetActive(true);
    }
    void OnDestroyPoolObject(GameObject go)
    {
        Destroy(go);
        eList.RemoveAt(eList.Count-1);
    }

    void Spawn()
    {
        if(CreatePool() != null && GameManager.Heart)
        {
            var script = CreatePool();
            poolPos.x = GameManager.Heart.transform.position.x + 100 * Mathf.Cos(rad);
            poolPos.y = GameManager.Heart.transform.position.y + 100 * Mathf.Sin(rad);
            //Debug.Log(poolPos);
            script.transform.position = poolPos;
            rad += 0.1f;
        }
    }

    void PlayerSpawn()
    {
        var px = Random.Range(-3f, 3f);
        var py = Random.Range(-3f, 3f);
        Instantiate(playerObj, new Vector2(px, py), playerObj.transform.rotation);
    }

    void SetCapacity(int size)
    {
        if (size < eList.Count) return;
        for(int i = eList.Count - 1; i < size; i++)
        {
            Enemy obj = enemy.GetComponent<Enemy>();
            //obj.DisActiveForInstantiate();
            eList.Add(obj);
        }
    }
}
