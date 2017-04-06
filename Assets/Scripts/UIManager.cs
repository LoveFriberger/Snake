using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text timeValue;
    public Text scoreValue;
    public Text headerText;
    public Text currentlyBestText;
    public GameObject pauseMenuObject;

    private int score;
    private float startingTime;

    private void Awake()
    {
        GameManager.StartGame += ResetValues;
        GameManager.Win += WinGame;
        GameManager.Lose += LoseGame;
        GameManager.Score += AddScore;
        SetCurrentlyBestText();
    }

    private void Update()
    {
        if (GameManager.running)
        {
            //Update the current time every frame.
            timeValue.text = Mathf.FloorToInt(Time.time - startingTime).ToString();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.ClearBoard();
            GameManager.StartGame();
        }
    }

    private void AddScore()
    {
        //Update the score.
        score++;
        scoreValue.text = score.ToString();
    }

    private void WinGame()
    {
        //This runs if we fill up the whole field.
        pauseMenuObject.SetActive(true);

        //Compare our winning time and give the result.
        var currentBestWinTime = PlayerPrefs.GetInt("Win time", 0);
        if (score < currentBestWinTime)
            headerText.text = "You Won!";
        else if (score == currentBestWinTime)
            headerText.text = "Win time tied!";
        else
        {
            headerText.text = "New Best time!";
            PlayerPrefs.SetInt("Win time", score);
        }
        SetCurrentlyBestText();
    }

    private void LoseGame()
    {
        //This runs when we lose a round
        pauseMenuObject.SetActive(true);

        //Compare our score and give the polayer the result.
        var currentHighScore = PlayerPrefs.GetInt("Highscore", 0);
        if (score < currentHighScore || score == 0)
            headerText.text = "You died!";
        else if (score == currentHighScore)
            headerText.text = "Highscore tied!";
        else
        {
            headerText.text = "New highscore!";
            PlayerPrefs.SetInt("Highscore", score);
        }
        SetCurrentlyBestText();
    }

    private void SetCurrentlyBestText()
    {
        //This text tells the player what to aim for
        var currentBestWinTime = PlayerPrefs.GetInt("Win time", 0);
        if (currentBestWinTime > 0)
            currentlyBestText.text = string.Format("This game has been finished in {0} seconds", currentBestWinTime);
        else
            currentlyBestText.text = string.Format("The current highscore is {0} points", PlayerPrefs.GetInt("Highscore", 0));
    }

    private void ResetValues()
    {
        //Resets all values when starting a new run.
        pauseMenuObject.SetActive(false);
        score = 0;
        scoreValue.text = "0";
        startingTime = Time.time;
    }
    
}
