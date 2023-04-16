using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PictowordManager : MonoBehaviour
{
    public List<PictureItem> pictureItems = new List<PictureItem>();
    public PictureItem currentPictoword;
    public string currentWordText;
    [Space(20)]
    public int score;
    public int highScore;
    public float timer;
    public bool timerOn;
    [Space(20)]
    public Image picture1;
    public Image picture2;
    public TMP_InputField inputField;
    public TMP_Text wordText;
    public TMP_Text timerText;
    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    public TMP_Text finalScoreText;
    public GameObject gameOverScreen;
    public AudioClip correctAudio;

    void Start()
    {
        score = 0;
        gameOverScreen.SetActive(false);
        GrabRandomPictureItem();

        if(!PlayerPrefs.HasKey("Picto_High"))
            PlayerPrefs.SetInt("Picto_High", 0);

        highScore = PlayerPrefs.GetInt("Picto_High");
    }

    void Update()
    {
        scoreText.text = "Score: " + score;
        finalScoreText.text = "Final Score: " + score;
        highScoreText.text = "High Score: " + highScore;
        if(score >= highScore)
            highScore = score;

        if(timer > 0)
        {
            if(currentPictoword != null)
            {
                if(inputField.text.ToUpper() == currentPictoword.word.ToUpper())
                {
                    score++;

                    GameManager.Instance.audioSource.PlayOneShot(correctAudio);
                    inputField.text = "";
                    GrabRandomPictureItem();
                }
            }
        } else
        {
            gameOverScreen.SetActive(true);
        }

        if(timerOn) TimerUIUpdate();
    }

    public void SaveHighScore()
    {
        PlayerPrefs.SetInt("Picto_High", highScore);
    }

    void OnApplicationQuit()
    {
        SaveHighScore();
    }

    void TimerUIUpdate()
    {
        timerText.text = "Timer: " + timer.ToString("F2");
        if(timer > 0) timer -= Time.deltaTime;
        else timer = 0;
    }

    public void GrabRandomPictureItem()
    {
        if(currentPictoword == null)
            currentPictoword = pictureItems[UnityEngine.Random.Range(0, pictureItems.Count)];
        else
        {
            while(true)
            {
                PictureItem newPictureItem = pictureItems[UnityEngine.Random.Range(0, pictureItems.Count)];
                if(currentPictoword.word == newPictureItem.word) continue;
                else { currentPictoword = newPictureItem; break; }
            }
        }

        //currentWordText = new String('_', currentPictoword.word.Length);
        string result = "";
        for (int i = 0; i < currentPictoword.word.Length; i++)
        {
            if (currentPictoword.word[i] != ' ')
            {
                result += '_';
            }
            else
            {
                result += ' ';
            }
        }
        currentWordText = result;
        UpdateUI(currentPictoword);
    }

    public void UpdateUI(PictureItem item)
    {
        picture1.sprite = item.picture1;
        picture2.sprite = item.picture2;
        wordText.text = currentWordText;
    }
    

}
