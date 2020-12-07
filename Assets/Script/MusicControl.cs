using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControl : MonoBehaviour
{
    public AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        audio.Play();
        //0 stop
        //1 play
    }

  public void playAudio()
    {
        if (PlayerPrefs.GetInt("musicc")==0)
        {
            audio.Play();
            PlayerPrefs.SetInt("musicc", 1);
        }
    }

    public void stopAudio()
    {
        if (PlayerPrefs.GetInt("musicc") == 1)
        {
            audio.Stop();
            PlayerPrefs.SetInt("musicc", 0);
        }
    }

}
