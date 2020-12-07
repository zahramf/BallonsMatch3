using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    public Image musicButton;
    //public Sprite soundOnSprite;
    //public Sprite soundOffSprite;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;
    public  AudioSource buttonSound;
    public AudioSource destroyNoise;


    public AudioClip gameMusic;

     AudioSource backgroundMusic;


    void Start()
    {
        backgroundMusic = GetComponent<AudioSource>();
        Debug.Log("Game : " + PlayerPrefs.GetInt("musicGame"));
        SoundScript.Play(gameMusic);

        //if (PlayerPrefs.HasKey("Music"))
        //{
        //    if (PlayerPrefs.GetInt("Music") == 0)
        //    {
        //        musicButton.sprite = musicOffSprite;
        //        SoundScript.Pause(gameMusic, 0);
        //    }
        //    else
        //    {
        //        musicButton.sprite = musicOnSprite;
        //        SoundScript.Play(gameMusic);

        //    }
        //}
        //else
        //{
        //    musicButton.sprite = musicOnSprite;

        //    SoundScript.Play(gameMusic);


        //}
    }

    public void MusicButton()
    {

        if (PlayerPrefs.HasKey("Music"))
        {
            if (PlayerPrefs.GetInt("Music") == 0)
            {
                musicButton.sprite = musicOnSprite;
                SoundScript.Play(gameMusic);

                PlayerPrefs.SetInt("Music", 1);
            }
            else
            {
                musicButton.sprite = musicOffSprite;
                //SoundScript.Pause(gameMusic, 0);

                PlayerPrefs.SetInt("Music", 0);

            }
        }
        else
        {
            musicButton.sprite = musicOnSprite;
            SoundScript.Play(gameMusic);
            PlayerPrefs.SetInt("Music", 1);

        }
        Debug.Log("music" + PlayerPrefs.GetInt("Music"));
    }

    //public void AdjactVolume()
    //{
    //    if (PlayerPrefs.HasKey("Music"))
    //    {
    //        if (PlayerPrefs.GetInt("Music") == 0)
    //        {
    //            backgroundMusic.volume = 0;

    //        }
    //        else
    //        {
    //            backgroundMusic.volume = 1;
    //        }
    //    }
    //}

    public void PlayButtonSound()
    {
        buttonSound.Play();
    }

    public void PlayRandomDestroyNoise()
    {
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetInt("Sound") == 1)
            {
                //Choose a random number
                //int clipToPlay = Random.Range(0, destroyNoise.Length);
                //Play that clip
                destroyNoise.Play();
            }
        }
        else
        {
            //Choose a random number
            //int clipToPlay = Random.Range(0, destroyNoise.Length);
            //Play that clip
            destroyNoise.Play();
        }


    }
}
