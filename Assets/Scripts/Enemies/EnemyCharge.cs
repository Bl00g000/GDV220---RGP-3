using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharge : EnemyBase
{
    public Transform playerTransform;

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
        navMeshAgent.speed = fAttackSpeed;

        Vector3 v3ChargeTargetPos = playerTransform.position;
        navMeshAgent.SetDestination(v3ChargeTargetPos);

        // I want to first check if it's finished turning around to face player
        // before pausing its movement along path for prep time
        // BUT I HAVENT FIGURED THIS OUT YET - Teddy
        navMeshAgent.isStopped = true;

        // Prep time (IT STOMPIN ITS FEETS!)
        Debug.Log("Preparing to charge...");
        yield return new WaitForSeconds(3.0f);

        navMeshAgent.isStopped = false;

        // Wait until charge is finished
        yield return new WaitUntil(() => navMeshAgent.remainingDistance <= 1.0f);
        navMeshAgent.ResetPath();

        //yield return new WaitForSeconds(3.0f);
        bAttacking = false;
    }
}
