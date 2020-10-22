﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    wait,
    move
}

public enum TileKind
{
    Breakable,
    Blank,
    Normal
}
[System.Serializable]
public class TileType
{
    public int x;
    public int y;
    public TileKind tileKind;
}

public class Board : MonoBehaviour
{
    public GameState currentState = GameState.move;
    public int width;
    public int height;
    public int offSet;
    public GameObject tilePrefab;
    public GameObject breakableTilePrefab;
    public GameObject[] gamePieces;
    public GameObject destroyEffect;
    public GameObject[,] allDots;
    public Dot currentDot;
    public TileType[] boardLayout;

    bool[,] blankSpaces;
    BackgroundTile[,] breakableTiles;
    FindMatches findMatches;
    // Start is called before the first frame update
    void Start()
    {
        findMatches = FindObjectOfType<FindMatches>();
        blankSpaces = new bool[width, height];
        breakableTiles = new BackgroundTile[width, height];
        allDots = new GameObject[width, height];
        SetUp();
    }

    public void GenerateBlankSpaces()
    {
        for (int i = 0; i < boardLayout.Length; i++)
        {
            if (boardLayout[i].tileKind == TileKind.Blank)
            {
                blankSpaces[boardLayout[i].x, boardLayout[i].y] = true;
            }
        }
    }

    public void GenerateBreakableTiles()
    {
        //look at all tiles in the layout
        for (int i = 0; i < boardLayout.Length; i++)
        {
            //if a tile is "Jelly" tile
            if (boardLayout[i].tileKind == TileKind.Breakable)
            {
                //create a Jelly tile at that position
                Vector2 tempPosition = new Vector2(boardLayout[i].x, boardLayout[i].y);
                GameObject tile = Instantiate(breakableTilePrefab, tempPosition, Quaternion.identity);
                breakableTiles[boardLayout[i].x, boardLayout[i].y] = tile.GetComponent<BackgroundTile>();
            }
        }
    }

    private void SetUp()
    {
        GenerateBlankSpaces();
        GenerateBreakableTiles();
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if (!blankSpaces[i, j]) {
                    Vector2 tempPosition = new Vector2(i, j + offSet);
                    GameObject tile = Instantiate(tilePrefab, tempPosition, Quaternion.identity) as GameObject;
                    tile.transform.parent = this.transform;
                    tile.name = "(" + i + "," + j + ")";
                    int dotToUse = Random.Range(0, gamePieces.Length);
                    int maxIterations = 0;
                    while (MatchesAt(i, j, gamePieces[dotToUse]) && maxIterations < 100)
                    {
                        dotToUse = Random.Range(0, gamePieces.Length);
                        maxIterations++;
                        Debug.Log(maxIterations);
                    }
                    maxIterations = 0;

                    GameObject dot = Instantiate(gamePieces[dotToUse], tempPosition, Quaternion.identity);
                    dot.GetComponent<Dot>().row = j;
                    dot.GetComponent<Dot>().column = i;

                    dot.transform.parent = this.transform;
                    dot.name = "(" + i + "," + j + ")";
                    allDots[i, j] = dot;
                }

            }
        }
    }

    private bool MatchesAt(int column,int row,GameObject piece)
    {
        if(column >1 && row > 1)
        {
            if(allDots[column-1,row] != null && allDots[column-2,row] != null)
            {
                if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
                {
                    return true;
                }
            }
            if(allDots[column,row-1] != null && allDots[column,row-2] != null)
            {
                if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
                {
                    return true;
                }
            }
            
           
        }
        else if(column <= 1 || row <= 1)
        {
            if (row > 1)
            {
                if(allDots[column,row-1] != null && allDots[column,row-2] != null)
                {
                    if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
                    {
                        return true;
                    }
                }
               
            }
            if (column > 1)
            {
                if(allDots[column-1,row] != null && allDots[column-2,row] != null)
                {
                    if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
                    {
                        return true;
                    }
                }
               
            }

        }
        return false;
    }

    private bool ColumnOrRow()
    {
        int numberHorizontal = 0;
        int numberVertical = 0;
        Dot firstPice = findMatches.currentMatches[0].GetComponent<Dot>();

        if(firstPice != null)
        {
            foreach (GameObject currentPice in findMatches.currentMatches)
            {
                Dot dot = currentPice.GetComponent<Dot>();
                if(dot.row == firstPice.row)
                {
                    numberHorizontal++;
                
                }
                if (dot.column == firstPice.column)
                {
                    numberVertical++;

                }
            }
        }

        return (numberVertical == 5 || numberHorizontal == 5);
       
    }

    private void CheckToMakeBomb()
    {
        if(findMatches.currentMatches.Count == 4 || findMatches.currentMatches.Count == 7)
        {
            findMatches.CheckBombs();
        }
        if (findMatches.currentMatches.Count == 5 || findMatches.currentMatches.Count == 8)
        {
            if (ColumnOrRow())
            {
                //make a color bomb
                //is the current dot match?
                if(currentDot != null)
                {
                    if (currentDot.isMatched)
                    {
                        if (!currentDot.isColorBomb)
                        {
                            currentDot.isMatched = false;
                            currentDot.MakeColorBomb();
                        }
                    }
                    else
                    {
                        if(currentDot.otherDot != null)
                        {
                            Dot otherDot = currentDot.otherDot.GetComponent<Dot>();
                            if (otherDot.isMatched)
                            {
                                if (!otherDot.isColorBomb)
                                {
                                    otherDot.isMatched = false;
                                    otherDot.MakeColorBomb();
                                }
                            }
                        }
                    }
                }

            }
            else
            {
                //make a adjacent bomb

                if (currentDot != null)
                {
                    if (currentDot.isMatched)
                    {
                        if (!currentDot.isAdjacentBomb)
                        {
                            currentDot.isMatched = false;
                            currentDot.MakeAdjacentBomb();
                        }
                    }
                    else
                    {
                        if (currentDot.otherDot != null)
                        {
                            Dot otherDot = currentDot.otherDot.GetComponent<Dot>();
                            if (otherDot.isMatched)
                            {
                                if (!otherDot.isAdjacentBomb)
                                {
                                    otherDot.isMatched = false;
                                    otherDot.MakeAdjacentBomb();
                                }
                            }
                        }
                    }
                }
            }

        }
    }

    private void DestroyMatchesAt(int column, int row)
    {
        if (allDots[column, row].GetComponent<Dot>().isMatched)
        {
            //how many elements are in matched pieces list from find matched?
            if(findMatches.currentMatches.Count>=4)
                //if (findMatches.currentMatches.Count == 4 || findMatches.currentMatches.Count == 7)
                {
                CheckToMakeBomb();
                }
            //Does a tile need to break?
            if(breakableTiles[column,row] != null)
            {
                //if it does, give one damage
                breakableTiles[column, row].TakeDamage(1);
                if(breakableTiles[column,row].hitPoint <= 0)
                {
                    breakableTiles[column, row] = null;
                }
            }

            GameObject particle = Instantiate(destroyEffect, allDots[column, row].transform.position, Quaternion.identity);
            Destroy(particle, .5f);
            Destroy(allDots[column, row]);
            allDots[column, row] = null;
        }
    }
    
    public void DestroyMatches()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j=0; j<height; j++)
            {
                if(allDots[i,j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }
        findMatches.currentMatches.Clear();
        StartCoroutine(DecreaseRowCo2());
    }

    private IEnumerator DecreaseRowCo2()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                //if the current spot isn't blank and is empty ...
                if(!blankSpaces[i,j] && allDots[i,j] == null)
                {
                    //loop from the space above to the top of the column
                    for(int k = j + 1; k < height; k++)
                    {
                        //if a dot is found
                        if(allDots[i,k] != null)
                        {
                            //move that dot to empty space
                            allDots[i, k].GetComponent<Dot>().row = j;
                            //set that spot to be null
                            allDots[i, k] = null;
                            //break out of the loop
                            break;
                        }
                    }
                }


            }
        }

        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoardCo());
    }
    private IEnumerator DecreaseRowCo()
    {
        int nullCount = 0;
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if(allDots[i, j] == null)
                {
                    nullCount++;
                } else if (nullCount > 0)
                {
                    allDots[i, j].GetComponent<Dot>().row -= nullCount;
                    allDots[i, j] = null;
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoardCo());
    }

    private void RefillBoard()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if(allDots[i, j] == null && !blankSpaces[i,j])
                {
                    Vector2 tempPosition = new Vector2(i, j+offSet);
                    int dotToUse = Random.Range(0, gamePieces.Length);
                    GameObject piece = Instantiate(gamePieces[dotToUse], tempPosition, Quaternion.identity);
                    allDots[i, j] = piece;
                    piece.GetComponent<Dot>().row = j;
                    piece.GetComponent<Dot>().column = i;

                }
            }
        }
    }

    private bool MatchesOnBoard()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if(allDots[i, j] != null)
                {
                    if (allDots[i, j].GetComponent<Dot>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private IEnumerator FillBoardCo()
    {
        RefillBoard();
        yield return new WaitForSeconds(.5f);

        while (MatchesOnBoard())
        {
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }
        findMatches.currentMatches.Clear();
        currentDot = null;
        yield return new WaitForSeconds(.5f);

        if (IsDeadLocked())
        {
            ShuffleBoars();
            Debug.Log("Dead Locked!!!");
        }
        currentState = GameState.move;
    }

    private void SwitchPieces(int column,int row,Vector2 direction)
    {
        //take the second pice and save it in a holder
        GameObject holder = allDots[column+(int)direction.x, row+(int)direction.y] as GameObject;
        //switching the first dot to be the second position
        allDots[column + (int)direction.x, row + (int)direction.y] = allDots[column, row];
        //first dot to be the second dot
        allDots[column, row] = holder;
    }

    private bool CheckForMatches()
    {
        for(int i = 0; i < width; i++)
        {
            //////////////////////////////////////////////////////////////
            for(int j = 0; j < height; j++)
            {
                if(allDots[i,j] != null)
                {
                    //Make sure that one and two to the right on in the board
                    //Check if the dots to the right and two the right exist
                    if(i<width - 2)
                    {
                        if (allDots[i + 1, j] != null && allDots[i + 2, j] != null)
                        {
                            if (allDots[i + 1, j].tag == allDots[i, j].tag
                                && allDots[i + 2, j].tag == allDots[i, j].tag)
                            {
                                return true;
                            }

                        }
                    }
                    if (j < height - 2)
                    {
                        if (allDots[i, j + 1] != null && allDots[i, j + 2] != null)
                        {
                            if (allDots[i, j + 1].tag == allDots[i, j].tag
                                && allDots[i, j + 2].tag == allDots[i, j].tag)
                            {
                                return true;
                            }

                        }
                    }
                   
                }
            }
        }
        return false;
    }

    private bool SwitchAndCheck(int column,int row,Vector2 direction)
    {
        SwitchPieces(column, row, direction);
        if (CheckForMatches())
        {
            SwitchPieces(column, row, direction);

            return true;
        }
        SwitchPieces(column, row, direction);

        return false;
    }

    private bool IsDeadLocked()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if(allDots[i,j] != null)
                {
                    if (i < width - 1)
                    {
                       if( SwitchAndCheck(i, j, Vector2.right))
                        {
                            return false;
                        }

                    }
                    if (j < height - 1)
                    {
                        if (SwitchAndCheck(i, j, Vector2.up))
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }

    private void ShuffleBoars()
    {
        //create a list of game object
        List<GameObject> newboard = new List<GameObject>();
        //Add every piece to this list
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if(allDots[i,j] != null)
                {
                    newboard.Add(allDots[i, j]);
                }
            }
        }
        //for every spot on the board
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                //if this spot shouldn't be blank
                if (!blankSpaces[i, j])
                {
                    //pick a random number
                    int pieceToUse = Random.Range(0, newboard.Count);
                   
                    //Assign the column to the piece
                    int maxIterations = 0;
                    while (MatchesAt(i, j, newboard[pieceToUse]) && maxIterations < 100)
                    {
                        pieceToUse = Random.Range(0, newboard.Count);
                        maxIterations++;
                        Debug.Log(maxIterations);
                    }
                    //Make the container for the piece
                    Dot piece = newboard[pieceToUse].GetComponent<Dot>();
                    maxIterations = 0;
                    piece.column = i;
                    //Assign the row to the piece
                    piece.row = j;
                    //Fill in the dots Array with this new piece
                    allDots[i, j] = newboard[pieceToUse];
                    //Remove it from the list
                    newboard.Remove(newboard[pieceToUse]);

                }
            }
        }
        //Check if it's still deadlocked
        if (IsDeadLocked())
        {
            ShuffleBoars();
        }
    }
}
