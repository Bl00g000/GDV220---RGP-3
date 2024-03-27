using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class EnemyBase : MonoBehaviour
{
    // THIS IS THE NON-ENEMY - IT ONLY WANDERS!!

    protected NavMeshAgent navMeshAgent;

    public float fMaxHealth;
    public float fHealth;
    public float fDamage;
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


    protected Animator animator;
    private VisualEffect visualEffect;

    // event

    public event Action<float> OnDamageTaken;

    protected void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        visualEffect = GetComponentInChildren<VisualEffect>();
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

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(transform.position, fAggroRange);
    //}

    // Update is called once per frame
    protected void Update()
    {
        if (bFlashlighted)
        {
            if (visualEffect) visualEffect.enabled = true;
            fSlowMultiplier = 0.2f;
            Debug.Log("flashlighted - attack");

            // Aggro enemies when flashlighted?!
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
            fSlowMultiplier = 1.0f;
            if (visualEffect) visualEffect.enabled = false;
        }

        if (PlayerMovement.instance.gameObject.activeSelf)
        {
            // Check if player is inside aggro range and there is a visible line between the enemy and player
            if (Vector3.Distance(transform.position, PlayerMovement.instance.transform.position) < fAggroRange && HasLOS())
            {
                // If within aggro range, stop wandering
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

        if (!Flashlight.instance.pointsToPlane.LightContainsObject(gameObject) || !Flashlight.instance.bFlashLightActive)
        {
            bFlashlighted = false;
        }
        
        if (animator)
        {
            animator.SetFloat("Blend_Speed", navMeshAgent.velocity.magnitude/3f, 0f, Time.deltaTime);
        }
    }
    public virtual void Attack()
    {
        // Function to be overriden by child classes
        // depending on how enemy attacks
    }

    //protected void CheckDamagePlayer()
    //{
    //
    //    float fDistFromPlayer = Vector3.Distance(transform.position, PlayerMovement.instance.transform.position);
    //    if (fDistFromPlayer <= 1.0f)
    //    {
    //        // DEAL DAMAGE HERE
    //        Debug.Log("DEATH TO THE PLAYER!");
    //
    //        PlayerData.instance.TakeDamage(fDamage);
    //       // PlayerMovement.instance.gameObject.SetActive(false);
    //    }
    //}

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
        if (Physics.Raycast(navMeshAgent.transform.position, v3DirectionToPlayer, out playerHit, fAggroRange * 5.0f, playerLayer))
        {
            if (Physics.Raycast(navMeshAgent.transform.position, v3DirectionToPlayer, out wallHit, fAggroRange * 5.0f, wallLayer))
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

    public void TakeDamage(float _fDamage)
    {
        fHealth -= _fDamage;
<<<<<<< Updated upstream

        // THE ENEMIES DIE NOW AND SO DOES TEDDY
        if (fHealth <= 0)
        {
            Destroy(gameObject);
=======
        if (_fDamage > 0)
        {
            OnDamageTaken?.Invoke(_fDamage);
>>>>>>> Stashed changes
        }
    }
}
