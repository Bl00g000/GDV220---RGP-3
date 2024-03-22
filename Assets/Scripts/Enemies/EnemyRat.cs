using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRat : EnemyBase
{
    private VisionCone lightCone;

    private void Start()
    {
        base.Start();

        lightCone = PlayerMovement.instance.GetComponent<VisionCone>();
    }

    public override void Attack()
    {
        bAttacking = true;
        navMeshAgent.speed = fAttackingSpeed * fSlowMultiplier;

        if (!bFlashlighted)
        {
            navMeshAgent.SetDestination(PlayerMovement.instance.transform.position);
        }
        else
        {
            // GET OUT OF LIGHT AAA!
            
            // Find the perpendicular lines from this object to each edge of vision cone
                // Edges of vision cone = lines from first and last hitpoints to player
                // Normalize????? probably
            // Check the magnitude of each perp line and take the lesser one (closer edge)
            // Make rat go in that direction until out of light
        }

        CheckDamagePlayer();
    }
}
