using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SplineFollower)), RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [Expandable] public EnemyInfo enemyInfo;
    public int currentHealth;
    public Transform targetPos;

    [HorizontalLine]
    public UnityEvent OnDeath;
    [ReadOnly] public bool dead;

    SplineFollower splineFollower;
    Animator animator;

    void OnEnable()
    {
        animator = GetComponent<Animator>();
        splineFollower = GetComponent<SplineFollower>();
        splineFollower.followSpeed = enemyInfo.speed;
        currentHealth = enemyInfo.health;
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        if(currentHealth <= 0) currentHealth = 0;
        if(currentHealth <= 0 && !dead)
        {
            dead = true;

            animator.SetTrigger("Die");
            splineFollower.followSpeed = 0;

            OnDeath?.Invoke();
        }
    }
}
