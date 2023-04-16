using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BasketballPointCounter : MonoBehaviour
{
    public int points;
    public int highScore;
    public TMP_Text pointsText;
    public TMP_Text highScoreText;
    public AudioClip netClip;
    public AudioClip scoreClip;

    void Start()
    {
        if(!PlayerPrefs.HasKey("BBall_High"))
            PlayerPrefs.SetInt("BBall_High", 0);

        highScore = PlayerPrefs.GetInt("BBall_High");
    }

    void Update()
    {
        pointsText.text = "Points: " + points;
        highScoreText.text = "High Score: " + highScore;

        if(points >= highScore)
            highScore = points;
    }

    public void SaveHighScore()
    {
        PlayerPrefs.SetInt("BBall_High", highScore);
    }

    void OnApplicationQuit()
    {
        SaveHighScore();
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
