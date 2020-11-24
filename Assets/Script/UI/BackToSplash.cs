using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackToSplash : MonoBehaviour
{
    public string sceneToLoad;
    public int numStars;
    //public GameObject[] stars;
    //public Image[] stars;
    public GameData gameData;
    public  ScoreManager scoreManager;
   public Board board;
    public int topLevel;
    public int starLevel;
    //static public int coin;
    public int coinp;
    //public int coin;
    public GameObject tryMenu;
    public Button btn;
   public EndGameManager gameMoveRequirement;

    public void WinOk()
    {
        if (gameData != null)
        {
           
            int doneLevel = board.level;
            topLevel = board.level + 1;

            PlayerPrefs.GetInt("Current Level");
           


            starLevel = scoreManager.GetComponent<ScoreManager>().numberStars;

            gameData.saveData.isActive[topLevel] = true;

            
            gameData.saveData.stars[board.level] = starLevel;
            
            PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + 50);
            

        }
      
        SceneManager.LoadScene(sceneToLoad);
       

    }

    public void LoseOk()
    {
        if (gameData != null)
        {
            gameData.saveData.isActive[board.level] = true;
            gameData.Save();


            //////
            //numStars = scoreManager.GetComponent<ScoreManager>().numberStars;

            //gameData.saveData.stars[board.level] = numStars;

            /////



        }
        SceneManager.LoadScene(sceneToLoad);

    }

    public void ContinueOk()
    {
        btn = GameObject.FindGameObjectWithTag("btn").GetComponent<Button>();
        if (btn.interactable = true)
        {
            tryMenu.SetActive(false);
        }
        //int c = 1;
        //if (c < 10)
        //{
        //    btn.interactable = false;
        //    //GetComponent<Button>().interactable = false;
        //    //btn.interactable = false;
        //}
        //else
        //{
        //    btn.enabled = true;
        //    //GetComponent<Button>().interactable = true;

        //    //btn.interactable = true;
        //}

    }

    // Start is called before the first frame update
    void Start()
    {
         
        //btn = GetComponent<Button>();
        // coin = PlayerPrefs.GetInt("Coin");
        //Debug.Log("Coin" + coin);
        //gameMoveRequirement = FindObjectOfType<EndGameManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        gameData = FindObjectOfType<GameData>();
        board = FindObjectOfType<Board>();
        //ContinueOk();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
