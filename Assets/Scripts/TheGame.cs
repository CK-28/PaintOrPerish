using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TheGame : MonoBehaviour
{
    public TMP_Text RedScoreText;
    public TMP_Text BlueScoreText;
    private int RedScore;
    private int BlueScore;

    //bool gameStarted = true;

    // Start is called before the first frame update
    void Start()
    {
        updateRedScore(0);
        updateBlueScore(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isGameStarted ()
    {
        var gameObject = GameObject.Find("Timer");
        Timer timeScript = gameObject.GetComponent<Timer>();

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
