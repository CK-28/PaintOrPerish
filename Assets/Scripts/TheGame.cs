using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TheGame : MonoBehaviour
{
    GameObject timer;
    Timer timeScript;

    GameObject gameOver, crosshair, winner;

    public TMP_Text RedScoreText;
    public TMP_Text BlueScoreText;
    public TMP_Text Winner;
    private int RedScore;
    private int BlueScore;

    private const int SCORE_TO_WIN = 5;

    //bool gameStarted = true;

    // Start is called before the first frame update
    void Start()
    {
        // reset scores
        updateRedScore(0);
        updateBlueScore(0);

        // initialize timer
        timer = GameObject.Find("Timer");
        timeScript = timer.GetComponent<Timer>();

        // show crosshair
        crosshair = GameObject.FindGameObjectWithTag("Crosshair");
        crosshair.SetActive(true);

        // hide game over text
        gameOver = GameObject.FindGameObjectWithTag("GameOver");
        gameOver.SetActive(false);

        // hide winner text
        winner = GameObject.FindGameObjectWithTag("Winner");
        winner.SetActive(false);
    }

    // checks for game end condition.  returns true if condition is met, false otherwise
    public bool GameOver()
    {
        if (RedScore >= SCORE_TO_WIN || BlueScore >= SCORE_TO_WIN)
        {
            EndGame();
            return true;
        } else if (timeScript.returnTimeLeft() <= 0)
        {
            EndGame();
            return true;
        }
        return false;
    }

    // carries out end of game procedures
    private void EndGame()
    {
        // show game over text and hide crosshair
        gameOver.SetActive(true);
        crosshair.SetActive(false);
        winner.SetActive(true);

        if (RedScore > BlueScore)
        {
            Winner.text = "YOU WIN!";
        }
        else if (RedScore == BlueScore)
        {
            Winner.text = "IT'S A TIE!";
        }
        else
        {
            Winner.text = "YOU LOSE!";
        }
    }

    public bool isGameStarted ()
    {
        if (timeScript.returnTimeLeft() < 80)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void updateRedScore(int toAdd)
    {
        RedScore = RedScore + toAdd;
        RedScoreText.text = "Red Score: " + RedScore;
    }

    public void updateBlueScore(int toAdd)
    {
        BlueScore = BlueScore + toAdd;
        BlueScoreText.text = "Blue Score: " + BlueScore;
    }
}
