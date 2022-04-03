using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text scoreText;
    public int updateScoreInterval = 5;

    private bool isRunning = false;
    private float startTime = 0;
    private float currentScore = 0;

    void Start()
    {
        //StartScore();
    }

    void Update()
    {
        if (isRunning)
        {
            currentScore = Time.time - startTime;
            if (Time.frameCount % updateScoreInterval == 0)
            {
                scoreText.text = currentScore.ToString("F2");
            }
        }
    }

    public void StartScore()
    {
        startTime = Time.time;
        isRunning = true;
    }
    public void EndScore()
    {
        isRunning = false;
    }
}
