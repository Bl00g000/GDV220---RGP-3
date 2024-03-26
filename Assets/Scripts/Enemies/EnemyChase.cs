using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : EnemyBase
{
    bool bIsPaused = false;

    public override void Attack()
    {
        if (bIsPaused) return;

        bAttacking = true;
        navMeshAgent.speed = fAttackingSpeed * fSlowMultiplier;
        navMeshAgent.SetDestination(PlayerMovement.instance.transform.position);
    }

    private IEnumerator HitPlayerPause()
    {
        if (bIsPaused) yield break;

        bAttacking = false;
        bIsPaused = true;

        navMeshAgent.isStopped = true;
        navMeshAgent.ResetPath();

        yield return new WaitForSeconds(1.0f);

        bIsPaused = false;
        // Not sure if we need to reset destination after
        //navMeshAgent.SetDestination(PlayerMovement.instance.transform.position);
    }

    //private void OnCollisionEnter(Collision _collision)
    //{
    //    if (_collision.gameObject == PlayerData.instance.gameObject)
    //    {
    //        StartCoroutine(HitPlayerPause());
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerData.instance.gameObject)
        {
            StartCoroutine(HitPlayerPause());
        }
    }
}
