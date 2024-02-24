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
    public float disappearTime = 4;
    public UnityEvent OnDeath;
    [ReadOnly] public bool dead;

    [HideInInspector] public SplineFollower splineFollower;
    [HideInInspector] public ObjectPooler pool;
    Animator animator;

    void OnEnable()
    {
        dead = false;

        animator = GetComponent<Animator>();
        splineFollower = GetComponent<SplineFollower>();

        splineFollower.SetPercent(0);
        splineFollower.followSpeed = enemyInfo.speed;
        currentHealth = enemyInfo.health;

        StartCoroutine(StartDoingDamage());
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
            CoinsManager.Instance.ChangeCoins(enemyInfo.coinDropAmount);
            
            OnDeath?.Invoke();
            StartCoroutine(Disappear());
        }
    }

    IEnumerator StartDoingDamage()
    {
        yield return new WaitForSeconds(enemyInfo.damageRate);
        if(splineFollower.result.percent >= 0.99f)
            Health.Instance.ChangeHealth(-enemyInfo.damage);

        StartCoroutine(StartDoingDamage());
    }

    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(disappearTime);
        pool.InsertToPool(gameObject);

        gameObject.transform.position = Vector3.zero;
        if(TryGetComponent<Rigidbody>(out Rigidbody rb)) rb.velocity = Vector3.zero;
    }
}
