using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharge : EnemyBase
{
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

        // Prep time (IT STOMPIN ITS FEETS!)
        Debug.Log("Preparing to charge...");
        yield return new WaitForSeconds(2.0f);

        // Charge at player position
        Debug.Log("Charging now!");
        Vector3 v3ChargeTargetPos = PlayerMovement.instance.transform.position;
        navMeshAgent.SetDestination(v3ChargeTargetPos);

        // Wait until charge is finished
        yield return new WaitUntil(() => navMeshAgent.remainingDistance <= 1.0f);

        navMeshAgent.ResetPath();
        bAttacking = false;
    }
}
