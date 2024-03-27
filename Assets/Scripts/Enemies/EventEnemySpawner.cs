using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventEnemySpawner : MonoBehaviour
{
    // TEDDY'S NOTE:
    // Enemies in the list will spawn in the position of matching index!
    // So make sure that the order of enemies in the list matches
    // the order of the CHILDREN under this object if you want specific
    // enemies to spawn at specific spawn points
    [Tooltip("Enemies in this list will spawn at the positions of the child spawn point with a matching index.\n" +
        "If uneven sizes, the smaller list will wrap around to fulfil the bigger list.")]
    [SerializeField] private List<GameObject> enemiesToSpawn;
    private List<GameObject> spawnPoints = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // Disable the sprite renderer on start
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;       // Collider disabled until camera pickup

        if (transform.childCount > 0)
        {
            // Add spawn point locations here
            foreach (Transform child in transform)
            {
                spawnPoints.Add(child.gameObject);

                // Disable renderer and nose
                child.GetComponent<Renderer>().enabled = false;
                child.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (PlayerData.instance.cameraWeapon.bHasCamera)
        {
            if (!GetComponent<Collider>().enabled)
            {
                GetComponent<Collider>().enabled = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (enemiesToSpawn.Count == 0) return;

        // Make sure its the player entering
        if (other.gameObject == PlayerMovement.instance.gameObject)
        {
            // Wrap enemy indices around to spawn them at all locations
            // Useful for if we want to spawn only 1 or 2 kinds of enemy in multiple locations
            if (spawnPoints.Count > enemiesToSpawn.Count)
            {
                int iEnemiesIndex = 0;

                // Spawn enemies at spawn points
                for (int i = 0; i < spawnPoints.Count; i++)
                {
                    Instantiate(enemiesToSpawn[iEnemiesIndex], spawnPoints[i].transform.position, spawnPoints[i].transform.rotation);
                    iEnemiesIndex++;

                    // Reset enemy index
                    if (iEnemiesIndex > enemiesToSpawn.Count - 1)
                    {
                        iEnemiesIndex = 0;
                    }
                }
            }
            // Wrap spawn point indices around to spawn all enemies
            // Useful for if we want to spawn multiple enemies in 1 or 2 locations
            else if(enemiesToSpawn.Count > spawnPoints.Count)
            {
                int iSpawnIndex = 0;

                // Spawn enemies at spawn points
                for (int i = 0; i < enemiesToSpawn.Count; i++)
                {
                    Instantiate(enemiesToSpawn[i], spawnPoints[iSpawnIndex].transform.position, spawnPoints[iSpawnIndex].transform.rotation);
                    iSpawnIndex++;

                    // Reset spawn index
                    if (iSpawnIndex > spawnPoints.Count - 1)
                    {
                        iSpawnIndex = 0;
                    }
                }
            }
            else
            {
                // Spawn enemies at spawn points
                for (int i = 0; i < enemiesToSpawn.Count; i++)
                {
                    Instantiate(enemiesToSpawn[i], spawnPoints[i].transform.position, spawnPoints[i].transform.rotation);
                }
            }

            // Disable the trigger so it can only be triggered once
            Destroy(gameObject);
        }
    }
}