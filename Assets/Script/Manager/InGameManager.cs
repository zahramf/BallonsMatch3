using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameManager : MonoBehaviour
{
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
        SceneManager.LoadScene("Splash");
    }

    public void NoExit()
    {
        exitMenu.SetActive(false);

        //this.gameObject.SetActive(false);
    }
}
