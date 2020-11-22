using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public GameObject buyMenu;
    public Text coinText;
    GameData gameData;
    public int coin;
    int coint;
    BackToSplash backSplash;
    // Start is called before the first frame update
    void Start()
    {
        coint = 1000;
        //coin+= PlayerPrefs.GetInt("Coin");
        gameData = FindObjectOfType<GameData>();
        backSplash = FindObjectOfType<BackToSplash>();
       coint= PlayerPrefs.GetInt("Coin", 0);
        //coint = PlayerPrefs.GetInt("Coin" +20);
        //coint = BackToSplash.coin;
        //coint = gameData.saveData.coins;
        coinText.text = coint.ToString();
        //gameData.Save();
        //coint += coint;
        Debug.Log("coinManager" + coint);




    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log("cointManager" + coint);
       
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {

                SceneManager.LoadScene("Start");

                //Application.Quit();
            }
        }
    }
    //void OnApplicationPause()
    //{

    //   ga
    //}

    public void Buy()
    {
        buyMenu.SetActive(true);
    }
    public void CloseBuyBtn()
    {
        buyMenu.SetActive(false);
    }
}
