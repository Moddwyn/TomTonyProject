using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BasketballPointCounter : MonoBehaviour
{
    public int points;
    public TMP_Text pointsText;
    public AudioClip netClip;
    public AudioClip scoreClip;

    void Update()
    {
        pointsText.text = "Points: " + points;
    }

    
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ball")
        {
            GameManager.Instance.audioSource.PlayOneShot(netClip);
            GameManager.Instance.audioSource.PlayOneShot(scoreClip);
            points++;
        }
    }
}
