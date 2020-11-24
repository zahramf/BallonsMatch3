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
    int coint=1000;
    BackToSplash backSplash;
    // Start is called before the first frame update
    void Start()
    {
        coinText.text = coint.ToString();


        gameData = FindObjectOfType<GameData>();
        backSplash = FindObjectOfType<BackToSplash>();

      
        Debug.Log("coinManager" + coint);

        int cointt = PlayerPrefs.GetInt("Coin");

        coinText.text = cointt.ToString();


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
