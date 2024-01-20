using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileShoot : MonoBehaviour
{
    public GameObject projectilePrefab; // Assign your prefab in the Inspector
    public float moveSpeed = 5f;        // Speed at which the object moves towards the target
    public float shootSpeed = 1.5f;

    public UnityEvent OnShoot;

    public void Shoot(Transform target, Vector3 spawnLocation)
    {
        GameObject spawnedObject = Instantiate(projectilePrefab, spawnLocation, Quaternion.identity);
        StartCoroutine(MoveObject(spawnedObject.transform, target, moveSpeed));

        OnShoot?.Invoke();
    }

    IEnumerator MoveObject(Transform obj, Transform target, float speed)
    {
        Vector3 directionToTarget = target.position - obj.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        obj.rotation = targetRotation;
        while (obj.transform.position != target.position)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target.position, speed * Time.deltaTime);
            yield return null;
        }
    }
}
