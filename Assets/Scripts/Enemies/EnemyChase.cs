using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : EnemyBase
{
    bool bIsPaused = false;

    public override void Attack()
    {
        bAttacking = true;
        if (bIsPaused) return;

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
    //        Debug.Log("Collided with player");
    //        StartCoroutine(HitPlayerPause());
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerData.instance.gameObject)
        {
            PlayerData.instance.TakeDamage(fDamage);
            StartCoroutine(HitPlayerPause());
        }
    }
}
