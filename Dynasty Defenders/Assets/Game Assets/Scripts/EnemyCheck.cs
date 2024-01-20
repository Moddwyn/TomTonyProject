using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCheck : MonoBehaviour
{
    public ProjectileShoot projectileShoot;
    public Tower tower;
    
    [Space(20)]
    public float checkRadius = 5f; // Radius of the sphere
    public LayerMask layerMask;    // Layer mask to filter for specific layer
    

    void Start()
    {
        StartCoroutine(CheckForObjects());
    }

    IEnumerator CheckForObjects()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRadius, layerMask);
        Collider closestCollider = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (var hitCollider in hitColliders)
        {
            Vector3 directionToTarget = hitCollider.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestCollider = hitCollider;
            }
            yield return null;
        }

        if (closestCollider != null)
        {
            Transform closestPosition = closestCollider.transform;
            tower.target = closestPosition;
            projectileShoot.Shoot(closestPosition, tower.projectileLocation.position);
        } else tower.target = null;

        yield return new WaitForSeconds(projectileShoot.shootSpeed);
        StartCoroutine(CheckForObjects());
    }


    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
