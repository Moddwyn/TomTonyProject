using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBounceSound : MonoBehaviour
{
    public AudioClip bounceClip;
    void OnCollisionEnter(Collision other)
    {
        GameManager.Instance.audioSource.PlayOneShot(bounceClip);
    }
}
