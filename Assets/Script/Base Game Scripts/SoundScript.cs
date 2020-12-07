using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour
{
  static  SoundScript Instance;
    [SerializeField]
     AudioSource musicSource = null;


    public static void Play(AudioClip clip)
    {
        if (PlayerPrefs.GetInt("musicGame") == 1)
        {
            Debug.Log("play");
            if (clip == null)
                Instance.musicSource.Stop();
            if (Instance.musicSource.clip == clip && Instance.musicSource.isPlaying)
            {
                return;
            }
            Instance.musicSource.clip = clip;

            Instance.musicSource.Play();
            Instance.musicSource.volume = 1;
        }
        else if (PlayerPrefs.GetInt("musicGame") == 0)
        {
            Debug.Log("pause");

            Instance.musicSource.clip = null;
            Instance.musicSource.Play();
            Instance.musicSource.volume = 0;
          
        }
       
    }

    void Awake()
    {
        Debug.Log("awake" + PlayerPrefs.GetInt("musicGame"));
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(Instance.gameObject);
    }
}
