using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BlankGoal
{
    public int numberNeeded;
    public int numberCollected;
    public Sprite goalSprite;
    public string matchValue;
}

public class GoalManager : MonoBehaviour
{
    public int coin;
    public GameObject[] stars;

    public int numberOfStar;
    public BlankGoal[] levelGoals;
    public List<GoalPanel> currentGoals = new List<GoalPanel>();
    public GameObject goalPrefab;
    public GameObject goalIntroPrefab;
    public GameObject goalIntroParent;
    public GameObject goalGameParent;
    ScoreManager scoreManager;
    public GameObject gameGoal;
    //public GoalPanel panel;

    //GameData gameData;
    Board board;
    EndGameManager endGame;

    // Start is called before the first frame update
    void Start()
    {
         //panel = FindObjectOfType<GoalPanel>();
        board = FindObjectOfType<Board>();
        endGame = FindObjectOfType<EndGameManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        GetGoals();
        SetupGoals();
    }

    void GetGoals()
    {
        if(board != null)
        {
            if(board.world != null)
            {
                if (board.level < board.world.levels.Length)
                {
                    if (board.world.levels[board.level] != null)
                    {
                        levelGoals = board.world.levels[board.level].levelGoals;
                        for (int i = 0; i < levelGoals.Length; i++)
                        {
                            levelGoals[i].numberCollected = 0;
                        }
                    }
                }
            }
        }
    }

    public void SetGoal()
    {
        GameObject[] goal;
        
        for(int j = 0; j < levelGoals.Length; j++)
        {
            goal = GameObject.FindGameObjectsWithTag("goal");
            Destroy(goal[j]);
            //Destroy(levelGoals[j].goalSprite);
            currentGoals.Clear();
        }
        
        for (int i = 0; i < levelGoals.Length; i++)
        {


            //Create a new goalPanel at the goalGamePanel position
            gameGoal = Instantiate(goalPrefab, goalGameParent.transform.position, Quaternion.identity);
            gameGoal.transform.SetParent(goalGameParent.transform);
            GoalPanel panel = gameGoal.GetComponent<GoalPanel>();
            panel = gameGoal.GetComponent<GoalPanel>();
            currentGoals.Add(panel);
            panel.thisSprite = levelGoals[i].goalSprite;
            panel.thisString = "0/" + levelGoals[i].numberNeeded;
            levelGoals[i].numberCollected = 0;

        }
    }


  public  void SetupGoals()
    {
        for(int i = 0; i < levelGoals.Length; i++)
        {
            //Create a new goalPanel at the goalIntroParent position
            GameObject goal = Instantiate(goalIntroPrefab, goalIntroParent.transform.position, Quaternion.identity);
            goal.transform.SetParent(goalIntroParent.transform);

            //Set Image and text of the goal
            GoalPanel panel = goal.GetComponent<GoalPanel>();
            panel.thisSprite = levelGoals[i].goalSprite;
            panel.thisString = ""+ levelGoals[i].numberNeeded;


            //Create a new goalPanel at the goalGamePanel position
            gameGoal = Instantiate(goalPrefab, goalGameParent.transform.position, Quaternion.identity);
            gameGoal.transform.SetParent(goalGameParent.transform);
          GoalPanel  panel2 = gameGoal.GetComponent<GoalPanel>();
            currentGoals.Add(panel2);
            panel2.thisSprite = levelGoals[i].goalSprite;
            panel2.thisString = "0/" + levelGoals[i].numberNeeded;
        }
    }

  public void UpdateGoals()
    {
        int goalsCompleted = 0;
        for(int i = 0; i < levelGoals.Length; i++)
        {
            currentGoals[i].thisText.text = "" + levelGoals[i].numberCollected + "/" + levelGoals[i].numberNeeded;
            if (levelGoals[i].numberCollected >= levelGoals[i].numberNeeded)
            {
                goalsCompleted++;
                currentGoals[i].thisText.text = "" + levelGoals[i].numberNeeded + "/" + levelGoals[i].numberNeeded;
            }
        }

        if(goalsCompleted >= levelGoals.Length)
        {
            if (endGame != null)
            {
                numberOfStar = scoreManager.GetComponent<ScoreManager>().numberStars;
                for (int i = 0; i < numberOfStar; i++)
                {
                    stars[i].SetActive(true);
                }
                //gameData.saveData.stars[board.level] = numberOfStar;
                StartCoroutine(endGame.WinGame());
                //endGame.WinGame();
              

                //gameData.Save();
            }
            Debug.Log("You Win");
        }
       
    }

    public void CompareGoal(string goalToCompare)
    {
        for(int i = 0; i < levelGoals.Length; i++)
        {
            if(goalToCompare == levelGoals[i].matchValue)
            {
                levelGoals[i].numberCollected++;
            }
        }
    }
}
