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
   public EndGameRequirement gameMoveRequirement;
    public EndGameManager endgame;
    GoalManager goal;
    LevelButton levelButton;
    ConfirmPanel confirmPAnel;
    public int currentLevel;
    World world;
    Manager manager;
    SoundManager soundManager;
    public void WinOk()
    {
        PlayerPrefs.SetInt("winpanel", 0);
        PlayerPrefs.SetInt("panel", 0);
        if (gameData != null)
        {
            int doneLevel = board.level;
            topLevel = board.level + 1;

            PlayerPrefs.GetInt("Current Level");
           


            starLevel = scoreManager.GetComponent<ScoreManager>().numberStars;

            gameData.saveData.isActive[topLevel] = true;

            
            gameData.saveData.stars[board.level] = starLevel;
            
            PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + 50);
            PlayerPrefs.SetInt("First", 1);
            

        }
        soundManager.PlayButtonSound();

        SceneManager.LoadScene(sceneToLoad);
       

    }

    public void LoseOk()
    {
        PlayerPrefs.SetInt("winpanel", 0);
        PlayerPrefs.SetInt("panel", 0);
        if (gameData != null)
        {
            gameData.saveData.isActive[board.level] = true;
            gameData.Save();


            //////
            //numStars = scoreManager.GetComponent<ScoreManager>().numberStars;

            //gameData.saveData.stars[board.level] = numStars;

            /////



        }
        soundManager.PlayButtonSound();

        SceneManager.LoadScene(sceneToLoad);

    }

    public void LooseOk()
    {
        btn = GameObject.FindGameObjectWithTag("btne").GetComponent<Button>();
        if (btn.interactable = true)
        {
            PlayerPrefs.SetInt("panel", 0);
            soundManager.PlayButtonSound();
            int energy = PlayerPrefs.GetInt("totalEnergy");

            energy -= 1;
            PlayerPrefs.SetInt("FirstE", 1);
            PlayerPrefs.SetInt("totalEnergy", energy);
            int e = PlayerPrefs.GetInt("totalEnergy");
            Debug.Log("Elose" + e);
            tryMenu.SetActive(false);
            PlayerPrefs.SetInt("panel", 0);
            //int  totalEnergy = PlayerPrefs.GetInt("totalEnergy");
            //Debug.Log("E1" + totalEnergy);
            //totalEnergy--;
            //PlayerPrefs.SetInt("firstEnergy", 1);
            //PlayerPrefs.SetInt("totalEnergy", totalEnergy);
            int lvl = PlayerPrefs.GetInt("OnLevel");
            Debug.Log("onlevel" + lvl);
            board.LoseEnergy();
            endgame.SetupGame();
            goal.SetGoal();
            //endgame.SetupGame();
            //confirmPAnel.GetComponent<ConfirmPanel>().level = lvl;

            //levelButton.LoseConfirmPanel(lvl);
            //confirmPAnel.Play();
            //confirmPanel.GetComponent<ConfirmPanel>().level = lvl;
            //world.; levels[lvl];

            //manager.UseEnergyMethod();
            //tryMenu.SetActive(false);
            //int loseCoin = PlayerPrefs.GetInt("Coin");
            //loseCoin -= 900;
            //PlayerPrefs.SetInt("Coin", loseCoin);
            //board.LoseManager();
            //endgame.SetupGame();

        }


    }

    public void ContinueOk()
    {
        btn = GameObject.FindGameObjectWithTag("btn").GetComponent<Button>();
        if (btn.interactable = true)
        {
            soundManager.PlayButtonSound();
            tryMenu.SetActive(false);
            PlayerPrefs.SetInt("panel", 0);
            int loseCoin= PlayerPrefs.GetInt("Coin");
            loseCoin -= 900;
            PlayerPrefs.SetInt("Coin", loseCoin);
            board.LoseManager();
            endgame.SetupGame();
            PlayerPrefs.SetInt("winpanel", 0);


        }


    }

    // Start is called before the first frame update
    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        goal = FindObjectOfType<GoalManager>();
        manager = FindObjectOfType<Manager>();
        levelButton = FindObjectOfType<LevelButton>();
        confirmPAnel = FindObjectOfType<ConfirmPanel>();
        endgame = FindObjectOfType<EndGameManager>();
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
