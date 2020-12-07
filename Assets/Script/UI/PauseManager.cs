using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    //public Animator settingAnim;
    public GameObject pausePanel;
    Board board;
    public bool paused = false;
    public Image musicButton;
    //public Sprite soundOnSprite;
    //public Sprite soundOffSprite;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;
    SoundManager sound;
    public Button btnBack;

    // Start is called before the first frame update
    void Start()
    {
        sound = FindObjectOfType<SoundManager>();
        board = FindObjectOfType<Board>();
        btnBack = GetComponent<Button>();
        pausePanel.SetActive(false);
        //In Player Prefs, the "Music" key is for sound
        //If Music==0 then mute, If Music==1 unmute
        //if (PlayerPrefs.HasKey("Music"))
        //{
        //    if(PlayerPrefs.GetInt("Music") == 0)
        //    {
        //        musicButton.sprite = musicOffSprite;
        //    }
        //    else
        //    {
        //        musicButton.sprite = musicOnSprite;
        //    }
        //}
        //else
        //{
        //    musicButton.sprite = musicOnSprite;
        //}
        pausePanel.SetActive(false);
        board = GameObject.FindWithTag("Board").GetComponent<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        if (paused && !pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(true);
            board.currentState = GameState.pause;
        }
        if (!paused && pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(false);
            board.currentState = GameState.move;
        }
    }

    //public void Sound()
    //{
    //    if (PlayerPrefs.HasKey("Music"))
    //    {
    //        if(PlayerPrefs.GetInt("Music") == 0)
    //        {
    //            PlayerPrefs.SetInt("Music", 1);
    //            musicButton.sprite = musicOnSprite;
    //            sound.AdjactVolume();
    //        }
    //        else
    //        {
    //            PlayerPrefs.SetInt("Music", 0);
    //            musicButton.sprite = musicOffSprite;
    //            sound.AdjactVolume();

    //        }
    //    }
    //    else
    //    {
    //        PlayerPrefs.SetInt("Music", 0);
    //        musicButton.sprite = musicOffSprite;
    //        sound.AdjactVolume();

    //    }
    //}

    public void MusicButton()
    {
        if (PlayerPrefs.HasKey("Music"))
        {
            if (PlayerPrefs.GetInt("Music") == 0)
            {
                musicButton.sprite = musicOnSprite;
                PlayerPrefs.SetInt("Music", 1);
            }
            else
            {
                musicButton.sprite = musicOffSprite;
                PlayerPrefs.SetInt("Music", 0);

            }
        }
        else
        {
            musicButton.sprite = musicOffSprite;
            PlayerPrefs.SetInt("Music", 1);

        }
    }
    //public void SoundButton()
    //{
    //    if (PlayerPrefs.HasKey("Sound"))
    //    {
    //        if (PlayerPrefs.GetInt("Sound") == 0)
    //        {
    //            musicButton.sprite = musicOnSprite;
    //            PlayerPrefs.SetInt("Sound", 1);
    //        }
    //        else
    //        {
    //            musicButton.sprite = musicOffSprite;
    //            PlayerPrefs.SetInt("Sound", 0);

    //        }
    //    }
    //    else
    //    {
    //        musicButton.sprite = musicOffSprite;
    //        PlayerPrefs.SetInt("Sound", 1);

    //    }
    //}
    public void ExitSetting()
    {
        paused = false;

    }

    //IEnumerator ExitSettingMenu()
    //{
    //    yield return new WaitForSeconds(1f);
    //    pausePanel.SetActive(false);
    //}
    public void PauseGame()
    {
        paused = !paused;
        //if (paused == true)
        //{
        //    btnBack = GameObject.FindGameObjectWithTag("back").GetComponent<Button>();
        //    btnBack.interactable = true;
        //    board.currentState = GameState.pause;
        //}


       
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("Splash");
    }
}
