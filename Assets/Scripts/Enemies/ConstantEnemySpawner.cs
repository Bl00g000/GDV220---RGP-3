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
        GetComponent<Renderer>().enabled = false;

        foreach (Transform child in transform)
        {
            // Set spawn point and disable sprite renderer
            if (child.GetComponent<SpriteRenderer>() != null)
            {
                spawnPoint = child.position;
                child.GetComponent<SpriteRenderer>().enabled = false;
                continue;
            }

            tendrils.Add(child.GetComponent<EnemyTendril>());
        }

        transform.GetChild(0).GetComponent<Renderer>().enabled = false;

        ////////// UNCOMMENT BELOW TO DISABLE CUBE ON START
        //transform.GetComponent<Renderer>().enabled = false; 
    }

    // Update is called once per frame
    void Update()
    {
        CheckTendrilsDeath();

        if (!bIsSpawning)
        {
            StartCoroutine(SpawnEnemies());
        }
    }

    private IEnumerator SpawnEnemies()
    {
        if (enemiesToSpawn.Count == 0) yield break;

        bIsSpawning = true;

        // Child count - num tendrils - 1 (spawn point)
        while (transform.childCount - tendrils.Count - 1 < iMaxNumSpawns)
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
            if (child.gameObject.GetComponent<EnemyTendril>() != null)
            {
                tendrils.Add(child.gameObject.GetComponent<EnemyTendril>());
            }
        }

        // If no tendrils found, destroy spawner
        if (tendrils.Count <= 0)
        {
            // Unparent all spawns so they don't die when we destroy this
            foreach (Transform child in transform)
            {
                if (child.gameObject.GetComponent<EnemyBase>() != null)
                {
                    child.SetParent(null);
                }
            }

            Destroy(gameObject);
        }
    }
}
