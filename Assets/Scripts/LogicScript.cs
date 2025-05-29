using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogicScript : MonoBehaviour
{
    public int playerScore = 0;
    public int playerHighScore;

    public Text scoreText;
    public Text highScoreText;
    public GameObject gameOverScreen;
    public AudioSource highScoreAudio;

    void Start()
    {
        playerHighScore = PlayerPrefs.GetInt("highScore", 0);
        highScoreText.text = "Record: " + playerHighScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void addScore(int scoreToAdd)
    {
        if (gameOverScreen.activeSelf != true)
        {
            playerScore += scoreToAdd;
            scoreText.text = playerScore.ToString();
        }
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void gameOver()
    {
        gameOverScreen.SetActive(true);

        if (playerScore > playerHighScore)
        {
            highScoreAudio.Play();
            playerHighScore = playerScore;
            PlayerPrefs.SetInt("highScore", playerHighScore);
            PlayerPrefs.Save();
            highScoreText.text = "High score: " + playerHighScore.ToString();
        }

    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
