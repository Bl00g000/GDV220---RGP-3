using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    // THIS IS THE NON-ENEMY - IT ONLY WANDERS!!

    protected NavMeshAgent navMeshAgent;

    public float fMaxHealth;
    public float fHealth;
    public float fWanderSpeed;
    public float fAttackingSpeed;
    public float fAggroRange;
    [HideInInspector] public float fSlowMultiplier;

    Vector3 v3StartingPosition;
    [SerializeField] float fWanderRadius = 20.0f;

    // States
    protected bool bWandering;
    protected bool bAttacking;
    public bool bFlashlighted;

    // Start is called before the first frame update
    protected void Start()
    {
        bWandering = false;
        bAttacking = false;
        bFlashlighted = false;
        fSlowMultiplier = 1.0f;

        navMeshAgent = GetComponent<NavMeshAgent>();
        v3StartingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (bFlashlighted)
        {
            fSlowMultiplier = 0.2f;
        }
        else
        {
            fSlowMultiplier = 1.0f;
        }

        if (PlayerMovement.instance.gameObject.activeSelf)
        {
            // Check if player is inside aggro range
            if (Vector3.Distance(transform.position, PlayerMovement.instance.transform.position) < fAggroRange)
            {
                if (bWandering)
                {
                    bWandering = false;
                    StopCoroutine(Wander());
                    navMeshAgent.ResetPath();
                }

                Attack();
            }
        }
        else
        {
            bAttacking = false;
        }

        if (!bWandering && !bAttacking && Vector3.Distance(transform.position, PlayerMovement.instance.transform.position) > fAggroRange * 2)
        {
            StartCoroutine(Wander());
        }

        bFlashlighted = false;
    }

    public virtual void Attack()
    {
        // Function to be overriden by child classes
        // depending on how enemy attacks
    }

    protected void CheckDamagePlayer()
    {
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
        // Set wander speed and position
        navMeshAgent.speed = fWanderSpeed * fSlowMultiplier;

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