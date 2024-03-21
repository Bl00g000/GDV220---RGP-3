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
    [Tooltip("Enemies in this list will spawn at the positions of the child spawn point with a matching index.")]
    [SerializeField] private List<GameObject> enemiesToSpawn;
    private List<Vector3> spawnPoints = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        // Disable the sprite renderer on start
        //GetComponent<Renderer>().enabled = false;

        if (transform.childCount > 0 && enemiesToSpawn.Count > 0)
        {
            // Add spawn point locations here
            foreach (Transform child in transform)
            {
                spawnPoints.Add(child.position);
                child.GetComponent<Renderer>().enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (enemiesToSpawn.Count == 0) return;

        // Make sure its the player entering
        if (other.gameObject == PlayerMovement.instance.gameObject)
        {
            // Spawn enemies at spawn points
            for (int i = 0; i < spawnPoints.Count; i++)
            {
                Instantiate(enemiesToSpawn[i], spawnPoints[i], Quaternion.identity);
            }
        }

        // Disable the trigger so it can only be triggered once
        GetComponent<Collider>().enabled = false;
    }
}