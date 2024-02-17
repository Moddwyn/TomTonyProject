using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public TowerInfo towerInfo;
    public Transform weapon;
    public Transform projectileLocation;

    [HorizontalLine]
    [ReadOnly] public Transform target;

    void Update() 
    {
        if(target != null && weapon != null)
        {
            Vector3 directionToTarget = target.position - weapon.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            weapon.rotation = targetRotation;
        }
    }
}
