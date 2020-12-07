using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    private Board board;
    public float hintDelay;
    private float hintDelaySeconds;
    public GameObject hintParticle;
    public GameObject currentHint;
    public List<GameObject > possible;
    Animator anim;
   public GameObject move;

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        anim = GetComponent<Animator>();

        hintDelaySeconds = hintDelay;
        List<GameObject> possible = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        hintDelaySeconds -= Time.deltaTime;
        if(hintDelaySeconds<=0 && currentHint == null)
        {
            MarkHint();
            hintDelaySeconds = hintDelay;
        }
    }

    //First, I want to find all possible matches on the board
    List<GameObject> FindAllMatches()
    {
        List<GameObject> possibleMoves = new List<GameObject>();
        for (int i = 0; i <board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                if (board.allDots[i, j] != null)
                {
                    if (i < board.width - 1)
                    {
                        if (board.SwitchAndCheck(i, j, Vector2.right))
                        {
                            possibleMoves.Add(board.allDots[i, j]);
                            //possible.Add(board.allDots[i, j]);

                        }

                    }
                    if (j < board.height - 1)
                    {
                        if (board.SwitchAndCheck(i, j, Vector2.up))
                        {
                            possibleMoves.Add(board.allDots[i, j]);
                            //possible.Add(board.allDots[i, j]);


                        }
                    }
                }
            }
        }
        return possibleMoves;
        //return possible;


    }
    //Pick one of those matches randomly

    GameObject PickOneRandomly()
    {
        List<GameObject> possibleMoves = new List<GameObject>();
        possibleMoves = FindAllMatches();
        if (possibleMoves.Count > 0)
        {
            int pieceToUse = Random.Range(0, possibleMoves.Count);
            return possibleMoves[pieceToUse];
        }
        return null;
    }

   
    //Create the hint behind the chosen match
    private void MarkHint()
    {
         move = PickOneRandomly();

        //GameObject move = PickOneRandomly();
        if (move != null)
        {
            move.GetComponent<Dot>().HintAnim();
            currentHint = Instantiate(hintParticle, move.transform.position, Quaternion.identity);
        }
    }
    //Destroy the hint
    public void DestroyHint()
    {
        if (currentHint != null)
        {
            //yield return new WaitForSeconds(1f);
            move.GetComponent<Dot>().DestroyHintAnim();
            Destroy(currentHint);
            currentHint = null;
            hintDelaySeconds = hintDelay;

        }
    }

}
