﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject gameCanvas;
    public GameObject gameOverCanvas;

    public GameObject player;
    public GameObject ball;

    public Text lives;
    public Text score;
    public Text finalScore;

    public static GameManager Instance;

    public bool isBallReady = false;

    private int currentScore = 0;
    private int highScore;
    private int playerLives = 3;

    public float baseBallSpeed;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        mainMenuCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        isBallReady = true;
        playerLives = 3;
    }

    public void PlayAgain()
    {
        gameOverCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        isBallReady = true;
        playerLives = 3;
    }

    public void LaunchBall()
    {
        if(isBallReady)
        {
            isBallReady = !isBallReady;
            BallController bc = ball.GetComponent<BallController>();
            bc.velocity = new Vector2(Random.Range(1.0f, 2.0f), Random.Range(0.0f, 2.0f));
            float multiplier = bc.velocity.magnitude / baseBallSpeed;
            bc.velocity = new Vector2(bc.velocity.x/multiplier, bc.velocity.y / multiplier);
        }
    }

    public void IncreaseScore()
    {
        currentScore++;
        BallController bc = ball.GetComponent<BallController>();
        //float speedX, speedY;
        Vector2 ballDir = bc.velocity;
        ballDir *= (bc.velocity.magnitude + baseBallSpeed / 100.0f) / bc.velocity.magnitude;
        Debug.Log(bc.velocity.magnitude);
        bc.velocity = ballDir;
        /*if (ballRg.velocity.x >= 0)
        {
            speedX = ballDir.x;
        }
        else speedX = -ballDir.x;
        if (ballRg.velocity.y >= 0)
        {
            speedY = ballDir.y;
        }
        else speedY = -ballDir.y;
        ballRg.velocity = new Vector2(ballRg.velocity.x + speedX, ballRg.velocity.y + speedY);*/
        score.text = "Score: " + currentScore.ToString();
    }

    public void Kill()
    {
        playerLives--;
        if(playerLives<=0)
        {
            GameOver();
        }
        else
        {
            BallController bc = ball.GetComponent<BallController>();
            bc.velocity *= 0;
            lives.text = "Lives: " + playerLives.ToString();
            isBallReady = true;
            ball.transform.position = new Vector2(0.0f, 0.0f);
            player.transform.position = new Vector2(-8.0f, 0.0f);
        }
    }

    public void GameOver()
    {
        BallController bc = ball.GetComponent<BallController>();
        bc.velocity *= 0;
        gameOverCanvas.SetActive(true);
        gameCanvas.SetActive(false);
        finalScore.text = "FINAL SCORE: " + currentScore.ToString();
    }

    public void Quit()
    {
        Application.Quit();
    }
}