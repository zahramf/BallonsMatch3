﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Dot : MonoBehaviour
{
    [Header("Board Variables")]
    public int column;
    public int row;
    public int previousColumn;
    public int previousRow;
    public int targetX;
    public int targetY;
    public int targetxX;
    public int targetyY;
    public bool isMatched = false;
    public GameObject otherDot;

    Animator anim;
    float shineDelay;
    float shineDelaySeconds;

    EndGameManager endGameManager;
    HintManager hintManager;
    FindMatches findMatches;
    Board board;
    Vector2 firstTouchPosition = Vector2.zero;
    Vector2 finalTouchPosition = Vector2.zero;
    Vector2 tempPosition;

    [Header("Swipe Stuff")]
    public float swipeAngle = 0;
    public float swipeResist = 1f;


    [Header("Powerup Stuff")]
    public bool isColorBomb;
    public bool isRowBomb;
    public bool isColumnBomb;
    public bool isAdjacentBomb;
    public GameObject adjacentMarker;
    public GameObject rowArrow;
    public GameObject columnArrow;
    public GameObject colorBomb;


    // Start is called before the first frame update
    void Start()
    {
        isColumnBomb = false;
        isRowBomb = false;
        isColorBomb = false;
        isAdjacentBomb = false;
        shineDelay = Random.Range(3f, 6f);
        shineDelaySeconds = shineDelay;

        anim = GetComponent<Animator>();

        endGameManager = FindObjectOfType<EndGameManager>();
        hintManager = FindObjectOfType<HintManager>();
        board = GameObject.FindWithTag("Board").GetComponent<Board>();
        //board = FindObjectOfType<Board>();
        findMatches = FindObjectOfType<FindMatches>();
        StartCoroutine(DestroyHint());
        //targetxX = ()transform.position=row;
        //targetyY = (int)transform.position.y;
        //row = targetY;
        //column = targetX;
        //previousRow = row;
        //previousColumn = column;

    }

    //for test and debug
    //private void OnMouseOver()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {

    //        GetRow(1);
    //        Debug.Log("test");
    //    }
    //}
    //void GetRow(int row)
    //{
    //    isMatched = true;
    //    if (isMatched == true)
    //    {
    //        List<GameObject> dots = new List<GameObject>();
    //        for (int i = 0; i < board.width; i++)
    //        {
    //            if (board.allDots[i, row] != null)
    //            {

    //                board.allDots[i, row].GetComponent<Dot>().isMatched = true;
    //            }
    //        }
    //    }
      

    //}
    // Update is called once per frame
    void Update()
    {



        shineDelaySeconds -= Time.deltaTime;
        if (shineDelaySeconds <= 0)
        {
            shineDelaySeconds = shineDelay;
            StartCoroutine(StartShineCo());
        }
        //if (hintManager != null)
        //{
        //    StartCoroutihintManager.DestroyHint());

        //}
        //StartCoroutine(DestroyHint());

        targetX = column;
        targetY = row;
        if (Mathf.Abs(targetX - transform.position.x) > .1)
        {
            //Move Towards the target
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (board.allDots[column, row] != this.gameObject)
            {
                board.allDots[column, row] = this.gameObject;
                findMatches.FindAllMatches();
            }
        }
        else
        {
            //Directly set the position
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;

        }
        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            //Move Towards the target
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (board.allDots[column, row] != this.gameObject)
            {
                board.allDots[column, row] = this.gameObject;
                findMatches.FindAllMatches();

            }

        }
        else
        {
            //Directly set the position
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;

        }
    }

    public IEnumerator CheckMoveCo()
    {
        if (isColorBomb)
        {
            //this piece is color bomb and the other pieces are color to destroy
            findMatches.MatchPiecesOfColor(otherDot.tag);
            isMatched = true;

        }
        else if (otherDot.GetComponent<Dot>().isColorBomb)
        {
            //the other piece is a color bomb and this piece has the color to destroy
            findMatches.MatchPiecesOfColor(this.gameObject.tag);
            otherDot.GetComponent<Dot>().isMatched = true;
        }
        yield return new WaitForSeconds(.5f);
        if (otherDot != null)
        {
            if (!isMatched && !otherDot.GetComponent<Dot>().isMatched)
            {
                otherDot.GetComponent<Dot>().row = row;
                otherDot.GetComponent<Dot>().column = column;
                row = previousRow;
                column = previousColumn;
                yield return new WaitForSeconds(.5f);
                board.currentDot = null;
                board.currentState = GameState.move;

            }
            else
            {
                if (endGameManager != null)
                {
                    if (endGameManager.requirements.gameType == GameType.Moves)
                    {
                        endGameManager.DecreaseCounterValue();
                    }
                }

               
                StartCoroutine(board.DestroyMatches()); 

            }
            //otherDot = null;
        }


    }
    IEnumerator DestroyHint()
    {
        while (true)
        {
            if (hintManager != null)
            {
                yield return new WaitForSeconds(4f);

                hintManager.DestroyHint();
                //hintManager.move = anim.SetBool("touch", false);
                //anim.SetBool("touch", false);


            }
        }
      
    }

    IEnumerator StartShineCo()
    {
        anim.SetBool("shine", true);
        yield return null;
        anim.SetBool("shine", false);

    }

    public void PopAnim()
    {
        anim.SetBool("pop", true);
    }
    public void HintAnim()
    {
        anim.SetBool("touch", true);
    }

    public void DestroyHintAnim()
    {
        anim.SetBool("touch", false);
    }


    private void OnMouseDown()
    {
        //if (anim != null)
        //{
        //    anim.SetBool("touch", false);
        //}
        //Destroy hint
        //if (hintManager != null)
        //{
            
        //    hintManager.DestroyHint();
        //    //hintManager.move = anim.SetBool("touch", false);
        //    //anim.SetBool("touch", false);


        //}
        if (board.currentState == GameState.move)
        {
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        }
        //Debug.Log("finalTouchPosition");
    }

    private void OnMouseUp()
    {
        //anim.SetBool("touch", false);

        if (board.currentState == GameState.move)
        {
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        }

    }

    void CalculateAngle()
    {
        if (Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipeResist || Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist)
        {
            board.currentState = GameState.wait;

            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            MovePieces();
            board.currentDot = this;

        }
        else
        {
            board.currentState = GameState.move;
        }

    }

    void MovePiecesActual(Vector2 direction)
    {
        otherDot = board.allDots[column + (int)direction.x, row + (int)direction.y];
        previousRow = row;
        previousColumn = column;
        if(board.lockTiles[column,row] == null && board.lockTiles[column+(int)direction.x ,row + (int)direction.y] == null)
        {
            if (otherDot != null)
            {
                otherDot.GetComponent<Dot>().column += -1 * (int)direction.x;
                otherDot.GetComponent<Dot>().row += -1 * (int)direction.y;

                column += (int)direction.x;
                row += (int)direction.y;
                StartCoroutine(CheckMoveCo());
            }
            else
            {
                board.currentState = GameState.move;
            }
        }
        else
        {
            board.currentState = GameState.move;
        }

    }

    void MovePieces()
    {
        if (swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1)
        {
            //Right Swipe
            /*
            otherDot = board.allDots[column + 1, row];
            previousRow = row;
            previousColumn = column;
            otherDot.GetComponent<Dot>().column -= 1;
            column += 1;
            StartCoroutine(CheckMoveCo());
            */
            MovePiecesActual(Vector2.right);

        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1)
        {
            //Up Swipe
            /*
            otherDot = board.allDots[column , row+1];
            previousRow = row;
            previousColumn = column;
            otherDot.GetComponent<Dot>().row -= 1;
            row += 1;
            StartCoroutine(CheckMoveCo());
            */
            MovePiecesActual(Vector2.up);

        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
        {
            //Left Swipe
            /*
            otherDot = board.allDots[column - 1, row];
            previousRow = row;
            previousColumn = column;
            otherDot.GetComponent<Dot>().column += 1;
            column -= 1;
            StartCoroutine(CheckMoveCo());
            */
            MovePiecesActual(Vector2.left);

        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            //Down Swipe
            /*
            otherDot = board.allDots[column , row-1];
            previousRow = row;
            previousColumn = column;
            otherDot.GetComponent<Dot>().row += 1;
            row -= 1;
            StartCoroutine(CheckMoveCo());
            */
            MovePiecesActual(Vector2.down);

        }
        else
        {
            board.currentState = GameState.move;

        }


    }

    void FindMatches()
    {
        if (column > 0 && column < board.width - 1)
        {
            GameObject leftDot1 = board.allDots[column - 1, row];
            GameObject rightDot1 = board.allDots[column + 1, row];

            if (leftDot1 != null && rightDot1 != null)
            {
                if (leftDot1.tag == this.gameObject.tag && rightDot1.tag == this.gameObject.tag)
                {
                    leftDot1.GetComponent<Dot>().isMatched = true;
                    rightDot1.GetComponent<Dot>().isMatched = true;
                    isMatched = true;

                }
            }

        }
        if (row > 0 && row < board.height - 1)
        {
            GameObject upDot1 = board.allDots[column, row + 1];
            GameObject downDot1 = board.allDots[column, row - 1];
            if (upDot1 != null && downDot1 != null)
            {
                if (upDot1.tag == this.gameObject.tag && downDot1.tag == this.gameObject.tag)
                {
                    upDot1.GetComponent<Dot>().isMatched = true;
                    downDot1.GetComponent<Dot>().isMatched = true;
                    isMatched = true;

                }
            }


        }
    }

    public void MakeRowBomb()
    {
        if(!isColumnBomb && !isColorBomb && !isAdjacentBomb)
        {
            isRowBomb = true;
            GameObject arrow = Instantiate(rowArrow, transform.position, Quaternion.identity);
            arrow.transform.parent = this.transform;
            //Debug.Log("MakeRowBomb");
        }
       
    }

    public void MakeColumnBomb()
    {
        if(!isRowBomb && !isColorBomb && !isAdjacentBomb)
        {
            isColumnBomb = true;
            GameObject arrow = Instantiate(columnArrow, transform.position, Quaternion.identity);
            arrow.transform.parent = this.transform;
            ////this.gameObject.tag = "Color";
        }


    }

    public void MakeColorBomb()
    {
        if(!isColumnBomb && !isRowBomb && !isAdjacentBomb)
        {
            isColorBomb = true;
            GameObject color = Instantiate(colorBomb, transform.position, Quaternion.identity);
            color.transform.parent = this.transform;
            this.gameObject.tag = "Color";
        }
       
    }

    public void MakeAdjacentBomb()
    {
        if(!isColumnBomb && !isRowBomb && !isColorBomb)
        {
            isAdjacentBomb = true;
            GameObject marker = Instantiate(adjacentMarker, transform.position, Quaternion.identity);
            marker.transform.parent = this.transform;
            //Debug.Log("MakeAdjacentBomb");
        }
       
    }
}
