using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : EnemyBase
{
    public override void Attack()
    {
        bAttacking = true;
        navMeshAgent.speed = fAttackingSpeed * fSlowMultiplier;
        navMeshAgent.SetDestination(PlayerMovement.instance.transform.position);

        CheckDamagePlayer();
    }
}
