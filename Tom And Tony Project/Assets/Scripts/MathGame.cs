using System.Collections.Generic;
using System.Data;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class MathGame : MonoBehaviour
{
    [ReadOnly] public string equation;
    [Space(20)]
    public int score;
    [ReadOnly] public int highScore;
    public float timer;
    [Space(20)]
    public TMP_Text equationText;
    public TMP_Text button1;
    public TMP_Text button2;
    public TMP_Text button3;
    public TMP_Text timerText;
    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    public TMP_Text finalScoreText;
    public GameObject gameOverScreen;
    public AudioClip correctAudio;
    public AudioClip wrongAudio;


    [ReadOnly][SerializeField] bool startTimer;
    [ReadOnly][SerializeField] int correctButton;

    void Start()
    {
        score = 0;
        equation = "";
        startTimer = false;
        gameOverScreen.SetActive(false);

        if (!PlayerPrefs.HasKey("Math_High"))
            PlayerPrefs.SetInt("Math_High", 0);

        highScore = PlayerPrefs.GetInt("Math_High");

        PrintSimilarNumbers(238);
    }

    public void StartGame()
    {
        startTimer = true;
    }

    int[] PrintSimilarNumbers(int num)
    {
        // Get the last digit of the input number
        int lastDigit = num % 10;

        // Generate two random numbers with the same last digit and decimal place as the input number
        int similarNum1;
        int similarNum2;
        int rangeX = (int)System.Math.Pow(10, num.ToString().Length - 2) * 10;
        int rangeY = (int)System.Math.Pow(10, num.ToString().Length - 1) * 10;

        if (num.ToString().Length == 1)
        {
            similarNum1 = Random.Range(0, 10);
            similarNum2 = Random.Range(0, 10);
            while(similarNum1 == similarNum2 || similarNum1 == num || similarNum2 == num)
            {
                similarNum1 = Random.Range(0, 10);
                similarNum2 = Random.Range(0, 10);
            }
        }
        else
        {
            similarNum1 = Random.Range(rangeX, rangeY);
            similarNum2 = Random.Range(rangeX, rangeY);

            similarNum1 = (similarNum1 - (similarNum1 % 10)) + lastDigit;
            similarNum2 = (similarNum2 - (similarNum2 % 10)) + lastDigit;
            while (similarNum1 == similarNum2 || similarNum1 == num || similarNum2 == num)
            {
                similarNum1 = Random.Range(rangeX, rangeY);
                similarNum2 = Random.Range(rangeX, rangeY);

                similarNum1 = (similarNum1 - (similarNum1 % 10)) + lastDigit;
                similarNum2 = (similarNum2 - (similarNum2 % 10)) + lastDigit;
            }
        }



        return new int[] { similarNum1, similarNum2 };
    }


    void Update()
    {
        timerText.text = "Timer: " + timer.ToString("F2");
        scoreText.text = "Score: " + score;
        finalScoreText.text = "Final Score: " + score;
        highScoreText.text = "High Score: " + highScore;

        if (score >= highScore)
            highScore = score;

        if (timer > 0 && startTimer)
        {
            timer -= Time.deltaTime;
        }

        if (timer <= 0)
        {
            timer = 0;
            startTimer = false;
            gameOverScreen.SetActive(true);
        }

        EquationGeneration();
    }

    void EquationGeneration()
    {
        if (string.IsNullOrEmpty(equation) && startTimer)
        {
            equation += Random.Range(1, 51) + "";

            int randOperation = Random.Range(1, 4);
            if (randOperation == 1)
                equation += " + ";
            else if (randOperation == 1)
                equation += " - ";
            else
                equation += " * ";

            equation += Random.Range(1, 51) + "";

            int randButton = Random.Range(1, 4);
            List<double> numbers = new List<double>();

            numbers.Add(Evaluate(equation));
            int[] otherChoices = PrintSimilarNumbers((int)numbers[0]);
            numbers.Add(otherChoices[0]);
            numbers.Add(otherChoices[1]);
            foreach (var item in numbers)
            {
                print(item);
            }
            if (randButton == 1)
            {
                button1.text = numbers[0] + "";
                button2.text = numbers[1] + "";
                button3.text = numbers[2] + "";
                correctButton = 1;
            }
            else if (randButton == 2)
            {
                button1.text = numbers[1] + "";
                button2.text = numbers[0] + "";
                button3.text = numbers[2] + "";
                correctButton = 2;
            }
            else
            {
                button1.text = numbers[2] + "";
                button2.text = numbers[1] + "";
                button3.text = numbers[0] + "";
                correctButton = 3;
            }

            equationText.text = equation + " = ?";


        }
    }

    public void ChooseButton(int num)
    {
        if (num == correctButton)
        {
            score++;
            timer += 5;
            equation = "";
            GameManager.Instance.audioSource.PlayOneShot(correctAudio);
        }
        else
        {
            timer -= 5;
            GameManager.Instance.audioSource.PlayOneShot(wrongAudio);
        }
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
