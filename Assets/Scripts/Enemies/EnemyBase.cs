using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;

    public float fHealth;
    public float fWanderSpeed;
    public float fAttackSpeed;
    public float fAggroRange;

    Vector3 v3StartingPosition;
    [SerializeField] float fWanderRadius = 20.0f;

    // States
    bool bWandering;
    bool bAttacking;

    // Start is called before the first frame update
    void Start()
    {
        bWandering = false;
        bAttacking = false;

        v3StartingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!bWandering && !bAttacking)
        {
            StartCoroutine(Wander());
        }

        if (PlayerMovement.instance.gameObject.activeSelf)
        {
            if (Vector3.Distance(transform.position, PlayerMovement.instance.transform.position) < fAggroRange)
            {
                if (bWandering)
                {
                    bWandering = false;
                }

                Attack();
            }
        }
        else
        {
            bAttacking = false;
        }
    }

    // Basic run-attack
    // Function to be overriden by child classes
    // depending on how enemy attacks
    public virtual void Attack()
    {
        bAttacking = true;
        navMeshAgent.speed = fAttackSpeed;
        navMeshAgent.SetDestination(PlayerMovement.instance.transform.position);

        float fDistFromPlayer = Vector3.Distance(transform.position, PlayerMovement.instance.transform.position);
        if (fDistFromPlayer < 1.0f)
        {
            // DEAL DAMAGE HERE
            Debug.Log("DEATH TO THE PLAYER!");
            PlayerMovement.instance.gameObject.SetActive(false);
        }
    }

    protected Vector3 FindNewWanderPos()
    {
        Vector3 v3MinPos = v3StartingPosition - new Vector3(fWanderRadius, 0.0f, fWanderRadius);
        Vector3 v3MaxPos = v3StartingPosition + new Vector3(fWanderRadius, 0.0f, fWanderRadius);

        Vector3 v3NewTarget = new Vector3(Random.Range(v3MinPos.x, v3MaxPos.x), 0.0f, Random.Range(v3MinPos.z, v3MaxPos.z));

        return v3NewTarget;
    }

    protected IEnumerator Wander()
    {
        // Set new position to wander to
        navMeshAgent.speed = fWanderSpeed;
        Vector3 v3WanderPos = FindNewWanderPos();
        navMeshAgent.SetDestination(v3WanderPos);
        bWandering = true;
        
        // Wait until path has been completed -- DIFFERENT FROM PATHSTATUS!!!
        yield return new WaitUntil(() => !navMeshAgent.hasPath);

        // Variable delay/pause before it wanders again
        yield return new WaitForSeconds(Random.Range(3.0f, 5.0f));

        bWandering = false;
    }
}
