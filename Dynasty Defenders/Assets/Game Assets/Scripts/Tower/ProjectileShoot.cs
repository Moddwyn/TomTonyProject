using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class ProjectileShoot : MonoBehaviour
{
    public bool moving;
    public float shootSpeed = 1.5f;
    [ShowIf("IsMoving")] public string poolTag;
    [ShowIf("IsMoving")] public float moveSpeed = 5f;
    public int damage = 20;

    public UnityEvent OnShoot;

    ObjectPooler projectilePool;
    AudioSource audioSource;

    void OnEnable() 
    {
        audioSource = GetComponent<AudioSource>();
        if(IsMoving())
            projectilePool = GameObject.FindGameObjectWithTag(poolTag).GetComponent<ObjectPooler>();
    }

    public void Shoot(Transform target, Vector3 spawnLocation, TowerInfo towerInfo)
    {
        if(IsMoving()) ShootMoving(target, spawnLocation);
        else HitEnemy(null, target.GetComponentInParent<Enemy>());

        if(towerInfo != null)
            audioSource.PlayOneShot(towerInfo.OnFireAudio);
        OnShoot?.Invoke();
    }

    void ShootMoving(Transform target, Vector3 spawnLocation)
    {
        GameObject spawnedObject = projectilePool.GrabFromPool(spawnLocation, Quaternion.identity);
        StartCoroutine(MoveObject(spawnedObject.transform, target, moveSpeed));
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

        if(IsMoving())
            projectilePool.InsertToPool(obj);
    }

    public bool IsMoving()=>moving;
}
