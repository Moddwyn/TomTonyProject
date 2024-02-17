using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu()]
public class EnemyInfo : ScriptableObject
{
    public int health = 100;
    public int dropCost;
    public float speed = 3;
    public int damage = 5;

    [HorizontalLine]
    public AudioClip movingAudio;
    public AudioClip randomAudio;
    public AudioClip damagedAudio;
    public AudioClip deathAudio;
}
