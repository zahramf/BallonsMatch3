using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    [Header("Active Stuff")]
    public bool isActive;
    public Sprite activeSprite;
    public Sprite lockedSprite;
    Image buttonImage;
    Button myButton;
    int starsActive;


    [Header("Level UI")]

    public Image[] stars;
    public Text levelText;
    public int level;
    public GameObject confirmPanel;
    Board board;


    GameData gameData;
    // Start is called before the first frame update
    void Start()
    {
        gameData = FindObjectOfType<GameData>();
        buttonImage = GetComponent<Image>();
        myButton = GetComponent<Button>();
        board = FindObjectOfType<Board>();
        LoadData();
        ActiveStars();
        ShowLevel();
        DecideSprite();
    }

    void LoadData()
    {
        //Is game data present?
        if (gameData != null)
        {
            int top = PlayerPrefs.GetInt("Current Level");
            //Debug.Log("Player Toooooooooooooooooooooooooooop"+ top);
            //Decide if the level is active
            if (  gameData.saveData.isActive[level - 1])
            //if (board.level > PlayerPrefs.GetInt("TopLevel"))
            {
                //Debug.Log("Level" + board.level);
                isActive = true;
                //gameData.saveData.isActive[level - 1]
            }
            else
            {
                isActive = false;

            }
            //Decide how many stars to active
            starsActive = gameData.saveData.stars[level - 1];
        }
    }

    void ActiveStars()
    {
        for(int i = 0; i < starsActive; i++)
        {
            stars[i].enabled = true;
        }
    }

    void DecideSprite()
    {
        if (isActive)
        {
            buttonImage.sprite = activeSprite;
            myButton.enabled = true;
            levelText.enabled = true;
        }
        else
        {
            buttonImage.sprite = lockedSprite;
            myButton.enabled = false;
            levelText.enabled = false;
        }
    }


    void ShowLevel()
    {
        levelText.text = "" + level;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public void ConfirmPanel(int level)
    {
        if (isActive)
        {
            confirmPanel.GetComponent<ConfirmPanel>().level = level;
            confirmPanel.SetActive(true);


        }

    }
  
}
