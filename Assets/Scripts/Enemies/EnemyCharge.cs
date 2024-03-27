using System.Collections;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.VFX;

public class EnemyCharge : EnemyBase
{
    public Vector3 v3TargetPos;
    bool bIsTurning = false;
    bool bIsPaused = false;
    
    void Update()
    {
        base.Update();
        animator.SetFloat("Blend_Speed", navMeshAgent.velocity.magnitude/3f, 0f, Time.deltaTime);
        Debug.DrawRay(navMeshAgent.transform.position, navMeshAgent.transform.forward * 10.0f, Color.cyan);
    }

    public override void Attack()
    {
        if (!bAttacking)
        {
            bWandering = false;
            StartCoroutine(Charge());
        }
    }

    private IEnumerator Charge()
    {
        if (bIsPaused) yield break;

        bAttacking = true;
        navMeshAgent.speed = fAttackingSpeed * fSlowMultiplier;

        // Prep time (IT STOMPIN ITS FEETS!)
        //Debug.Log("Preparing to charge...");
        bIsTurning = true;
        StartCoroutine(TurnTowardsPlayer());
        yield return new WaitUntil(() => !bIsTurning);

        if (!bAttacking)
        {
            yield break;
        }

        v3TargetPos = PlayerMovement.instance.transform.position;

        //////////// Uncomment below if you want the moose to pause before charging!!
        yield return new WaitForSeconds(0.5f);

        // Charge at player position
        Debug.Log("Charging now!");
        navMeshAgent.SetDestination(v3TargetPos);
        
        // Wait until charge is finished
        yield return new WaitUntil(() => Vector3.Distance(v3TargetPos, transform.position) <= 1.0f);
        navMeshAgent.ResetPath();

        bAttacking = false;
    }

    private IEnumerator TurnTowardsPlayer()
    {
        if (bIsPaused) yield break;

        RaycastHit hit;
        while (true)
        {
            Vector3 v3ChargePos = (PlayerMovement.instance.transform.position - navMeshAgent.transform.position).normalized;
            Quaternion qTargetRot = Quaternion.LookRotation(v3ChargePos);

            // Rotate towards the player
            navMeshAgent.transform.rotation = Quaternion.Lerp(navMeshAgent.transform.rotation, qTargetRot, Time.deltaTime * 5.0f);

            // Cast a ray towards the moose's forward vector
            
            if (Physics.Raycast(navMeshAgent.transform.position, navMeshAgent.transform.forward, out hit, fAggroRange * 2.0f, playerLayer))
            {
                // Break if it hits the player
                break;
            }
            else if (Physics.Raycast(navMeshAgent.transform.position, navMeshAgent.transform.forward, out hit, fAggroRange * 2.0f, playerLayer))
            {
                bAttacking = false;
                break;
            }
            else if (bWandering)
            {
                break;
            }

            yield return null;
        }

        yield return null;
        bIsTurning = false;
    }

    private IEnumerator PlayerHit()
    {
        if (bIsPaused) yield break;
        bIsPaused = true;
        bAttacking = false;

        // Stop moose and apply knockback to player
        navMeshAgent.velocity = Vector3.zero;
        navMeshAgent.ResetPath();
        
        float fKnockbackSpeed = Mathf.Max(navMeshAgent.velocity.magnitude, 10.0f);
        PlayerMovement.instance.KnockPlayerBack(fKnockbackSpeed, navMeshAgent.transform.forward);

        yield return new WaitForSeconds(1.5f);

        bIsPaused = false;
    }

    //private void OnCollisionEnter(Collision _collision)
    //{
    //    if (_collision.gameObject == PlayerData.instance.gameObject)
    //    {
    //        StopAllCoroutines();
    //        StartCoroutine(PlayerHit());
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerData.instance.gameObject)
        {
            PlayerData.instance.TakeDamage(fDamage);
            StartCoroutine(PlayerHit());
        }
    }
}