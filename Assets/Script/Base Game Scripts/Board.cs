using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum GameState
{
    wait,
    move,
    win,
    lose,
    pause
}

public enum TileKind
{
    Breakable,
    Blank,
    Lock,
    Concrete,
    slime,
    Normal
}
[System.Serializable]
public class MatchType
{
    public int type;
    public string color;
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
    [Header("Scriptable Object Stuff")]
    public World world;
    public int level;

    public GameState currentState = GameState.move;

    [Header("Board Dimensions")]
    public int width;
    public int height;
    public int offSet;

    public GameType gameType;
    public int counterValue;


  
    public BlankGoal[] levelGoals;

    [Header("Prefabs")]
    public GameObject tilePrefab;
    public GameObject breakableTilePrefab;
    public GameObject lockTilePrefab;
    public GameObject concreteTilePrefab;
    public GameObject slimePiecePrefab;
    public GameObject[] gamePieces;
    public GameObject destroyEffect;

    [Header("Layaout")]
    public GameObject[,] allDots;

    [Header("Match Stuff")]
    public MatchType matchType;
    public Dot currentDot;
    FindMatches findMatches;
    public int basePieceValue = 20;
    int streakValue = 1;
    ScoreManager scoreManager;
    SoundManager soundManager;
    GoalManager goalMAnager;
    EndGameRequirement endGame;
    EndGameManager endGameManager;
    BlankGoal[] goalLevel;
    public float refillDelay = 0.0000001f;
    public int[] scoreGoals;
    bool makeSlime = true;

    public TileType[] boardLayout;
    bool[,] blankSpaces;
    BackgroundTile[,] breakableTiles;
   public BackgroundTile[,] lockTiles;
    BackgroundTile[,] concreteTiles;
    BackgroundTile[,] slimeTiles;
   
    



    void Awake()
    {
        //if (world != null)
        //{
            if (PlayerPrefs.HasKey("Current Level"))
            {
                level = PlayerPrefs.GetInt("Current Level");
            }
            if(world != null)
        {
            if (level < world.levels.Length)
            {
                if (world.levels[level] != null)
                {
                    width = world.levels[level].width;
                    height = world.levels[level].height;
                    gamePieces = world.levels[level].gamePieces;
                    scoreGoals = world.levels[level].scoreGoals;
                    boardLayout = world.levels[level].boardLayout;
                    gameType = world.levels[level].endGameReqirements.gameType;
                    counterValue = world.levels[level].endGameReqirements.counterValue;
                    levelGoals = world.levels[level].levelGoals;
                }
            }
        }

       

        //}
    }

    //private void OnMouseOver()
    //{
    //    List<GameObject> currentMatches = new List<GameObject>();
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        GetRow(1);

    //        //currentMatches.Union(GetRow(1));
    //        Debug.Log("test");
    //    }
    //}
    //List<GameObject> GetRow(int row)
    //{

    //    List<GameObject> dots = new List<GameObject>();
    //    for (int i = 0; i < width; i++)
    //    {
    //        if (allDots[i, row] != null)
    //        {

    //            dots.Add(allDots[i, row]);
    //            allDots[i, row].GetComponent<Dot>().isMatched = true;
    //        }
    //    }
    //    return dots;
    //    //Debug.Log("Dots" + dots);

    //}

    // Start is called before the first frame update
    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        goalMAnager = FindObjectOfType<GoalManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        findMatches = FindObjectOfType<FindMatches>();
        endGameManager = FindObjectOfType<EndGameManager>();
        //endGame = FindObjectOfType<EndGameRequirement>();
        blankSpaces = new bool[width, height];
        breakableTiles = new BackgroundTile[width, height];
        lockTiles = new BackgroundTile[width, height];
        concreteTiles = new BackgroundTile[width, height];
        slimeTiles = new BackgroundTile[width, height];

        allDots = new GameObject[width, height];
        SetUp();
        currentState = GameState.pause;
        
    }
    public void LoseManager()
    {
        int lvl = PlayerPrefs.GetInt("OnLevel");
        Debug.Log("onlevel" + lvl);
        world.levels[lvl - 1].endGameReqirements.counterValue = 5;
        Debug.Log("counter" + world.levels[lvl - 1].endGameReqirements.counterValue);
    }

    public void LoseEnergy()
    {
        Debug.Log("Eeeeeeeeeeeeee");
        int lvl = PlayerPrefs.GetInt("OnLevel");
        Debug.Log("onlevel" + lvl);
        width = world.levels[lvl-1].width;
        Debug.Log("width" + world.levels[lvl - 1].width);
        height = world.levels[lvl-1].height;
        gamePieces = world.levels[lvl-1].gamePieces;
        scoreGoals = world.levels[lvl-1].scoreGoals;
        Debug.Log("scoreGoals" + world.levels[lvl - 1].scoreGoals);

        boardLayout = world.levels[lvl-1].boardLayout;
        gameType = world.levels[lvl-1].endGameReqirements.gameType;

        counterValue = world.levels[lvl - 1].endGameReqirements.counterValue;
        world.levels[lvl - 1].endGameReqirements.counterValue = counterValue;
        levelGoals = world.levels[lvl-1].levelGoals;
        goalMAnager.UpdateGoals();

        
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

    void GenerateLockTiles()
    {
        //look at all tiles in the layout
        for (int i = 0; i < boardLayout.Length; i++)
        {
            //if a tile is "Lock" tile
            if (boardLayout[i].tileKind == TileKind.Lock)
            {
                //create a Lock tile at that position
                Vector2 tempPosition = new Vector2(boardLayout[i].x, boardLayout[i].y);
                GameObject tile = Instantiate(lockTilePrefab, tempPosition, Quaternion.identity);
                lockTiles[boardLayout[i].x, boardLayout[i].y] = tile.GetComponent<BackgroundTile>();
            }
        }
    }

    void GenerateConcreteTiles()
    {
        //look at all tiles in the layout
        for (int i = 0; i < boardLayout.Length; i++)
        {
            //if a tile is "Lock" tile
            if (boardLayout[i].tileKind == TileKind.Concrete)
            {
                //create a Lock tile at that position
                Vector2 tempPosition = new Vector2(boardLayout[i].x, boardLayout[i].y);
                GameObject tile = Instantiate(concreteTilePrefab, tempPosition, Quaternion.identity);
                concreteTiles[boardLayout[i].x, boardLayout[i].y] = tile.GetComponent<BackgroundTile>();
            }
        }
    }

    void GenerateSlimeTiles()
    {
        //look at all tiles in the layout
        for (int i = 0; i < boardLayout.Length; i++)
        {
            //if a tile is "Lock" tile
            if (boardLayout[i].tileKind == TileKind.slime)
            {
                //create a Lock tile at that position
                Vector2 tempPosition = new Vector2(boardLayout[i].x, boardLayout[i].y);
                GameObject tile = Instantiate(slimePiecePrefab, tempPosition, Quaternion.identity);
                slimeTiles[boardLayout[i].x, boardLayout[i].y] = tile.GetComponent<BackgroundTile>();
            }
        }
    }
    private void SetUp()
    {
        GenerateBlankSpaces();
        GenerateBreakableTiles();
        GenerateLockTiles();
        GenerateConcreteTiles();
        GenerateSlimeTiles();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (!blankSpaces[i, j] && !concreteTiles[i,j] && !slimeTiles[i,j])
                {
                    Vector2 tempPosition = new Vector2(i, j + offSet);
                    Vector2 tilePosition = new Vector2(i, j);
                    GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                    //if (Input.GetMouseButtonDown(0))
                    //{
                    //    Debug.Log("Position :" + tilePosition);
                    //}
                    tile.transform.parent = this.transform;
                    tile.name = "(" + i + "," + j + ")";
                    int dotToUse = Random.Range(0, gamePieces.Length);
                    int maxIterations = 0;
                    while (MatchesAt(i, j, gamePieces[dotToUse]) && maxIterations < 100)
                    {
                        dotToUse = Random.Range(0, gamePieces.Length);
                        //Debug.Log("Maaaaaaaaaaaaaaaaatch");
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

    private bool MatchesAt(int column, int row, GameObject piece)
    {
        if (column > 1 && row > 1)
        {
            if (allDots[column - 1, row] != null && allDots[column - 2, row] != null)
            {
                if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
                {
                    return true;
                }
            }
            if (allDots[column, row - 1] != null && allDots[column, row - 2] != null)
            {
                if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
                {
                    return true;
                }
            }


        }
        else if (column <= 1 || row <= 1)
        {
            if (row > 1)
            {
                if (allDots[column, row - 1] != null && allDots[column, row - 2] != null)
                {
                    if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
                    {
                        return true;
                    }
                }

            }
            if (column > 1)
            {
                if (allDots[column - 1, row] != null && allDots[column - 2, row] != null)
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

    void DamageConcrete(int column,int row)
    {
        if (column > 0)
        {
            if(concreteTiles[column - 1, row])
            {
                concreteTiles[column - 1, row].TakeDamage(1);
                if (concreteTiles[column - 1, row].hitPoint <= 0)
                {
                    concreteTiles[column - 1, row] = null;
                }
            }
        }

        if (column < width - 1)
        {
            if (concreteTiles[column + 1, row])
            {
                concreteTiles[column + 1, row].TakeDamage(1);

                if (concreteTiles[column + 1, row].hitPoint <= 0)
                {
                    concreteTiles[column + 1, row] = null;
                }
            }
        }

        if (row > 0)
        {
            if (concreteTiles[column , row - 1])
            {
                concreteTiles[column, row - 1].TakeDamage(1);

                if (concreteTiles[column , row - 1].hitPoint <= 0)
                {
                    concreteTiles[column , row - 1] = null;
                }
            }
        }

        if (row < height - 1)
        {
            if (concreteTiles[column , row + 1])
            {
                concreteTiles[column, row + 1].TakeDamage(1);

                if (concreteTiles[column , row + 1].hitPoint <= 0)
                {
                    concreteTiles[column , row + 1] = null;
                }
            }
        }
    }

    void DamageSlime(int column, int row)
    {
        if (column > 0)
        {
            if (slimeTiles[column - 1, row])
            {
                slimeTiles[column - 1, row].TakeDamage(1);
                if (slimeTiles[column - 1, row].hitPoint <= 0)
                {
                    slimeTiles[column - 1, row] = null;
                }
                makeSlime = false;
            }
        }

        if (column < width - 1)
        {
            if (slimeTiles[column + 1, row])
            {
                slimeTiles[column + 1, row].TakeDamage(1);

                if (slimeTiles[column + 1, row].hitPoint <= 0)
                {
                    slimeTiles[column + 1, row] = null;
                }
                makeSlime = false;

            }
        }

        if (row > 0)
        {
            if (slimeTiles[column, row - 1])
            {
                slimeTiles[column, row - 1].TakeDamage(1);

                if (slimeTiles[column, row - 1].hitPoint <= 0)
                {
                    slimeTiles[column, row - 1] = null;
                }
                makeSlime = false;

            }
        }

        if (row < height - 1)
        {
            if (slimeTiles[column, row + 1])
            {
                slimeTiles[column, row + 1].TakeDamage(1);

                if (slimeTiles[column, row + 1].hitPoint <= 0)
                {
                    slimeTiles[column, row + 1] = null;
                }
                makeSlime = false;

            }
        }
    }

    private MatchType ColumnOrRow()
    {
        //make a copy of current matches
        List<GameObject> matchCopy = findMatches.currentMatches as List<GameObject>;

        matchType.type = 0;
        matchType.color = "";
        //Cycle through all of match copy and decide if a bomb needs to be made
        for(int i = 0; i < matchCopy.Count; i++)
        {
            //Store this dot
            Dot thisDot = matchCopy[i].GetComponent<Dot>();
            string color = matchCopy[i].tag;
            int column = thisDot.column;
            int row = thisDot.row;
            int columnMatch = 0;
            int rowMatch = 0;
            //Cycle through the rest of the pieces and compare
            for(int j = 0; j < matchCopy.Count; j++)
            {
                //Store the next dot
                Dot nextDot = matchCopy[j].GetComponent<Dot>();
                if (nextDot == thisDot)
                {
                    continue;
                }
                if(nextDot.column == thisDot.column && nextDot.tag == color)
                {
                    columnMatch++;
                }
                if (nextDot.row == thisDot.row && nextDot.tag == color)
                {
                    rowMatch++;
                }
            }

            //Return 3 if color or row
            //Return 2 if adjacent
            //Return 1 if color bomb
            if(columnMatch == 4 || rowMatch == 4)
            {
                matchType.type = 1;
                matchType.color = color;
                return matchType;
            }
           else if (columnMatch == 2 && rowMatch == 2)
            {
                matchType.type = 2;
                matchType.color = color;
                return matchType;
            }
           else if (columnMatch == 3 || rowMatch == 3)
            {
                matchType.type = 3;
                matchType.color = color;
                return matchType;
            }

        }

        matchType.type = 0;
        matchType.color = "";
        return matchType;        /*
        int numberHorizontal = 0;
        int numberVertical = 0;
        Dot firstPice = findMatches.currentMatches[0].GetComponent<Dot>();

        if (firstPice != null)
        {
            foreach (GameObject currentPice in findMatches.currentMatches)
            {
                Dot dot = currentPice.GetComponent<Dot>();
                if (dot.row == firstPice.row)
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
        */
    }

    private void CheckToMakeBomb()
    {
        //How many objects are in findMatches currentMatches?
        if(findMatches.currentMatches.Count > 3)
        {
            //What type of match?
            MatchType typeOfMatch = ColumnOrRow();
            if(typeOfMatch.type == 1)
            {
                //make a color bomb
                //is the current dot match?
                if (currentDot != null && currentDot.isMatched && currentDot.tag == typeOfMatch.color)
                {


                    currentDot.isMatched = false;
                    currentDot.MakeColorBomb();
                }

                else
                {
                    if (currentDot.otherDot != null)
                    {
                        Dot otherDot = currentDot.otherDot.GetComponent<Dot>();
                        if (otherDot.isMatched && otherDot.tag == typeOfMatch.color)
                        {

                            otherDot.isMatched = false;
                            otherDot.MakeColorBomb();

                        }
                    }
                }
                
            }else if(typeOfMatch.type == 2)
            {
                //make a adjacent bomb

                if (currentDot != null && currentDot.isMatched && currentDot.tag == typeOfMatch.color)
                {
                  
                       
                            currentDot.isMatched = false;
                            currentDot.MakeAdjacentBomb();
                        
                 }
                    else
                    {
                        if (currentDot.otherDot != null)
                        {
                            Dot otherDot = currentDot.otherDot.GetComponent<Dot>();
                            if (otherDot.isMatched && otherDot.tag == typeOfMatch.color)
                            {
                                
                                    otherDot.isMatched = false;
                                    otherDot.MakeAdjacentBomb();
                                
                            }
                        
                       }
                  }
            }else if(typeOfMatch.type == 3)
            {
                findMatches.CheckBombs(typeOfMatch);

            }
        }

        /*
        if (findMatches.currentMatches.Count ==4)
        {
            findMatches.CheckBombs();
        }
        if (findMatches.currentMatches.Count >= 5)
        //if (findMatches.currentMatches.Count == 5 || findMatches.currentMatches.Count == 8)
        {
            if (ColumnOrRow())
            {
                //make a color bomb
                //is the current dot match?
                if (currentDot != null)
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
                        if (currentDot.otherDot != null)
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
        */
    }

    public void BombRow(int row)
    {
        for(int i = 0; i < width; i++)
        {
            
                if (concreteTiles[i, row])
                {
                    concreteTiles[i, row].TakeDamage(1);
                    if (concreteTiles[i, row].hitPoint <= 0)
                    {
                        concreteTiles[i, row] = null;
                    }
                }
            
        }
        //Debug.Log("BombRow");

    }

    public void BombColumn(int column)
    {
        for (int i = 0; i < width; i++)
        {
            
                if (concreteTiles[column, i])
                {
                    concreteTiles[column, i].TakeDamage(1);
                    if (concreteTiles[column, i].hitPoint <= 0)
                    {
                        concreteTiles[column, i] = null;
                    }
                }
            
        }

    }

    private void DestroyMatchesAt(int column, int row)
    {
        if (allDots[column, row].GetComponent<Dot>().isMatched)
        {
          
            //Does a tile need to break?
            if (breakableTiles[column, row] != null)
            {
                //if it does, give one damage
                breakableTiles[column, row].TakeDamage(1);
                if (breakableTiles[column, row].hitPoint <= 0)
                {
                    breakableTiles[column, row] = null;
                }
            }
            if (lockTiles[column, row] != null)
            {
                //if it does, give one damage
                lockTiles[column, row].TakeDamage(1);
                if (lockTiles[column, row].hitPoint <= 0)
                {
                    lockTiles[column, row] = null;
                }
            }
            DamageConcrete(column, row);
            DamageSlime(column, row);
            if (goalMAnager != null)
            {
                goalMAnager.CompareGoal(allDots[column, row].tag.ToString());
                goalMAnager.UpdateGoals();
            }
            //Does the sound manager exit?
            int panel = PlayerPrefs.GetInt("panel");
            if (panel !=1 && soundManager != null)
            {
                    soundManager.PlayRandomDestroyNoise();
                
            }

            GameObject particle = Instantiate(destroyEffect, allDots[column, row].transform.position, Quaternion.identity);
            Destroy(particle, .5f);
            allDots[column, row].GetComponent<Dot>().PopAnim();
            Destroy(allDots[column, row],.5f);
            scoreManager.IncreaseScore(basePieceValue * streakValue);
            allDots[column, row] = null;
        }
    }

    public IEnumerator DestroyMatches()
    {
        //how many elements are in matched pieces list from find matched?
        if (findMatches.currentMatches.Count >= 4)
        {
            CheckToMakeBomb();
            //findMatches.currentMatches.Clear();

        }
        findMatches.currentMatches.Clear();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    
                    DestroyMatchesAt(i, j);
                }
            }
        }
        yield return new WaitForSeconds(.3f);
        StartCoroutine(DecreaseRowCo2());
       
    }

    private IEnumerator DecreaseRowCo2()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                //if the current spot isn't blank and is empty ...
                if (!blankSpaces[i, j] && allDots[i, j] == null && !concreteTiles[i,j] && !slimeTiles[i,j])
                {
                    //loop from the space above to the top of the column
                    for (int k = j + 1; k < height; k++)
                    {
                        //if a dot is found
                        if (allDots[i, k] != null)
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
        yield return new WaitForSeconds(refillDelay);


        //yield return new WaitForSeconds(refillDelay * 0.5f);
        Debug.Log("Refilling the board");

        StartCoroutine(FillBoardCo());
    }
    private IEnumerator DecreaseRowCo()
    {
        int nullCount = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null)
                {
                    nullCount++;
                }
                else if (nullCount > 0)
                {
                    ///
                    //yield return new WaitForSeconds(refillDelay * 1f);

                    allDots[i, j].GetComponent<Dot>().row -= nullCount;
                    allDots[i, j] = null;
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(refillDelay);

        //yield return new WaitForSeconds(refillDelay * 0.5f);
        StartCoroutine(FillBoardCo());
    }

    private void RefillBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null && !blankSpaces[i, j] && !concreteTiles[i, j] && !slimeTiles[i, j])
                    //if (allDots[i, j] == null && !blankSpaces[i, j])
                {
                    Vector2 tempPosition = new Vector2(i, j + offSet);
                    int dotToUse = Random.Range(0, gamePieces.Length);
                    int maxIterations = 0;
                    while (MatchesAt(i, j, gamePieces[dotToUse]) && maxIterations < 100)
                    {
                        maxIterations++;
                        dotToUse = Random.Range(0, gamePieces.Length);
                    }
                    maxIterations = 0;

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
        //Debug.Log("fulllllllllllllll");
        findMatches.FindAllMatches();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
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
        //yield return new WaitForSeconds(refillDelay);
        RefillBoard();
        yield return new WaitForSeconds(refillDelay);

        while (MatchesOnBoard())
        {
            streakValue++;
            //yield return new WaitForSeconds(1f);
            StartCoroutine(DestroyMatches());
            //DestroyMatches();

            yield break;
            //yield return new WaitForSeconds( refillDelay);
        }
        currentDot = null;
        CheckToMakeSlime();
        if (IsDeadLocked())
        {
            StartCoroutine(ShuffleBoars());
            Debug.Log("Dead Locked!!!");
        }
        yield return new WaitForSeconds(refillDelay);
        Debug.Log("Done Refilling");
        System.GC.Collect();
        if(currentState != GameState.pause)
           currentState = GameState.move;

        makeSlime = true;
        streakValue = 1;
    }

    private void CheckToMakeSlime()
    {
        //Check to the slime tiles array
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if(slimeTiles[i,j] != null && makeSlime)
                {
                    //Call another method to make slime
                    MakeNewSlime();
                    return;
                }
            }
        }
    }

    private Vector2 CheckForAdjacent(int column, int row)
    {
        if(allDots[column + 1, row] && column < width - 1)
        {
            return Vector2.right;
        }
        if (allDots[column - 1, row] && column > 0)
        {
            return Vector2.left;
        }
        if (allDots[column , row + 1] && row < height - 1)
        {
            return Vector2.up;
        }
        if (allDots[column , row - 1] && row > 0)
        {
            return Vector2.down;
        }
        return Vector2.zero;
    }

    private void MakeNewSlime()
    {
        bool slime = false;
        int loops = 0;
        while (!slime && loops < 200)
        {
            int newX = Random.Range(0, width);
            int newY = Random.Range(0, height);
            if (slimeTiles[newX, newY] != null)
            {
                Vector2 adjacent = CheckForAdjacent(newX, newY);
                Debug.Log(adjacent);

                if (adjacent != Vector2.zero)
                {
                    Destroy(allDots[newX + (int) adjacent.x, newY + (int) adjacent.y]);
                    Vector2 tempPosition = new Vector2(newX + (int)adjacent.x, newY + (int)adjacent.y);
                    GameObject tile = Instantiate(slimePiecePrefab, tempPosition, Quaternion.identity);
                    Debug.Log(tempPosition);

                    slimeTiles[newX + (int)adjacent.x, newY + (int)adjacent.y] = tile.GetComponent<BackgroundTile>();
                    slime = true;
                }
            }
            loops++;
            Debug.Log(loops);

        }
    }

    private void SwitchPieces(int column, int row, Vector2 direction)
    {
        if(allDots[column + (int)direction.x,row + (int)direction.y] != null)
        {
            //take the second pice and save it in a holder
            GameObject holder = allDots[column + (int)direction.x, row + (int)direction.y] as GameObject;
            //switching the first dot to be the second position
            allDots[column + (int)direction.x, row + (int)direction.y] = allDots[column, row];
            //first dot to be the second dot
            allDots[column, row] = holder;
        }
       
    }

    private bool CheckForMatches()
    {
        for (int i = 0; i < width; i++)
        {
            //////////////////////////////////////////////////////////////
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    //Make sure that one and two to the right on in the board
                    //Check if the dots to the right and two the right exist
                    if (i < width - 2)
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

    public bool SwitchAndCheck(int column, int row, Vector2 direction)
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
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    if (i < width - 1)
                    {
                        if (SwitchAndCheck(i, j, Vector2.right))
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

    private IEnumerator ShuffleBoars()
    {
        yield return new WaitForSeconds(0.5f);
        //create a list of game object
        List<GameObject> newboard = new List<GameObject>();
        //Add every piece to this list
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    newboard.Add(allDots[i, j]);
                }
            }
        }

        yield return new WaitForSeconds(0.5f);
        //for every spot on the board
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                //if this spot shouldn't be blank
                if (!blankSpaces[i, j] && !concreteTiles[i,j] && !slimeTiles[i,j])
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
            StartCoroutine(ShuffleBoars());
            //ShuffleBoars();
        }
    }
}
