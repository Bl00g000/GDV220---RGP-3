using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

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

    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected LayerMask wallLayer;


    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

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

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, fAggroRange);
    }

    // Update is called once per frame
    protected void Update()
    {
      
        if (bFlashlighted)
        {
            fSlowMultiplier = 0.2f;
            Debug.Log("flashlighted - attack");

            // Aggro enemies when flashlighted?!
            if (!bAttacking)
            {
                Attack();
            }
        }
        else
        {
            fSlowMultiplier = 1.0f;
        }

        if (PlayerMovement.instance.gameObject.activeSelf)
        {
            // Check if player is inside aggro range and there is a visible line between the enemy and player
            if (Vector3.Distance(transform.position, PlayerMovement.instance.transform.position) < fAggroRange && HasLOS())
            {
                if (bWandering)
                {
                    bWandering = false;
                    StopCoroutine(Wander());
                    navMeshAgent.ResetPath();
                }

                Attack();
            }
            else
            {
                bAttacking = false;
            }
        }
        else
        {
            bAttacking = false;
        }

        if (!bWandering && !bAttacking && !HasLOS())
        {
            StartCoroutine(Wander());
        }

        bFlashlighted = false;

        if (animator)
        {
            if (bWandering)
            {
                animator.SetFloat("Blend_Speed", 1f, 0.2f, Time.deltaTime);
            }
            else
            {
                animator.SetFloat("Blend_Speed", 0f, 0.2f, Time.deltaTime);
            }
            
        }
    }

    public virtual void Attack()
    {
        // Function to be overriden by child classes
        // depending on how enemy attacks
    }

    protected void CheckDamagePlayer()
    {
        float fDistFromPlayer = Vector3.Distance(transform.position, PlayerMovement.instance.transform.position);
        if (fDistFromPlayer <= 1.0f)
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

    protected bool HasLOS()
    {
        RaycastHit playerHit;
        RaycastHit wallHit;

        Vector3 v3DirectionToPlayer = (PlayerMovement.instance.transform.position - navMeshAgent.transform.position).normalized;

        // cast 2 rays, one that checks for player and one that checks for walls, if there is a wall between the moose and the player
        if (Physics.Raycast(navMeshAgent.transform.position, v3DirectionToPlayer, out playerHit, fAggroRange * 2.0f, playerLayer))
        {
            if (Physics.Raycast(navMeshAgent.transform.position, v3DirectionToPlayer, out wallHit, fAggroRange * 2.0f, wallLayer))
            {
                if (Vector3.Distance(transform.position, playerHit.point) > Vector3.Distance(transform.position, wallHit.point))
                {
                    return false;
                }
            }
            return true;
        }

        return false;
    }
}
