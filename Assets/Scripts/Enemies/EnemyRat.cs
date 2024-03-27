using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRat : EnemyBase
{
    private VisionCone lightCone;
    //private bool bIsRunningFromLight;

    private void Start()
    {
        base.Start();

        lightCone = PlayerMovement.instance.GetComponentInChildren<VisionCone>();
    }

    public override void Attack()
    {
        bAttacking = true;
        navMeshAgent.speed = fAttackingSpeed;

        if (!bFlashlighted)
        {
            //bIsRunningFromLight = false;
            navMeshAgent.SetDestination(PlayerMovement.instance.transform.position);
        }
        else
        {
            //Debug.Log("AAAA LIGHT!!!");
            // GET OUT OF LIGHT AAA!
            //if (!bIsRunningFromLight)
            //{
                //bIsRunningFromLight = true;
                navMeshAgent.isStopped = true;

                Vector3 v3DirAvoidLight = GetOutOfLightDirection();
                navMeshAgent.SetDestination(v3DirAvoidLight);
                navMeshAgent.isStopped = false;
            //}
        }
    }

    Vector3 GetOutOfLightDirection()
    {
        // Teddy was mathsing here say "DIE TEDDY!" because she did

        // Find the perpendicular lines from this object to each edge of vision cone
            // Edges of vision cone = lines from first and last hitpoints to player
        // Check the magnitude of each perp line and take the lesser one (closer edge)
        // Make rat go in that direction until out of light

        Vector3 v3ConeEdge1 = lightCone.hitPositions[0];
        Vector3 v3ConeEdge2 = lightCone.hitPositions[lightCone.hitPositions.Count - 1];

        // Get direction vector from player to this
        Vector3 v3PosFromPlayer = transform.position - PlayerMovement.instance.transform.position;

        // Get direction vectors of the lines (edges of vision cone)
        // From player to hitpoints
        Vector3 v3Direction1 = v3ConeEdge1 - PlayerMovement.instance.transform.position;
        Vector3 v3Direction2 = v3ConeEdge2 - PlayerMovement.instance.transform.position;

        // Calculate dot products of position vector and normalized direction vecs
        float fDotProd1 = Vector3.Dot(v3PosFromPlayer, v3Direction1.normalized);
        float fDotProd2 = Vector3.Dot(v3PosFromPlayer, v3Direction2.normalized);

        // Calculate projected vectors
        // These are the shortest distance from the rat's position to each edge of the cone
        Vector3 v3ProjectedVec1 = fDotProd1 * v3Direction1.normalized;
        Vector3 v3ProjectedVec2 = fDotProd2 * v3Direction2.normalized;

        // Compare the distances to decide which direction is closer
        if (v3ProjectedVec1.magnitude > v3ProjectedVec2.magnitude)
        {
            return v3PosFromPlayer - v3ProjectedVec1;
        }
        else
        {
            return v3PosFromPlayer - v3ProjectedVec2;
        }
    }

    private void OnCollisionStay(Collision _collision)
    {
        if (_collision.gameObject == PlayerData.instance.gameObject)
        {
            PlayerData.instance.TakeDamage(fDamage);
        }
    }
}
