using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class ShapesManager : MonoBehaviour
{
    public static ShapesManager instance = null;
    public Button menu;
    public Text DebugText, ScoreText;
    public bool ShowDebugInfo = false;
   
    public ShapesArray shapes;
    public Image BackgroundImage;
    public List<Sprite> Images;
    public Text level;
    private int score;
    private string task;

    public int need;
    private int have;
    public Text TaskText;

    public Vector2 bannedArea1 = Constants.AreaForBan1;
    public Vector2 bannedArea2 = Constants.AreaForBan2;

    public Vector2 pointsArea1 = Constants.AreaForPoints1;
    public Vector2 pointsArea2 = Constants.AreaForPoints2;

    public Text howManyLeft;
    public int movesLeft = Constants.RequiredMoves;
    public float timeLeft = Constants.RequiredTime;

    public List<Sprite> starsVaraints;
    public Image oneStar;
    public Image twoStars;
    public Image threeStars;

    public readonly Vector2 BottomRight = new Vector2(-2.3f, -2.5f);
    public readonly Vector2 CandySize = new Vector2(0.65f, 0.65f);

    private GameState state = GameState.None;
    private GameObject hitGo = null;
    private Vector2[] SpawnPositions;
    public GameObject[] CandyPrefabs;
    public GameObject[] ExplosionPrefabs;
    public GameObject[] BonusPrefabs;

    private IEnumerator CheckPotentialMatchesCoroutine;
    private IEnumerator AnimatePotentialMatchesCoroutine;

    IEnumerable<GameObject> potentialMatches;

    public SoundManager soundManager;
    void Awake()
    {
        DebugText.enabled = ShowDebugInfo;
    }

    void GoToMenu()
    {
        SceneManager.LoadScene("mainMenu");
    }

    // Use this for initialization
    void Start()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        //DontDestroyOnLoad(gameObject);
        

        menu.onClick.AddListener(GoToMenu);
        level.text = Constants.CurrentLevel.ToString();
        if (Constants.CurrentLevel / 5 < 5)
        {
            BackgroundImage.sprite = Images.ElementAt((Constants.CurrentLevel-1) / 5);
        }
        else
        {
            BackgroundImage.sprite = Images.ElementAt(4);
        }
        Constants.Mode = Constants.CurrentLevel % 6;
        if (Constants.Mode == 0)
        {
            Constants.Mode = 1;
        }
        switch (Constants.Mode)
        {
            case 1:
                task = string.Format("Собрать {0} за {1}. {2}/{3}", need, Left(), have, need);
                break;
            case 2:
                task = string.Format("Собрать {0} за {1} ходов. {2}/{3}", need, Left(), have, need);
                break;
            case 3:
                task = string.Format("Собрать {0} очков комбинациями с символом локации за {1}. {2}/{3}", need, Left(), have,
                    need);
                break;
            case 4:
                task = string.Format("Собрать {0} на некой территории за {1}. {2}/{3}", need, Left(), have, need);
                break;
            case 5:
                task = string.Format("Собрать {0} только на закрытой части за {1}. {2}/{3}", need, Left(), have, need);
                break;
            default:
                break;
        }
        TaskText.text = task;
        InitializeTypesOnPrefabShapesAndBonuses();

        InitializeCandyAndSpawnPositions();

        StartCheckForPotentialMatches();
        if (Constants.Mode == 5)
        {
            setBanned(bannedArea1, bannedArea2);
        }
    }

    public string Left()
    {
        if (Constants.Mode == 1 || Constants.Mode == 3 || Constants.Mode == 5)
        { return String.Format("{0:F2}", timeLeft);}
        else
        { return movesLeft + " ходов";}
    }
    /// <summary>
    /// Initialize shapes
    /// </summary>
    private void InitializeTypesOnPrefabShapesAndBonuses()
    {
        //just assign the name of the prefab
        foreach (var item in CandyPrefabs)
        {
            item.GetComponent<Shape>().Type = item.name;

        }

        //assign the name of the respective "normal" candy as the type of the Bonus
        foreach (var item in BonusPrefabs)
        {
            item.GetComponent<Shape>().Type = CandyPrefabs.
                Where(x => x.GetComponent<Shape>().Type.Contains(item.name.Split('_')[1].Trim())).Single().name;
        }
    }

    public void InitializeCandyAndSpawnPositionsFromPremadeLevel()
    {
        InitializeVariables();

        var premadeLevel = DebugUtilities.FillShapesArrayFromResourcesData();

        if (shapes != null)
            DestroyAllCandy();

        shapes = new ShapesArray();
        SpawnPositions = new Vector2[Constants.Columns];

        for (int row = 0; row < Constants.Rows; row++)
        {
            for (int column = 0; column < Constants.Columns; column++)
            {

                GameObject newCandy = null;

                newCandy = GetSpecificCandyOrBonusForPremadeLevel(premadeLevel[row, column]);

                InstantiateAndPlaceNewCandy(row, column, newCandy);

            }
        }

        SetupSpawnPositions();
    }


    public void InitializeCandyAndSpawnPositions()
    {
        InitializeVariables();

        if (shapes != null)
            DestroyAllCandy();

        shapes = new ShapesArray();
        SpawnPositions = new Vector2[Constants.Columns];

        for (int row = 0; row < Constants.Rows; row++)
        {
            for (int column = 0; column < Constants.Columns; column++)
            {

                GameObject newCandy = GetRandomCandy();
             

                //check if two previous horizontal are of the same type
                while (column >= 2 && shapes[row, column - 1].GetComponent<Shape>()
                    .IsSameType(newCandy.GetComponent<Shape>())
                    && shapes[row, column - 2].GetComponent<Shape>().IsSameType(newCandy.GetComponent<Shape>()))
                {
                    newCandy = GetRandomCandy();
                }

                //check if two previous vertical are of the same type
                while (row >= 2 && shapes[row - 1, column].GetComponent<Shape>()
                    .IsSameType(newCandy.GetComponent<Shape>())
                    && shapes[row - 2, column].GetComponent<Shape>().IsSameType(newCandy.GetComponent<Shape>()))
                {
                    newCandy = GetRandomCandy();
                }
                newCandy.layer = 5;
                InstantiateAndPlaceNewCandy(row, column, newCandy);

            }
        }

        SetupSpawnPositions();
    }



    private void InstantiateAndPlaceNewCandy(int row, int column, GameObject newCandy)
    {
        GameObject go = Instantiate(newCandy,
            BottomRight + new Vector2(column * CandySize.x, row * CandySize.y), Quaternion.identity)
            as GameObject;

        //assign the specific properties
        go.GetComponent<Shape>().Assign(newCandy.GetComponent<Shape>().Type, row, column);
        shapes[row, column] = go;
    }

    private void SetupSpawnPositions()
    {
        //create the spawn positions for the new shapes (will pop from the 'ceiling')
        for (int column = 0; column < Constants.Columns; column++)
        {
            SpawnPositions[column] = BottomRight
                + new Vector2(column * CandySize.x, Constants.Rows * CandySize.y);
        }
    }




    /// <summary>
    /// Destroy all candy gameobjects
    /// </summary>
    private void DestroyAllCandy()
    {
        for (int row = 0; row < Constants.Rows; row++)
        {
            for (int column = 0; column < Constants.Columns; column++)
            {
                Destroy(shapes[row, column]);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        switch (Constants.Mode)
        {
            case 1:
                task = string.Format("Собрать {0}. Осталось {1}\n{2}/{3}", need, Left(), have, need);
                break;
            case 2:
                task = string.Format("Собрать {0}. Осталось {1}\n{2}/{3}", need, Left(), have, need);
                break;
            case 3:
                task = string.Format("Собрать {0} комбинаций с символом локации. Осталось {1}\n{2}/{3}", need, Left(), have,
                    need);
                break;
            case 4:
                task = string.Format("Собрать {0} на некой территории. Осталось {1}\n{2}/{3}", need, Left(), have, need);
                break;
            case 5:
                task = string.Format("Собрать {0}. Осталось {1}\n{2}/{3}", need, Left(), have, need);
                break;
            default:
                break;
        }
        TaskText.text = task;
        timeLeft -=  Time.deltaTime;
            have = score;
        if (have >= need / 3)
        {
            oneStar.sprite = starsVaraints[1];
        }
        if (have >= need / 2)
        {
            twoStars.sprite = starsVaraints[1];
        }
        if (have >= need)
        {
            threeStars.sprite = starsVaraints[1];
        }
        if (ShowDebugInfo)
            DebugText.text = DebugUtilities.GetArrayContents(shapes);
        if (Constants.Mode == 5)
        {
            setBanned(bannedArea1, bannedArea2);
        }
        if (state == GameState.None)
        {
            //user has clicked or touched
            if (Input.GetMouseButtonDown(0))
            {
                //get the hit position
                var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null) //we have a hit!!!
                {
                    hitGo = hit.collider.gameObject;
                    state = GameState.SelectionStarted;
                }
                
            }
        }
        else if (state == GameState.SelectionStarted)
        {
            //user dragged
            if (Input.GetMouseButton(0))
            {
                

                var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                //we have a hit
                if (hit.collider != null && hitGo != hit.collider.gameObject)
                {

                    //user did a hit, no need to show him hints 
                    StopCheckForPotentialMatches();

                    //if the two shapes are diagonally aligned (different row and column), just return
                    if (!Utilities.AreVerticalOrHorizontalNeighbors(hitGo.GetComponent<Shape>(),
                        hit.collider.gameObject.GetComponent<Shape>()))
                    {
                        state = GameState.None;
                    }
                    else if (hitGo.GetComponent<Shape>().isBanned)
                    {
                        state = GameState.None;
                    } else 
                    {
                        state = GameState.Animating;
                        FixSortingLayer(hitGo, hit.collider.gameObject);
                        StartCoroutine(FindMatchesAndCollapse(hit));
                    }
                }
            }
        }
        //TODO
        howManyLeft.text = Left();

        if (Constants.Mode == 1 || Constants.Mode == 3 || Constants.Mode == 5)
        {
            if (timeLeft <= 0)
            {
                if (have >= need)
                {
                    PlayerPrefs.SetInt("Level " + Constants.CurrentLevel, 3);
                }
                else if (have > need / 2)
                {
                    PlayerPrefs.SetInt("Level " + Constants.CurrentLevel, 2);
                }
                else if (have > need / 3)
                {
                    PlayerPrefs.SetInt("Level " + Constants.CurrentLevel, 1);
                }


                    PlayerPrefs.SetInt("LastLevel", Constants.CurrentLevel);
                PlayerPrefs.SetInt("Score" + Constants.CurrentLevel, score);
                wait();
                SceneManager.LoadScene("endGame");
            }
        }
        else if (Constants.Mode == 2 ||Constants.Mode == 4)
        {
            if (movesLeft <= 0)
            {
                if (have <= need)
                {
                    PlayerPrefs.SetInt("Level " + Constants.CurrentLevel, 3);
                }
                else if (have > need / 2)
                {
                    PlayerPrefs.SetInt("Level " + Constants.CurrentLevel, 2);
                }
                else if (have > need / 3)
                {
                    PlayerPrefs.SetInt("Level " + Constants.CurrentLevel, 1);
                }
                PlayerPrefs.SetInt("Score" + Constants.CurrentLevel, score);
                wait();
                SceneManager.LoadScene("endGame");
            }
        }
    }

    private IEnumerator wait()
    {
        yield return new WaitForSeconds(2);
    }
    /// <summary>
    /// Modifies sorting layers for better appearance when dragging/animating
    /// </summary>
    /// <param name="hitGo"></param>
    /// <param name="hitGo2"></param>
    private void FixSortingLayer(GameObject hitGo, GameObject hitGo2)
    {
        SpriteRenderer sp1 = hitGo.GetComponent<SpriteRenderer>();
        SpriteRenderer sp2 = hitGo2.GetComponent<SpriteRenderer>();
        if (sp1.sortingOrder <= sp2.sortingOrder)
        {
            sp1.sortingOrder = 1;
            sp2.sortingOrder = 0;
        }
    }

    void setBanned(Vector2 a, Vector2 b)
    {
        for (int i = (int)a.x; i <= (int)b.x; i++)
        {
            for (int j = (int)a.y; j <= (int)b.y; j++)
            {
                shapes[i, j].GetComponent<Shape>().isBanned = true;
            }
        }
    }

    bool isInArea(Vector2 a, Vector2 b, Vector2 c)
    {
        if (c.x >= a.x && c.x <= b.x)
        {
            if (c.y >= a.y && c.y <= b.y)
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator FindMatchesAndCollapse(RaycastHit2D hit2)
    {
        //get the second item that was part of the swipe
        var hitGo2 = hit2.collider.gameObject;
        if (hitGo2.GetComponent<Shape>().isBanned)
        {
            state = GameState.None;
            yield break;
        }
        shapes.Swap(hitGo, hitGo2);
        movesLeft--;
        if (movesLeft<=0) { Update();}
        howManyLeft.text = Left();
        int column1, row1;
        column1 = hitGo.GetComponent<Shape>().Column;
        row1 = hitGo.GetComponent<Shape>().Row;
        int column2, row2;
        column2 = hitGo2.GetComponent<Shape>().Column;
        row2 = hitGo2.GetComponent<Shape>().Row;
        //move the swapped ones
        hitGo.transform.positionTo(Constants.AnimationDuration, hitGo2.transform.position);
        string hitGoName = hitGo.name;
        hitGo2.transform.positionTo(Constants.AnimationDuration, hitGo.transform.position);
        yield return new WaitForSeconds(Constants.AnimationDuration);

        //get the matches via the helper methods
        var hitGomatchesInfo = shapes.GetMatches(hitGo);
        var hitGo2matchesInfo = shapes.GetMatches(hitGo2);

        var totalMatches = hitGomatchesInfo.MatchedCandy
            .Union(hitGo2matchesInfo.MatchedCandy).Distinct();
        
        //if user's swap didn't create at least a 3-match, undo their swap
        /*if (totalMatches.Count() < Constants.MinimumMatches)
        {
            hitGo.transform.positionTo(Constants.AnimationDuration, hitGo2.transform.position);
            hitGo2.transform.positionTo(Constants.AnimationDuration, hitGo.transform.position);
            yield return new WaitForSeconds(Constants.AnimationDuration);

            shapes.UndoSwap();
        }*/

        //if more than 3 matches and no Bonus is contained in the line, we will award a new Bonus
        bool addBonus = totalMatches.Count() >= Constants.MinimumMatchesForBonus &&
            !BonusTypeUtilities.ContainsDestroyWholeRowColumn(hitGomatchesInfo.BonusesContained) &&
            !BonusTypeUtilities.ContainsDestroyWholeRowColumn(hitGo2matchesInfo.BonusesContained);

        Shape hitGoCache = null;
        if (addBonus)
        {
            //get the game object that was of the same type
            var sameTypeGo = hitGomatchesInfo.MatchedCandy.Count() > 0 ? hitGo : hitGo2;
            hitGoCache = sameTypeGo.GetComponent<Shape>();
        }

        int timesRun = 1;
        while (totalMatches.Count() >= Constants.MinimumMatches)
        {
            //increase score
            //TODO add conditions to score increasement 
            var a = totalMatches.ElementAt(0).ToString();
            switch (Constants.Mode)
            {
                case 1:
                {
                    Increase(totalMatches, timesRun);
                    break;
                }
                case 2:
                {
                    Increase(totalMatches, timesRun);
                    break;
                }
                case 3:
                {
                    string color;
                    switch (Constants.CurrentLevel)
                    {
                            case 3:
                                color = "purple";
                                break;
                            case 8:
                                color = "blue";
                                break;
                            case 13:
                                color = "green";
                                break;
                            case 18:
                                color = "pink";
                                break;
                            case 23:
                                color = "orange";
                                break;
                            default:
                                color = "err";
                                break;
                    }
                    if (hitGoName.Contains(color))
                    {
                        Increase(totalMatches, timesRun);
                    }
                    break;
                }
                case 4:
                {
                    if (isInArea(pointsArea1, pointsArea2, new Vector2(column1, row1)) ||
                        isInArea(pointsArea1, pointsArea2, new Vector2(column2, row2)))
                    {
                        Increase(totalMatches, timesRun);
                    }
                    break;
                }
                case 5:
                {
                    foreach (var var in totalMatches)
                    {
                        if(isInArea(bannedArea1, bannedArea2, new Vector2(var.GetComponent<Shape>().Column, var.GetComponent<Shape>().Row)))
                        {
                            Increase(totalMatches, timesRun);
                            break;
                        }
                    }
                    /*
                    if (isInArea(0, 0, 4, 4, column1, row1) ||
                        isInArea(0, 0, 4, 4, column2, row2))
                    {
                        Increase(totalMatches, timesRun);
                    }*/
                    break;
                }
                default:
                {
                    Increase(totalMatches, timesRun);
                    break;
                }
            }
            soundManager.PlayCrincle();
            
            foreach (var item in totalMatches)
            {
                shapes.Remove(item);
                RemoveFromScene(item);
            }

            //check and instantiate Bonus if needed
            if (addBonus)
                CreateBonus(hitGoCache);

            addBonus = false;

            //get the columns that we had a collapse
            var columns = totalMatches.Select(go => go.GetComponent<Shape>().Column).Distinct();

            //the order the 2 methods below get called is important!!!
            //collapse the ones gone
            var collapsedCandyInfo = shapes.Collapse(columns);
            //create new ones
            var newCandyInfo = CreateNewCandyInSpecificColumns(columns);

            int maxDistance = Mathf.Max(collapsedCandyInfo.MaxDistance, newCandyInfo.MaxDistance);

            MoveAndAnimate(newCandyInfo.AlteredCandy, maxDistance);
            MoveAndAnimate(collapsedCandyInfo.AlteredCandy, maxDistance);



            //will wait for both of the above animations
            yield return new WaitForSeconds(Constants.MoveAnimationMinDuration * maxDistance);

            //search if there are matches with the new/collapsed items
            totalMatches = shapes.GetMatches(collapsedCandyInfo.AlteredCandy).
                Union(shapes.GetMatches(newCandyInfo.AlteredCandy)).Distinct();



            timesRun++;
        }

        state = GameState.None;
        
        StartCheckForPotentialMatches();
    }

    void Increase(IEnumerable<GameObject> totalMatches, int timesRun)
    {
        IncreaseScore((totalMatches.Count() - 2) * Constants.Match3Score);

        if (timesRun >= 2)
            IncreaseScore(Constants.SubsequentMatchScore);
    }
    /// <summary>
    /// Creates a new Bonus based on the shape parameter
    /// </summary>
    /// <param name="hitGoCache"></param>
    private void CreateBonus(Shape hitGoCache)
    {
        GameObject Bonus = Instantiate(GetBonusFromType(hitGoCache.Type), BottomRight
            + new Vector2(hitGoCache.Column * CandySize.x,
                hitGoCache.Row * CandySize.y), Quaternion.identity)
            as GameObject;
        shapes[hitGoCache.Row, hitGoCache.Column] = Bonus;
        var BonusShape = Bonus.GetComponent<Shape>();
        //will have the same type as the "normal" candy
        BonusShape.Assign(hitGoCache.Type, hitGoCache.Row, hitGoCache.Column);
        //add the proper Bonus type
        BonusShape.Bonus |= BonusType.DestroyWholeRowColumn;
    }




    /// <summary>
    /// Spawns new candy in columns that have missing ones
    /// </summary>
    /// <param name="columnsWithMissingCandy"></param>
    /// <returns>Info about new candies created</returns>
    private AlteredCandyInfo CreateNewCandyInSpecificColumns(IEnumerable<int> columnsWithMissingCandy)
    {
        AlteredCandyInfo newCandyInfo = new AlteredCandyInfo();

        //find how many null values the column has
        foreach (int column in columnsWithMissingCandy)
        {
            var emptyItems = shapes.GetEmptyItemsOnColumn(column);
            foreach (var item in emptyItems)
            {
                var go = GetRandomCandy();
                GameObject newCandy = Instantiate(go, SpawnPositions[column], Quaternion.identity)
                    as GameObject;

                newCandy.GetComponent<Shape>().Assign(go.GetComponent<Shape>().Type, item.Row, item.Column);

                if (Constants.Rows - item.Row > newCandyInfo.MaxDistance)
                    newCandyInfo.MaxDistance = Constants.Rows - item.Row;

                shapes[item.Row, item.Column] = newCandy;
                if (Constants.Mode == 5)
                {
                    if (isInArea(pointsArea1, pointsArea2, new Vector2(item.Row, item.Column)))
                    {
                        newCandy.GetComponent<Shape>().isBanned = true;
                    }
                } 
                newCandyInfo.AddCandy(newCandy);
            }
        }
        return newCandyInfo;
    }

    /// <summary>
    /// Animates gameobjects to their new position
    /// </summary>
    /// <param name="movedGameObjects"></param>
    private void MoveAndAnimate(IEnumerable<GameObject> movedGameObjects, int distance)
    {
        foreach (var item in movedGameObjects)
        {
            item.transform.positionTo(Constants.MoveAnimationMinDuration * distance, BottomRight +
                new Vector2(item.GetComponent<Shape>().Column * CandySize.x, item.GetComponent<Shape>().Row * CandySize.y));
        }
    }

    /// <summary>
    /// Destroys the item from the scene and instantiates a new explosion gameobject
    /// </summary>
    /// <param name="item"></param>
    private void RemoveFromScene(GameObject item)
    {
        GameObject explosion = GetRandomExplosion();
        var newExplosion = Instantiate(explosion, item.transform.position, Quaternion.identity) as GameObject;
        Destroy(newExplosion, Constants.ExplosionDuration);
        Destroy(item);
    }

    /// <summary>
    /// Get a random candy
    /// </summary>
    /// <returns></returns>
    private GameObject GetRandomCandy()
    {
        return CandyPrefabs[Random.Range(0, CandyPrefabs.Length)];
    }

    private void InitializeVariables()
    {
        score = 0;
        ShowScore();
    }

    private void IncreaseScore(int amount)
    {
        score += amount;
        ShowScore();
    }

    private void ShowScore()
    {
        ScoreText.text = "Счёт: " + score.ToString();
    }

    /// <summary>
    /// Get a random explosion
    /// </summary>
    /// <returns></returns>
    private GameObject GetRandomExplosion()
    {
        return ExplosionPrefabs[Random.Range(0, ExplosionPrefabs.Length)];
    }

    /// <summary>
    /// Gets the specified Bonus for the specific type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private GameObject GetBonusFromType(string type)
    {
        string color = type.Split('_')[1].Trim();
        foreach (var item in BonusPrefabs)
        {
            if (item.GetComponent<Shape>().Type.Contains(color))
                return item;
        }
        throw new System.Exception("Wrong type");
    }

    /// <summary>
    /// Starts the coroutines, keeping a reference to stop later
    /// </summary>
    private void StartCheckForPotentialMatches()
    {
        StopCheckForPotentialMatches();
        //get a reference to stop it later
        CheckPotentialMatchesCoroutine = CheckPotentialMatches();
        StartCoroutine(CheckPotentialMatchesCoroutine);
    }

    /// <summary>
    /// Stops the coroutines
    /// </summary>
    private void StopCheckForPotentialMatches()
    {
        if (AnimatePotentialMatchesCoroutine != null)
            StopCoroutine(AnimatePotentialMatchesCoroutine);
        if (CheckPotentialMatchesCoroutine != null)
            StopCoroutine(CheckPotentialMatchesCoroutine);
        ResetOpacityOnPotentialMatches();
    }

    /// <summary>
    /// Resets the opacity on potential matches (probably user dragged something?)
    /// </summary>
    private void ResetOpacityOnPotentialMatches()
    {
        if (potentialMatches != null)
            foreach (var item in potentialMatches)
            {
                if (item == null) break;

                Color c = item.GetComponent<SpriteRenderer>().color;
                c.a = 1.0f;
                item.GetComponent<SpriteRenderer>().color = c;
            }
    }

    /// <summary>
    /// Finds potential matches
    /// </summary>
    /// <returns></returns>
    private IEnumerator CheckPotentialMatches()
    {
        yield return new WaitForSeconds(Constants.WaitBeforePotentialMatchesCheck);
        potentialMatches = Utilities.GetPotentialMatches(shapes);
        if (potentialMatches != null)
        {
            while (true)
            {

                AnimatePotentialMatchesCoroutine = Utilities.AnimatePotentialMatches(potentialMatches);
                StartCoroutine(AnimatePotentialMatchesCoroutine);
                yield return new WaitForSeconds(Constants.WaitBeforePotentialMatchesCheck);
            }
        }
    }

    /// <summary>
    /// Gets a specific candy or Bonus based on the premade level information.
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    private GameObject GetSpecificCandyOrBonusForPremadeLevel(string info)
    {
        var tokens = info.Split('_');

        if (tokens.Count() == 1)
        {
            foreach (var item in CandyPrefabs)
            {
                if (item.GetComponent<Shape>().Type.Contains(tokens[0].Trim()))
                    return item;
            }

        }
        else if (tokens.Count() == 2 && tokens[1].Trim() == "B")
        {
            foreach (var item in BonusPrefabs)
            {
                if (item.name.Contains(tokens[0].Trim()))
                    return item;
            }
        }

        throw new System.Exception("Wrong type, check your premade level");
    }



}
