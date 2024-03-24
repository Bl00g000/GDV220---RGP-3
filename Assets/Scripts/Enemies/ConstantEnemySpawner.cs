using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ConstantEnemySpawner : MonoBehaviour
{
    public int iMaxNumSpawns;
    [SerializeField] private List<GameObject> enemiesToSpawn;
    private Vector3 spawnPoint;
    private List<EnemyBase> tendrils = new List<EnemyBase>();

    bool bIsSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = transform.GetChild(0).transform.position;
        transform.GetChild(0).GetComponent<Renderer>().enabled = false;

        ////////// UNCOMMENT BELOW TO DISABLE CUBE ON START
        //transform.GetComponent<Renderer>().enabled = false; 
    }

    // Update is called once per frame
    void Update()
    {
        if (!bIsSpawning)
        {
            StartCoroutine(SpawnEnemies());
        }

        CheckTendrilsDeath();
    }

    private IEnumerator SpawnEnemies()
    {
        if (enemiesToSpawn.Count == 0) yield break;

        bIsSpawning = true;

        // Child count - 1 because spawn point is a child
        while (transform.childCount - 1 < iMaxNumSpawns)
        {
            int iEnemySpawning = Random.Range(0, enemiesToSpawn.Count);     // int - max exclusive
            var enemySpawned = Instantiate(enemiesToSpawn[iEnemySpawning], spawnPoint, transform.rotation);
            enemySpawned.transform.SetParent(transform);

            yield return new WaitForSeconds(2.0f);
        }

        bIsSpawning = false;
        yield return null;
    }

    private void CheckTendrilsDeath()
    {
        tendrils.Clear();

        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<EnemyBase>() != null)
            {
                tendrils.Add(child.gameObject.GetComponent<EnemyBase>());
            }
        }

        // If no tendrils found, destroy spawner
        if (tendrils.Count <= 0)
        {
            Destroy(gameObject);
        }
    }
}
