using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float startTime;
    public WallOfDeath wod;
    public float stopWallDelay = 0.5f;
    public Score score;
    public Generator gen;
    public float gameOverDelay = 0.5f;
    public Animator gameOverAnimator;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (BlackScreen.instance == null)
        {
            StartGame();
        }
        else
        {
            BlackScreen.instance.done += StartGame;
            BlackScreen.instance.FadeFromBlack();
        }
    }


    public void StartGame()
    {
        if (BlackScreen.instance != null)
        {
            BlackScreen.instance.done -= StartGame;
        }
        score.StartScore();
        gen.isGenerating = true;
        wod.StartWall();
    }

    public void Die()
    {
        score.EndScore();
        Player.instance.GetComponent<PlayerDeath>().enabled = true;
        Player.instance.enabled = false;
        StartCoroutine(StopWallOfDeath());
        gen.isGenerating = false;
    }

    IEnumerator StopWallOfDeath()
    {
        yield return new WaitForSeconds(stopWallDelay);
        wod.Stop();
        yield return new WaitForSeconds(gameOverDelay);
        gameOverAnimator.Play("GameOver");
    }

}
