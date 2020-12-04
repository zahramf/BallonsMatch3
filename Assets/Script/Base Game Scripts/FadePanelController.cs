using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadePanelController : MonoBehaviour
{

    public Animator panelAnim;
    public Animator gameInfoAnim;
    SoundManager soundManager;
    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }
    void Play()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }
   
    public void Ok()
    {
        PlayerPrefs.SetInt("winpanel", 0);

        if (panelAnim != null && gameInfoAnim != null)
        {
            soundManager.PlayButtonSound();
            panelAnim.SetBool("Out", true);
            gameInfoAnim.SetBool("Out", true);
            StartCoroutine(GameStartCo());
        }
       
    }

    public void GameOver()
    {
        panelAnim.SetBool("Out", false);
        panelAnim.SetBool("Game Over", true);


    }

    IEnumerator GameStartCo()
    {
        yield return new WaitForSeconds(1f);
        Board board = FindObjectOfType<Board>();
        board.currentState = GameState.move;
    }
}
