using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BasketballPointCounter : MonoBehaviour
{
    public int points;
    public TMP_Text pointsText;

    void Update()
    {
        pointsText.text = "Points: " + points;
    }

    
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ball")
        {
            points++;
        }
    }
}
