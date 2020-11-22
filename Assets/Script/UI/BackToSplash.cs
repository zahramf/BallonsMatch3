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
    public int coin;


    public void WinOk()
    {
        if (gameData != null)
        {
           
            int doneLevel = board.level;
            Debug.Log("Done" + ":" + doneLevel);
            topLevel = board.level + 1;
            Debug.Log("Top" + ":" + topLevel);

            PlayerPrefs.GetInt("Current Level");
            //PlayerPrefs.SetInt("TopLevel", topLevel);
            //Debug.Log("Top" + top);


            starLevel = scoreManager.GetComponent<ScoreManager>().numberStars;
            //gameData.saveData.isActive[board.level + 1] = true;

            gameData.saveData.isActive[topLevel] = true;

            //PlayerPrefs.GetInt("StarLevel");
            //PlayerPrefs.SetInt("StarLevel", starLevel);
            gameData.saveData.stars[board.level] = starLevel;
            //Debug.Log("board : " + board.level + " : " + numStars);
            coin += 10;
            PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + 10);
            //PlayerPrefs.SetInt("Coin", coin);
            //coinp=  PlayerPrefs.GetInt("Coin");
            //gameData.saveData.coins = coin;

            //gameData.Save();

            //PlayerPrefs.SetInt("Coin", coin);
            //Debug.Log("coinBack" + coinp);

        }
        //coin += 10;
        //PlayerPrefs.SetInt("Coin", coin);
        SceneManager.LoadScene(sceneToLoad);
       
        //coin += 10;

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

    // Start is called before the first frame update
    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        gameData = FindObjectOfType<GameData>();
        board = FindObjectOfType<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
