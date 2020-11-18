using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private Board board;
    public Text scoreText;
    public int score;
    public Image scoreBar;
    //GameData gameData;
   //public int Stars;
    public int numberStars;
    BackToSplash backSlash;

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        //gameData = FindObjectOfType<GameData>();
        UpdateBar();
        //if(gameData != null)
        //{
        //    gameData.Load();
        //}
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = ""+score;
    }

    public void IncreaseScore(int amountToIncrease)
    {
        score += amountToIncrease;
        for (int i = 0; i < board.scoreGoals.Length; i++)
        {
            if (score > board.scoreGoals[i] && numberStars < i + 1)
            {
                numberStars++;

            }
        }
        //if (gameData != null)
        //{
        //int highScore = gameData.saveData.highScores[board.level];
        //if (score > highScore)
        //{
        //    //backSlash.WinOk(numberStars);
        //    gameData.saveData.highScores[board.level] = score;

        //    gameData.saveData.stars[board.level] = numberStars;



        //}
        //int currentStars = gameData.saveData.stars[board.level];
        ////int currentStars = backSlash.WinOk

        //if (numberStars > currentStars)
        //{
        //    //backSlash.WinOk(numberStars);
        //    gameData.saveData.stars[board.level] = numberStars;
        //}
        //gameData.Save();
        //}
        UpdateBar();
        //    score += amountToIncrease;
        //    for (int i = 0; i < board.scoreGoals.Length; i++)
        //    {
        //        if (score > board.scoreGoals[i] && Stars < i + 1)
        //        {
        //            Stars++;

        //        }
        //    }
        //    //if (gameData != null)
        //    //{
        //    int highScore = gameData.saveData.highScores[board.level];
        //    if (score > highScore)
        //    {
        //        numberStars = Stars;
        //        Debug.Log(numberStars);
        //        //backSlash.WinOk(numberStars);
        //        //gameData.saveData.highScores[board.level] = score;

        //        //gameData.saveData.stars[board.level] = numberStars;



        //    }
        //    int currentStars = gameData.saveData.stars[board.level];
        //    //int currentStars = backSlash.WinOk

        //    if (numberStars > currentStars)
        //    {
        //        numberStars = Stars;
        //        Debug.Log(numberStars);
        //        //backSlash.WinOk(numberStars);
        //        //gameData.saveData.stars[board.level] = numberStars;
        //    }
        //    //score += amountToIncrease;

        //    //if (gameData != null)
        //    //{
        //    //    int highScore = gameData.saveData.highScores[board.level];
        //    //    if (score > highScore)
        //    //    {
        //    //        gameData.saveData.highScores[board.level] = score;
        //    //    }
        //    //    gameData.Save();
        //    //}
        //    //for (int i = 0; i < board.scoreGoals.Length; i++)
        //    //{
        //    //    if (score > board.scoreGoals[i] && numberStars < i + 1)
        //    //    {
        //    //        numberStars++;

        //    //    }
        //    //}
        //    //}
        //    //if (gameData != null)
        //    //{
        //    //int highScore = gameData.saveData.highScores[board.level];
        //    //if (score > highScore)
        //    //{
        //    //    //backSlash.WinOk(numberStars);
        //    //    //gameData.saveData.highScores[board.level] = score;

        //    //    gameData.saveData.stars[board.level] = numberStars;



        //    //}
        //    //int currentStars = gameData.saveData.stars[board.level];
        //    ////int currentStars = backSlash.WinOk

        //    //if (numberStars > currentStars)
        //    //{
        //    //    //backSlash.WinOk(numberStars);
        //    //    //gameData.saveData.stars[board.level] = numberStars;
        //    //}
        //    //gameData.Save();
        //    //}
        //    UpdateBar();

    }



    void UpdateBar()
    {
        if (board != null && scoreBar != null)
        {
            int length = board.scoreGoals.Length;
            scoreBar.fillAmount = (float)score / (float)board.scoreGoals[length - 1];
        }
    }

}
