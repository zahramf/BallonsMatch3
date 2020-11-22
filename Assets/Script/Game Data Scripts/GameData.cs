using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class SaveData
{

    public bool[] isActive;
    public int[] highScores;
    public int[] stars;
    public int coins;
}


public class GameData : MonoBehaviour
{
    public static GameData gameData;
    public SaveData saveData;

    // Start is called before the first frame update
    void Awake()
    {
        if (gameData == null)
        {
            DontDestroyOnLoad(this.gameObject);
            gameData = this;

        }
        else
        {
            Destroy(this.gameObject);
            //Load();

        }
        Load();

    }

    void Start()
    {
        Load();
        //Debug.Log(Application.persistentDataPath);
    }

    public void Save()
    {
        //Create a binary formatter witch can read binary files
        BinaryFormatter formatter = new BinaryFormatter();

        //Create a route from the program to file
        //FileStream file = File.Open(Application.persistentDataPath + "/player.dat", FileMode.Create);
        FileStream file = File.Create(Application.persistentDataPath + "/player.txt");


        //Create a copy of my save data
        SaveData data = new SaveData();


        //test 
        //for (int i = 0; i < data.isActive.Length; i++)
        //    Debug.Log("Score : " + i + " :" + data.isActive[i]);

        //
        data = saveData;


        //Actually save the data in a file
        formatter.Serialize(file, data);

        //Close the data stream
        file.Close();


    }

    public void Load()
    {
        //Check if the game file exists
        if (File.Exists(Application.persistentDataPath + "/player.txt"))
        {
            //Create a binary formatter
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/player.txt", FileMode.Open);
            saveData = formatter.Deserialize(file) as SaveData;
            //for (int i = 0; i < saveData.isActive.Length; i++)
            //    Debug.Log("Score : " + i + " :  " + saveData.isActive[i]);
            file.Close();
            Debug.Log("Loaded");
        }

        else
        {
            //Debug.Log("File does not exsit !");
            //saveData = new SaveData();
            //saveData.isActive = new bool[20];
            //saveData.stars = new int[20];
            //saveData.highScores = new int[20];
            //saveData.isActive[0] = true;
        }
    }

    void OnApplicationPause()
    {

        Save();
    }

    void OnDisable()
    {
        Save();
    }
 

    // Update is called once per frame
    void Update()
    {

    }
}
