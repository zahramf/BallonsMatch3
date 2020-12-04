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
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;
    SoundManager sound;

    // Start is called before the first frame update
    void Start()
    {
        sound = FindObjectOfType<SoundManager>();
        board = FindObjectOfType<Board>();
        pausePanel.SetActive(false);
        //In Player Prefs, the "Sound" key is for sound
        //If sound==0 then mute, If Sound==1 unmute
        if (PlayerPrefs.HasKey("Sound"))
        {
            if(PlayerPrefs.GetInt("Sound") == 0)
            {
                musicButton.sprite = soundOffSprite;
            }
            else
            {
                musicButton.sprite = soundOnSprite;
            }
        }
        else
        {
            musicButton.sprite = soundOnSprite;
        }
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

    public void Sound()
    {
        if (PlayerPrefs.HasKey("Sound"))
        {
            if(PlayerPrefs.GetInt("Sound") == 0)
            {
                PlayerPrefs.SetInt("Sound", 1);
                musicButton.sprite = musicOnSprite;
                sound.AdjactVolume();
            }
            else
            {
                PlayerPrefs.SetInt("Sound", 0);
                musicButton.sprite = musicOffSprite;
                sound.AdjactVolume();

            }
        }
        else
        {
            PlayerPrefs.SetInt("Sound", 0);
            musicButton.sprite = musicOffSprite;
            sound.AdjactVolume();

        }
    }
    public void SoundButton()
    {
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetInt("Sound") == 0)
            {
                musicButton.sprite = musicOnSprite;
                PlayerPrefs.SetInt("Sound", 1);
            }
            else
            {
                musicButton.sprite = musicOffSprite;
                PlayerPrefs.SetInt("Sound", 0);

            }
        }
        else
        {
            musicButton.sprite = musicOffSprite;
            PlayerPrefs.SetInt("Sound", 1);

        }
    }
    //public void ExitSetting()
    //{
    //    if (paused == true)
    //    {
    //        settingAnim.SetBool("OutSetting", true);

    //        StartCoroutine(ExitSettingMenu());
    //        board.currentState = GameState.move;
    //        paused = false;


    //    }

    //}

    //IEnumerator ExitSettingMenu()
    //{
    //    yield return new WaitForSeconds(1f);
    //    pausePanel.SetActive(false);
    //}
    public void PauseGame()
    {
        //if (paused == false)
        //{
        //    paused = true;
        //    pausePanel.SetActive(true);
        //    board.currentState = GameState.pause;
        //}


        paused = !paused;
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("Splash");
    }
}
