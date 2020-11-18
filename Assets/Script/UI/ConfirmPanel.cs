using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConfirmPanel : MonoBehaviour
{
    [Header("LEvel Information")]
    public string levelToLoad;
    public int level;
    GameData gameData;
    int starsActive;
    int highScore;

    [Header("UI Stuff")]
    public Image[] stars;
    public Text highScoreText;
    public Text starText;



    // Start is called before the first frame update
    void OnEnable()
    {
        gameData = FindObjectOfType<GameData>();
        //LoadData();
        //ActiveStars();
        //SetText();
    }

    void SetText()
    {
        highScoreText.text = "" + highScore;
        starText.text = "" + starsActive + "/3";
        
    }
    void LoadData()
    {
        if (gameData != null)
        {
            starsActive = gameData.saveData.stars[level - 1];
            //highScore = gameData.saveData.highScores[level - 1];
        }
    }

    void ActiveStars()
    {
        for (int i = 0; i < starsActive; i++)
        {
            stars[i].enabled = true;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

   public void Cancel()
    {
        this.gameObject.SetActive(false);
    }

   public void Play()
    {
        //if (gameData != null) {
        //    gameData.Save();

        //}
       PlayerPrefs.SetInt("Current Level", level-1);
        //int tooop = PlayerPrefs.GetInt("TopLevel");
        //Debug.Log("toooooooooooooooooooooop" + tooop);

        SceneManager.LoadScene(levelToLoad);
    }
}
