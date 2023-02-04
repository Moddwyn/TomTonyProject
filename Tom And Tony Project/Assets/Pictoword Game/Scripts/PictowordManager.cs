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
    public float timer;
    public bool timerOn;
    [Space(20)]
    public Image picture1;
    public Image picture2;
    public TMP_InputField inputField;
    public TMP_Text wordText;
    public TMP_Text timerText;

    void Start()
    {
        GrabRandomPictureItem();

    }

    void Update()
    {
        

        if(currentPictoword != null)
        {
            if(inputField.text.ToUpper() == currentPictoword.word.ToUpper())
            {
                print("guessed!");
            }
        }

        if(timerOn) TimerUIUpdate();
    }

    void TimerUIUpdate()
    {
        timerText.text = "Timer: " + timer.ToString("F2");
        if(timer > 0) timer -= Time.deltaTime;
        else timer = 0;
    }

    public void GrabRandomPictureItem()
    {
        currentPictoword = pictureItems[UnityEngine.Random.Range(0, pictureItems.Count)];

        currentWordText = new String('_', currentPictoword.word.Length);
        UpdateUI(currentPictoword);
    }

    public void UpdateUI(PictureItem item)
    {
        picture1.sprite = item.picture1;
        picture2.sprite = item.picture2;
        wordText.text = currentWordText;
    }
    

}
