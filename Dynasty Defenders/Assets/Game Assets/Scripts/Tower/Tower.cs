using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Tower : MonoBehaviour
{
    [Required][Expandable] public TowerInfo towerInfo;

    [HorizontalLine]
    [BoxGroup("Shooting")] public float shootRate = 1.5f;
    [BoxGroup("Shooting")] public float checkRadius = 5f;
    [BoxGroup("Shooting")] public LayerMask layersToTarget;
    [BoxGroup("Shooting")] public UnityEvent OnShoot;

    [HorizontalLine]
    [BoxGroup("Projectile")][Min(0)] public int damage = 20;
    [BoxGroup("Projectile")]public bool movingProjectile;
    [BoxGroup("Projectile")][ShowIf("MovingProjectile")][Tag] public string poolTag;
    [BoxGroup("Projectile")][ShowIf("MovingProjectile")] public float moveSpeed = 5f;

    [HorizontalLine]
    [BoxGroup("Weapon")] public bool rotates;
    [BoxGroup("Weapon")][ShowIf("IsRotating")] public Transform weapon;
    [BoxGroup("Weapon")][Required] public Transform projectileLocation;

    [HorizontalLine]
    [BoxGroup("Debug")][ReadOnly] public Transform target;

    ObjectPooler projectilePool;
    AudioSource audioSource;

    void OnEnable() 
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(towerInfo.OnPlaceAudio);
        if(MovingProjectile())
            projectilePool = GameObject.FindGameObjectWithTag(poolTag).GetComponent<ObjectPooler>();

        StartCoroutine(CheckForObjects());
    }

    void Update() 
    {
        RotateUpdate();
    }

    void RotateUpdate()
    {
        if(target != null && IsRotating())
        {
            Vector3 directionToTarget = target.position - weapon.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            weapon.rotation = targetRotation;
        }
    }

    IEnumerator CheckForObjects()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRadius, layersToTarget).Where(x=>!x.GetComponent<Enemy>().dead).ToArray();
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
            target = closestPosition.GetComponent<Enemy>().targetPos;
            Shoot(target, projectileLocation.position);
        } else target = null;

        yield return new WaitForSeconds(shootRate);
        StartCoroutine(CheckForObjects());
    }

    public void Shoot(Transform target, Vector3 spawnLocation)
    {
        if(MovingProjectile()) ShootMoving(target, spawnLocation);
        else HitEnemy(null, target.GetComponentInParent<Enemy>());

        if(towerInfo != null && towerInfo.OnFireAudio != null)
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

        if(MovingProjectile())
            projectilePool.InsertToPool(obj);
    }


    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }

    bool IsRotating() => rotates;
    bool MovingProjectile() => movingProjectile;
}
