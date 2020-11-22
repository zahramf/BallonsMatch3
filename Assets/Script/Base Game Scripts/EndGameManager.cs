using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum GameType
{
    Moves,
    Time
}

[System.Serializable]
public class EndGameRequirement
{
   
    public GameType gameType;
    public int counterValue;
}
public class EndGameManager : MonoBehaviour
{
    //public int coin;
    public GameObject movesLable;
    public GameObject timeLable;
    public GameObject YouWinPanel;
    public GameObject TryAgainPanel;
    public Text counter;
    public int currentCounterValue;
    public EndGameRequirement requirements;
    Board board;
    float timerSeconds;

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        SetGameType();
        SetupGame();
    }

    void SetGameType()
    {
        if(board.world != null)
        {
            if (board.level < board.world.levels.Length)
            {
                if (board.world.levels[board.level] != null)
                {
                    requirements = board.world.levels[board.level].endGameReqirements;
                }
        }
    }
    }

    void SetupGame()
    {
        currentCounterValue = requirements.counterValue;
        if(requirements.gameType == GameType.Moves)
        {
            movesLable.SetActive(true);
            timeLable.SetActive(false);
        }
        else
        {
            timerSeconds = 1;
            movesLable.SetActive(false);
            timeLable.SetActive(true);
        }
        counter.text = "" + currentCounterValue;
    }

     public  void DecreaseCounterValue()
    {
        if(board.currentState != GameState.pause)
        {
            currentCounterValue--;
            counter.text = "" + currentCounterValue;

            if (currentCounterValue <= 0)
            {

                LoseGame();
            }
        }
    }

    public void WinGame()
    {
        //coin += 10;
       
        YouWinPanel.SetActive(true);
      
        board.currentState = GameState.win;
        //coin += 10;
        //Debug.Log("coin" + coin);
        currentCounterValue = 0;
        counter.text = "" + currentCounterValue;
        FadePanelController fade = FindObjectOfType<FadePanelController>();
        fade.GameOver();

    }

    public void LoseGame()
    {
        TryAgainPanel.SetActive(true);
        board.currentState = GameState.lose;
        Debug.Log("You Lose");
        currentCounterValue = 0;
        counter.text = "" + currentCounterValue;
        FadePanelController fade = FindObjectOfType<FadePanelController>();
        fade.GameOver();

    }

    // Update is called once per frame
    void Update()
    {
        if(requirements.gameType == GameType.Time && currentCounterValue > 0)
        {
            timerSeconds -= Time.deltaTime;
            if(timerSeconds <= 0)
            {
                DecreaseCounterValue();
                timerSeconds = 1;
            }
        }
    }
}
