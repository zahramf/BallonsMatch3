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
    Button btn;

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

  public  void SetupGame()
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
            Debug.Log("currentCounterValue" + currentCounterValue);
            counter.text = "" + currentCounterValue;

            if (currentCounterValue <= 0)
            {
                StartCoroutine(LoseGame());
                //LoseGame();
            }
        }
    }

    public IEnumerator WinGame()
    {
        //coin += 10;
        board.currentState = GameState.win;
        YouWinPanel.SetActive(true);
        
        //coin += 10;
        //Debug.Log("coin" + coin);
        currentCounterValue = 0;
        counter.text = "" + currentCounterValue;
        FadePanelController fade = FindObjectOfType<FadePanelController>();
        fade.GameOver();
        yield return new WaitForSeconds(1f);

        PlayerPrefs.SetInt("panel", 1);


    }

    public IEnumerator LoseGame()
    {
        board.currentState = GameState.lose;
        TryAgainPanel.SetActive(true);
        btn = GameObject.FindGameObjectWithTag("btn").GetComponent<Button>();
        Button eBtn= GameObject.FindGameObjectWithTag("btne").GetComponent<Button>();


        int coin = PlayerPrefs.GetInt("Coin");
        if (coin < 900)
        {
            btn.interactable = false;
            //GetComponent<Button>().interactable = false;
            //btn.interactable = false;
        }
        else
        {
            btn.enabled = true;
            //GetComponent<Button>().interactable = true;

            //btn.interactable = true;
        }
        int energy = PlayerPrefs.GetInt("totalEnergy");
        if (energy <= 0)
        {
            eBtn.interactable = false;
            //GetComponent<Button>().interactable = false;
            //btn.interactable = false;
        }
        else
        {
            eBtn.enabled = true;
            //GetComponent<Button>().interactable = true;

            //btn.interactable = true;
        }
        
        Debug.Log("You Lose");
        currentCounterValue = 0;
        counter.text = "" + currentCounterValue;
        FadePanelController fade = FindObjectOfType<FadePanelController>();
        fade.GameOver();
        yield return new WaitForSeconds(1f);
        PlayerPrefs.SetInt("panel", 1);

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
