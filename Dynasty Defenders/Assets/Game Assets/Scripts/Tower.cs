using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public Transform weapon;
    public Transform projectileLocation;

    [Space(20)]
    public Transform target;

    void Update() 
    {
        if(target != null)
        {
            Vector3 directionToTarget = target.position - weapon.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            weapon.rotation = targetRotation;
        }
    }
}
