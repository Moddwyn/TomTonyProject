using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileShoot : MonoBehaviour
{
    public string poolTag;
    public float moveSpeed = 5f;
    public float shootSpeed = 1.5f;
    public int damage = 20;

    public UnityEvent OnShoot;

    ObjectPooler projectilePool;
    void OnEnable() 
    {
        projectilePool = GameObject.FindGameObjectWithTag(poolTag).GetComponent<ObjectPooler>();
    }

    public void Shoot(Transform target, Vector3 spawnLocation)
    {
        GameObject spawnedObject = projectilePool.GrabFromPool(spawnLocation, Quaternion.identity);
        StartCoroutine(MoveObject(spawnedObject.transform, target, moveSpeed));

        OnShoot?.Invoke();
    }

    IEnumerator MoveObject(Transform obj, Transform target, float speed)
    {
        Vector3 directionToTarget = target.position - obj.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        obj.rotation = targetRotation;
        while (Vector3.Distance(obj.transform.position, target.position) > 0.1f)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target.position, speed * Time.deltaTime);
            yield return null;
        }
        HitEnemy(obj.gameObject, target.GetComponentInParent<Enemy>());
    }

    void HitEnemy(GameObject obj, Enemy target)
    {
        target.ChangeHealth(-damage);
        projectilePool.InsertToPool(obj);
    }
}
