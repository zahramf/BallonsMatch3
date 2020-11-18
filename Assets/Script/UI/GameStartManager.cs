using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartManager : MonoBehaviour
{
    //GameData gameData;
    public void StartGame()
    {
        SceneManager.LoadScene("Splash");
    }
    //public GameObject startPanel;
    //public GameObject levelPanel;
    //// Start is called before the first frame update
    void Start()
    {
        //gameData = FindObjectOfType<GameData>();
        //if(gameData != null)
        //{
        //    gameData.Load();

        //}

        //startPanel.SetActive(true);
        //levelPanel.SetActive(false);
    }

    //public void PlayGame()
    //{
    //    startPanel.SetActive(false);
    //    levelPanel.SetActive(true);
    //}

    //public void Home()
    //{
    //    startPanel.SetActive(true);
    //    levelPanel.SetActive(false);
    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
