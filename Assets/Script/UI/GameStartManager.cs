using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartManager : MonoBehaviour
{
    public  GameObject Load;
    public AudioSource button;
    SoundManager soundManager;
    //GameData gameData;

    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();

        //gameData = FindObjectOfType<GameData>();
        //if(gameData != null)
        //{
        //    gameData.Load();

        //}

        //startPanel.SetActive(true);
        //levelPanel.SetActive(false);
    }
    public void StartGame()
    {
        button.Play();
        Load.SetActive(true);

        SceneManager.LoadScene("Splash");
    }
    //public GameObject startPanel;
    //public GameObject levelPanel;
    //// Start is called before the first frame update
   

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
