using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public  AudioSource buttonSound;
    public AudioSource destroyNoise;

    public AudioSource backgroundMusic;


    void Start()
    {
        if (PlayerPrefs.HasKey("Sound"))
        {
            if(PlayerPrefs.GetInt("Sound") == 0)
            {
                backgroundMusic.Play();
                backgroundMusic.volume = 0;
            }
            else
            {
                backgroundMusic.Play();
                backgroundMusic.volume = 1;

            }
        }
        else
        {
            backgroundMusic.Play();
            backgroundMusic.volume = 1;
        }
    }
    public void AdjactVolume()
    {
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetInt("Sound") == 0)
            {
                backgroundMusic.volume = 0;

            }
            else
            {
                backgroundMusic.volume = 1;
            }
        }
    }

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
