using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class Manager : MonoBehaviour
{
    public GameObject buyMenu;
    public GameObject getEnergy;

    public Text coinText;
    GameData gameData;
    public int coin =1000;
    //int coint=1000;
    BackToSplash backSplash;

    [SerializeField]
    Text textEnergy;

    [SerializeField]
    Text textTimer;

    [SerializeField]
    int maxEnergy;

    int totalEnergy;
    DateTime nextEnergyTime;
    DateTime lastAddedTime;
    int restoreDuration = 50; //10 second for testing puspose

    bool restoring = false;

    // Start is called before the first frame update
    void Start()
    {
        Loade();

        StartCoroutine(RestoreRoutine());

        gameData = FindObjectOfType<GameData>();
        backSplash = FindObjectOfType<BackToSplash>();

      
        //Debug.Log("coinManager" + coint);

         coin = PlayerPrefs.GetInt("Coin");
        int firstCoin = PlayerPrefs.GetInt("First");
        if(coin == 0 && firstCoin != 1)
        {
            coinText.text = "" + 1000;
        }
        else
        {
            coinText.text = coin.ToString();
        }
       
        
        


    }

    IEnumerator RestoreRoutine()
    {
        UpdateTimer();
        UpdateEnergy();

        restoring = true;

        while (totalEnergy < maxEnergy)
        {
            DateTime currentTime = DateTime.Now;
            DateTime counter = nextEnergyTime;

            bool isAdding = false;
            while(currentTime > counter)
            {
                if (totalEnergy < maxEnergy)
                {
                    isAdding = true;
                    totalEnergy++;
                    DateTime timeToAdd = lastAddedTime > counter ? lastAddedTime : counter;
                    counter = AddDuration(timeToAdd, restoreDuration);
                }
                else
                {
                    break;
                }
            }

            if (isAdding)
            {
                lastAddedTime = DateTime.Now;
                nextEnergyTime = counter;
            }
            UpdateTimer();
            UpdateEnergy();
            Save();
            yield return null;
        }
        restoring = false;
    }

    void UpdateTimer()
    {
        if(totalEnergy >= maxEnergy)
        {
            textTimer.text = "ﻞﻣﺎﮐ";
            return;
        }
        TimeSpan t = nextEnergyTime - DateTime.Now;
        string value = String.Format("{0:00}:{1:00}", t.TotalMinutes, t.TotalSeconds);
        textTimer.text = value;
    }

    void UpdateEnergy()
    {
        textEnergy.text = totalEnergy.ToString();
    }

    DateTime AddDuration(DateTime time, int duration)
    {
        //return time.AddMinutes(duration);

        return time.AddSeconds(duration);
    }

    public void UseEnergyMethod()
    {
        //if (totalEnergy == 0)

        //    return;
        totalEnergy--;
        UpdateEnergy();

        if (!restoring)
        {
            if (totalEnergy + 1 == maxEnergy)
            {
                //if energy is full just now
                nextEnergyTime = AddDuration(DateTime.Now, restoreDuration);
            }
            StartCoroutine(RestoreRoutine());
        }
    }
    public void FullEnergyMethod()
    {
        if (totalEnergy == 5)

            return;
        totalEnergy=5;
        UpdateEnergy();

        if (!restoring)
        {
            if (totalEnergy + 1 == maxEnergy)
            {
                //if energy is full just now
                nextEnergyTime = AddDuration(DateTime.Now, restoreDuration);
            }
            StartCoroutine(RestoreRoutine());
        }
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
    public void Energy()
    {
        getEnergy.SetActive(true);

        Button btn = GameObject.FindGameObjectWithTag("FullEnergy").GetComponent<Button>();


        int coinEnergy = PlayerPrefs.GetInt("Coin");
          if (coin < 900)
            {
                btn.interactable = false;
                //GetComponent<Button>().interactable = false;
                //btn.interactable = false;
            }
            else
            {
                btn.enabled = true;
                //GetComponent<Button>().interactable = true;

                //btn.interactable = true;
            }
        
       
    }
    public void CloseBuyBtn()
    {
        buyMenu.SetActive(false);
    }
    public void CloseEnergyBtn()
    {
        getEnergy.SetActive(false);
    }

    void Loade()
    {
        totalEnergy = PlayerPrefs.GetInt("totalEnergy");
        nextEnergyTime = StringToDate(PlayerPrefs.GetString("nextEnergyTime"));
        lastAddedTime = StringToDate(PlayerPrefs.GetString("lastAddedTime"));

    }
    void Save()
    {
        PlayerPrefs.SetInt("totalEnergy", totalEnergy);
        PlayerPrefs.SetString("nextEnergyTime", nextEnergyTime.ToString());
        PlayerPrefs.SetString("lastAddedTime", lastAddedTime.ToString());

    }

    DateTime StringToDate(string date)
    {
        if (String.IsNullOrEmpty(date))
      
            return DateTime.Now;
        return DateTime.Parse(date);
      
    } 
}
