using System.Collections;
using System.Collections.Generic;
using System.Data;
using NaughtyAttributes;
using TMPro;
using UnityEditor;
using UnityEngine;

public class MathGame : MonoBehaviour
{

    [Space(20)]
    public int score;
    public int highScore;
    public float timer;
    [Space(20)]
    public TMP_Text timerText;
    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    public TMP_Text finalScoreText;
    public GameObject gameOverScreen;
    public AudioClip correctAudio;
    public AudioClip wrongAudio;

    [ReadOnly][SerializeField]
    bool hasEquation;
    
    void Start()
    {
        score = 0;
        gameOverScreen.SetActive(false);

        if(!PlayerPrefs.HasKey("Math_High"))
            PlayerPrefs.SetInt("Math_High", 0);

        highScore = PlayerPrefs.GetInt("Math_High");
    }

    void Update()
    {
        if(timer > 0)
        {
            
        } else
        {
            gameOverScreen.SetActive(true);
        }

        TimerUIUpdate();
    }

    void TimerUIUpdate()
    {
        timerText.text = "Timer: " + timer.ToString("F2");
        if(timer > 0) timer -= Time.deltaTime;
        else timer = 0;
    }

    public void SaveHighScore()
    {
        PlayerPrefs.SetInt("Math_High", highScore);
    }

    void OnApplicationQuit()
    {
        SaveHighScore();
    }

    public static double Evaluate(string expression)
    {
        var table = new DataTable();
        var value = table.Compute(expression, "");
        return double.Parse(value.ToString());
    }
}
