using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HoboController : Character
{
    //Pathfinding variables
    private Vector2[] path;
    private int targetIndex;
    private Vector2 currentWaypoint;

    //State machine variables
    private ThresholdState hpState;
    private ThresholdState spState;
    private bool scavenging = false;

    protected override int Health
    {
        get
        {
            return base.Health;
        }

        set
        {
            base.Health = value;

            if (value >= 80 && value < 100)
            {
                hpState = ThresholdState.Satisfied;
                AnalyzeStates();
            }
            else if (value >= 40 && value < 20)
            {
                hpState = ThresholdState.Low;
                AnalyzeStates();
            }
            else
            {
                hpState = ThresholdState.Critical;
                AnalyzeStates();
            }
        }
    }

    protected override int Sanity
    {
        get
        {
            return base.Sanity;
        }

        set
        {
            base.Sanity = value;

            if (value >= 70 && value < 100)
            {
                spState = ThresholdState.Satisfied;
                AnalyzeStates();
            }
            else if (value >= 30 && value < 15)
            {
                spState = ThresholdState.Low;
                AnalyzeStates();
            }
            else
            {
                spState = ThresholdState.Critical;
                AnalyzeStates();
            }
        }
    }

    private void AnalyzeStates()
    {
        if (hpState == ThresholdState.Satisfied && spState == ThresholdState.Satisfied)
        {
            //Idle actions
            //Debug.Log("Hobo AI idle");
        }
        else if (hpState == ThresholdState.Low)
        {
            //Scavenge
            Scavenge();
            //Debug.Log("Hobo AI scavenging");
        }
        else
        {
            //Beg?!?
            //Debug.Log("Hobo AI critical action");
        }
    }

    private Vector2[] shortestPath;
    private TrashSpawn nearestSpawn = new TrashSpawn();
    private Dictionary<Vector2[], int> paths;
    private int requestsSent = 0;
    private bool movingToTarget = false;

    private void Scavenge()
    {
        scavenging = true;
        //Get nearest trashcan
        if (paths == null)
        {
            foreach (TrashSpawn item in GameManager.Instance.GetTrashcans)
            {
                PathRequestManager.RequestPath(transform.position, item.transform.position, OnPathStuff);
                requestsSent++;
            } 
        }

        //Move to it when all the paths have been found and compared
        if (paths.Count >= requestsSent)
        {
            shortestPath = paths.OrderBy(kvp => kvp.Value).First().Key; //Vittumikäsäätö
            StartMovement(shortestPath);
            movingToTarget = true;
        }

        //when reached, search it
        if (!movingToTarget)
        {
            //Reached the end of the path, remove target from search pool
            paths.Remove(shortestPath);
            //paths = paths.OrderBy(x => x.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
        }
        //Interact happens on moving over, but should it tho? ¯\_(ツ)_/¯ 
        //repeat
    }

    public void OnPathStuff(Vector2[] newPath, bool pathSuccess, int pathLength)
    {
        if (pathSuccess)
        {
            paths.Add(newPath, pathLength);
        }
    }

    /// <summary>
    /// Called when callback occurs from PathRequestManager.RequestPath().
    /// </summary>
    /// <param name="newPath">The new calulated path.</param>
    /// <param name="pathSuccess">Was the pathfind successful?</param>
    public void OnPathFound(Vector2[] newPath, bool pathSuccess, int pathLength)
    {
        if (pathSuccess)
        {
            StartMovement(newPath);
        }
    }

    private void StartMovement(Vector2[] newPath)
    {
        path = newPath;
        targetIndex = 0;
        StopCoroutine("FollowPath");
        StartCoroutine("FollowPath");
    }

    /// <summary>
    /// Co-routine to move along the found path.
    /// </summary>
    private IEnumerator FollowPath()
    {
        currentWaypoint = path[0];

        while (true)
        {
            if ((Vector2)transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    targetIndex = 0;
                    path = new Vector2[0];
                    movementDirection = Vector3.zero;
                    if (scavenging)
                    {
                        movingToTarget = false;
                        paths.Remove(shortestPath);
                    }
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            movementDirection = new Vector3(currentWaypoint.x - transform.position.x, currentWaypoint.y - transform.position.y, 0f).normalized;
            yield return null;
        }
    }

    protected override void ApplyMovement()
    {
        if (sprinting)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, movementSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, sprintSpeed * Time.deltaTime);
        }
    }


    public override void ConsumeItem(int itemID)
    {
        //Think this over later
    }

    protected override void Attack()
    {
    }

    protected override void Beg()
    {
    }

    protected override void Death()
    {
    }

    public override void Gather(List<BaseItem> items)
    {
        foreach (BaseItem item in items)
        {
            if (item.Consumable)
            {
                //Does nothing right now
                ConsumeItem(item.BaseItemID);
                items.Remove(item);
            }
        }

        characterInventory.AddItemToInventory(items);
    }

    protected override void GetInput()
    {
        //Test
        if (Input.GetMouseButtonDown(0))
        {
            PathRequestManager.RequestPath(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), OnPathFound);
        }
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        /*GetInput();
        RecoverStamina();
        ExhaustTimer();
        Collision();
        AnimationChanger();
        ApplyMovement();*/
    }

}
public enum ThresholdState
{
    Satisfied,
    Low,
    Critical
}
