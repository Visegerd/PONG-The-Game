using System.Collections;
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
    public Text gameOverHighScore;
    public Text highScoreText;

    public static GameManager Instance;

    public bool isBallReady = false;

    private int currentScore = 0;
    private int highScore;
    private int playerLives = 3;
    private string highScoreKey = "HighScore";

    public float baseBallSpeed;

    private void Awake() //set singleton and read highscore key
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        highScore = PlayerPrefs.GetInt(highScoreKey, 0);
        highScoreText.text = "Najlepszy wynik: " + highScore.ToString();
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void StartGame() //Start game from main menu
    {
        mainMenuCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        isBallReady = true;
        playerLives = 3;
    }

    public void LaunchBall() //Launch ball to play
    {
        if(isBallReady)
        {
            isBallReady = !isBallReady;
            BallController bc = ball.GetComponent<BallController>();
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            bc.velocity = new Vector2(Random.Range(1.0f, 4.0f), Random.Range(-4.0f, 4.0f));
            float multiplier = bc.velocity.magnitude / baseBallSpeed;
            bc.velocity = new Vector2(bc.velocity.x/multiplier, bc.velocity.y / multiplier);
            bc.speed = baseBallSpeed;
            bc.CheckWall();
        }
    }

    public void IncreaseScore() //Increase player score
    {
        currentScore++;
        BallController bc = ball.GetComponent<BallController>();
        bc.speed *= (bc.velocity.magnitude + baseBallSpeed / 100.0f) / bc.velocity.magnitude;
        score.text = "Punkty: " + currentScore.ToString();
    }

    public void Kill() //Kill and reset
    {
        playerLives--;
        if(playerLives<=0)
        {
            GameOver();
        }
        else
        {
            BallController bc = ball.GetComponent<BallController>();
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            bc.velocity = new Vector2(0.0f, 0.0f);
            rb.isKinematic = true;
            lives.text = "Życia: " + playerLives.ToString();
            isBallReady = true;
            ball.transform.position = new Vector2(0.0f, 0.0f);
            player.transform.position = new Vector2(-8.0f, 0.0f);
        }
    }

    private void PlayAgain() //Play again from game over menu
    {
        gameOverCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        isBallReady = true;
        playerLives = 3;
        currentScore = 0;
        lives.text = "Życia: " + playerLives.ToString();
        score.text = "Punkty: " + currentScore.ToString();
        BallController bc = ball.GetComponent<BallController>();
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        bc.velocity = new Vector2(0.0f, 0.0f);
        rb.isKinematic = true;
        isBallReady = true;
        ball.transform.position = new Vector2(0.0f, 0.0f);
        player.transform.position = new Vector2(-8.0f, 0.0f);
    }

    private void GameOver() //Player ran out of lives
    {
        BallController bc = ball.GetComponent<BallController>();
        bc.velocity *= 0;
        gameOverCanvas.SetActive(true);
        gameCanvas.SetActive(false);
        finalScore.text = "Wynik końcowy: " + currentScore.ToString();
        if (currentScore>highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt(highScoreKey, highScore);
            PlayerPrefs.Save();
        }
        gameOverHighScore.text = "Najlepszy wynik: " + highScore.ToString();
    }

    private void Quit() //Quit game
    {
        Application.Quit();
    }
}
