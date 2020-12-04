using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartManager : MonoBehaviour
{
    public AudioSource btnSound;
    public GameObject exitMenu;
    // Start is called before the first frame update
    void Start()
    {
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
