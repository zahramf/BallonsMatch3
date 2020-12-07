using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartManager : MonoBehaviour
{
    [Header("Sound")]
    public AudioClip startMusic;
    AudioSource backgroundMusic;
    public AudioSource btnSound;


    [Header("UI")]
    public GameObject settingPanel;
    public GameObject exitMenu;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("start" + PlayerPrefs.GetInt("musicGame"));

        backgroundMusic = GetComponent<AudioSource>();
        SoundScript.Play(startMusic);
        settingPanel.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {


        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                exitMenu.SetActive(true);
                //SceneManager.LoadScene("Start");

                //Application.Quit();
            }
        }
    }

    public void GameSound()
    {
       int sound= PlayerPrefs.GetInt("musicGame");
        Debug.Log("musicGame" + PlayerPrefs.GetInt("musicGame"));
        if (sound == 1)
        {
            Debug.Log("sound" + sound);
            SoundScript.Play(startMusic);
            //GameObject.Find("musicGame").GetComponent<AudioSource>().volume = 1;
            PlayerPrefs.SetInt("musicGame", 0);
            //sound = 0;
        }
        else if (sound == 0)
        {
            SoundScript.Play(startMusic);
            //GameObject.Find("musicGame").GetComponent<AudioSource>().volume = 1;

            //sound = 1;
            Debug.Log("sound" + sound);
            PlayerPrefs.SetInt("musicGame", 1);

        }
    }

    public void SettingButton()
    {
        settingPanel.SetActive(true);
    }

    public void ExitSettingButton()
    {
        settingPanel.SetActive(false);
    }

    public void OkExit()
    {
        btnSound.Play();
        Application.Quit();
    }

   public void NoExit()
    {
        btnSound.Play();
        exitMenu.SetActive(false);

        //this.gameObject.SetActive(false);
    }
}
