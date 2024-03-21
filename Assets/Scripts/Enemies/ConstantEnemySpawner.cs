using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantEnemySpawner : MonoBehaviour
{
    public int iMaxNumSpawns;
    [SerializeField] private List<GameObject> enemiesToSpawn;
    private Vector3 spawnPoint;

    bool bIsSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = transform.GetChild(0).transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!bIsSpawning)
        {
            StartCoroutine(SpawnEnemies());
        }
    }

    private IEnumerator SpawnEnemies()
    {
        bIsSpawning = true;

        // Child count - 1 because spawn point is a child
        while (transform.childCount - 1 < iMaxNumSpawns)
        {
            Debug.Log("Spawning enemy");
            int iEnemySpawning = Random.Range(0, enemiesToSpawn.Count - 1);
            var enemySpawned = Instantiate(enemiesToSpawn[iEnemySpawning], spawnPoint, transform.rotation);
            enemySpawned.transform.SetParent(transform);

            yield return new WaitForSeconds(1.0f);
        }

        bIsSpawning = false;
        yield return null;
    }
}
