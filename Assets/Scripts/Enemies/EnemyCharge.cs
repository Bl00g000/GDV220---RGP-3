using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharge : EnemyBase
{
    public override void Attack()
    {
        if (!bAttacking)
        {
            bAttacking = true;
            StartCoroutine(Charge());
        }
    }

    private IEnumerator Charge()
    {
        navMeshAgent.speed = fAttackSpeed;

        //navMeshAgent.isStopped = true;

        Debug.Log("Preparing to charge...");
        // Prep time (IT STOMPIN ITS FEETS!)
        yield return new WaitForSeconds(3.0f);

        navMeshAgent.SetDestination(PlayerMovement.instance.transform.position);
        //navMeshAgent.isStopped = false;

        // Wait until charge is finished
        yield return new WaitUntil(() => !navMeshAgent.hasPath);

        //yield return new WaitForSeconds(3.0f);

        bAttacking = false;
    }
}
