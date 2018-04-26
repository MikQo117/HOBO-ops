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
            StartScavenge();
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
    private Dictionary<Vector2[], int> paths = new Dictionary<Vector2[], int>();
    private int requestsSent = 0;
    private bool movingToTarget = false;
    private List<TrashSpawn> spawnsToSearch = new List<TrashSpawn>();

    private void StartScavenge()
    {
        scavenging = true;
        if (spawnsToSearch.Count <= 0)
        {
            foreach (IInteractable item in GameManager.Instance.interactables)
            {
                if (item is TrashSpawn)
                {
                    TrashSpawn temp = item as TrashSpawn;
                    spawnsToSearch.Add(temp);
                }
            } 
        }
        foreach (TrashSpawn item in spawnsToSearch)
        {
            PathRequestManager.RequestPath(transform.position, item.transform.position, OnPathStuff);
            requestsSent++;
        }
    }

    protected override void CheckForInteraction()
    {
        //For through all interactable colliders, and see if intersects
        foreach (Collider2D item in GameManager.Instance.interactablesColliders)
        {
            //Debug.Log("Closest point from player: " + item.bounds.ClosestPoint(transform.position));
            //If contains, get component from collider, typeof IInteractable
            if (collider.bounds.Intersects(item.bounds))
            {
                Debug.Log("Hit interactable");
                //Call Interact and pass this as parameter
                IInteractable temp = item.GetComponent<IInteractable>();
                temp.Interact(this);
                if (temp is TrashSpawn)
                {
                    spawnsToSearch.Remove(temp as TrashSpawn);
                    Debug.Log(spawnsToSearch.Count);
                    foreach (TrashSpawn item2 in spawnsToSearch)
                    {
                        Debug.Log(item2); 
                    }
                }
            }
        }
    }

    public void OnPathStuff(Vector2[] newPath, bool pathSuccess, int pathLength)
    {
        if (pathSuccess)
        {
            paths.Add(newPath, pathLength);
            if (paths.Count >= requestsSent)
            {
                shortestPath = paths.OrderBy(kvp => kvp.Value).First().Key; //Vittumikäsäätö
                //shortestPath = paths.Aggregate((l, r) => l.Value < r.Value ? l : r).Key; //Vittumikäsäätö
                //shortestPath = paths.Where(x => x.Value == paths.Select(k => paths[k.Key]).Min());
                StartMovement(shortestPath);
                movingToTarget = true;
            }
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
                        StartScavenge();
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
        if (movingToTarget)
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
        /*foreach (BaseItem item in items)
        {
            if (item.Consumable)
            {
                //Does nothing right now
                ConsumeItem(item.BaseItemID);
                items.Remove(item);
            }
        }*/

        //characterInventory.AddItemToInventory(items);
    }

    protected override void GetInput()
    {
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        StartScavenge();
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
