using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharge : EnemyBase
{
    [SerializeField] LayerMask detectedLayers;
    bool bIsTurning = false;
    public Vector3 v3TargetPos;

    void Update()
    {
        base.Update();
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
        bAttacking = true;
        navMeshAgent.speed = fAttackingSpeed * fSlowMultiplier;

        // Prep time (IT STOMPIN ITS FEETS!)
        //Debug.Log("Preparing to charge...");
        bIsTurning = true;
        StartCoroutine(TurnTowardsPlayer());
        yield return new WaitUntil(() => !bIsTurning);

        v3TargetPos = PlayerMovement.instance.transform.position;

        //////////// Uncomment below if you want the moose to pause before charging!!
        //yield return new WaitForSeconds(0.5f);

        // Charge at player position
        Debug.Log("Charging now!");
        navMeshAgent.SetDestination(v3TargetPos);

        // Wait until charge is finished
        yield return new WaitUntil(() => Vector3.Distance(v3TargetPos, transform.position) <= 1.0f);
        navMeshAgent.ResetPath();

        CheckDamagePlayer();

        bAttacking = false;
    }

    private IEnumerator TurnTowardsPlayer()
    {
        RaycastHit hit;
        
        while (true)
        {
            Vector3 v3ChargePos = (PlayerMovement.instance.transform.position - navMeshAgent.transform.position).normalized;
            Quaternion qTargetRot = Quaternion.LookRotation(v3ChargePos);

            // Rotate towards the player
            navMeshAgent.transform.rotation = Quaternion.Lerp(navMeshAgent.transform.rotation, qTargetRot, Time.deltaTime * 5.0f);

            // Cast a ray towards the moose's forward vector
            if (Physics.Raycast(navMeshAgent.transform.position, navMeshAgent.transform.forward, out hit, fAggroRange * 2.0f, detectedLayers))
            {
                // Break if it hits the player
                if (hit.collider.gameObject == PlayerMovement.instance.gameObject) break;
            }

            yield return null;
        }

        yield return null;
        bIsTurning = false;
    }
}
