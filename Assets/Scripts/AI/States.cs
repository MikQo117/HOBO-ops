using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using StateStuff;

namespace StateStuff
{
    public class ScavengeState : State<HoboController>
    {

        private Vector2[]                  shortestPath;
        private TrashSpawn                 nearestSpawn = new TrashSpawn();
        private Dictionary<Vector2[], int> paths = new Dictionary<Vector2[], int>();
        private List<SpawnData>            spawns = new List<SpawnData>();
        private int                        requestsSent = 0;

        //Sub-state variables
        private int                        subState = 0;
        

        public void PathEndReached()
        {
            // The end of the path has been reached, advance to next sub-state
            subState = 2;
        }

        private void ResetVariables()
        {
            requestsSent = 0;
            subState = 0;
            paths.Clear();
            spawns.Clear();
            shortestPath = new Vector2[0];
            nearestSpawn = null;
        }

        private void ResetSubStateVariables()
        {
            requestsSent = 0;
            subState = 0;
            paths.Clear();
            shortestPath = new Vector2[0];
            nearestSpawn = null;
        }

        private void ResetPaths()
        {
            foreach (SpawnData item in spawns)
            {
                item.Path = null;
            }
        }

        public override void EnterState(HoboController owner)
        {
            ResetVariables();
            Debug.Log("Entering scavenge state");
            if (spawns.Count <= 0)
            {
                GetTrashCans();
                SendPathRequests(owner.transform.position);
            }
        }


        public override void UpdateState(HoboController owner)
        {
            switch (subState)
            {
                default:
                    break;
                case 0:
                    //Paths not received
                    Debug.Log("Waiting for paths");
                    break;
                case 1:
                    //Paths received
                    //Move
                    if (!owner.MovingToTarget)
                    {
                        Debug.Log("Movement started");
                        owner.Grid.Path = shortestPath;
                        owner.StartMovement(shortestPath);
                    }
                    else
                    {
                        Debug.Log("Waiting for movement to end");
                    }
                    break;
                case 2:
                    //Target reached, stop movement and gather
                    Debug.Log("Target reached");
                    spawns.Remove(spawns.Find(x => x.ActiveTarget == true));
                    ResetPaths();
                    if (spawns.Count <= 0)
                    {
                        owner.StateMachine.ChangeState(IdleState.Instance);
                    }
                    else
                    {
                        ResetSubStateVariables();
                        SendPathRequests(owner.transform.position);
                        subState = 0;
                    }
                    break;
            }
        }


        public override void ExitState(HoboController owner)
        {
            Debug.Log("Exiting scavenge state");
            //Reset variables
            ResetVariables();
        }


        private void GetTrashCans()
        {
            if (GameManager.Instance.interactables.Count > 0)
            {
                foreach (IInteractable item in GameManager.Instance.interactables)
                {
                    if (item is TrashSpawn)
                    {
                        spawns.Add(new SpawnData((TrashSpawn)item, null, 0));
                    }
                } 
            }
            else
            {
                Debug.Log("Interactables not populated");
            }
        }
        private void SendPathRequests(Vector3 startPosition)
        {
            foreach (SpawnData item in spawns)
            {
                PathRequestManager.RequestPath(startPosition, item.Target.transform.position, OnPathStuff);
                requestsSent++;
            }
        }

        private int tempIndex = 0;
        public void OnPathStuff(Vector2[] newPath, bool pathSuccess, int pathLength)
        {
            if (pathSuccess)
            {
                //paths.Add(newPath, pathLength);
                spawns[tempIndex].Path = newPath;
                spawns[tempIndex].PathLength = pathLength;
                tempIndex++;
                if (PathsPopulated())
                {
                    shortestPath = spawns.OrderBy(spawn => spawn.PathLength).First().SetAsCurrent(); //Vittumikäsäätö
                    //shortestPath = spawns.Aggregate((l, r) => l.PathLength < r.PathLength ? l : r).SetAsCurrent(); //Vittumikäsäätö
                    //shortestPath = spawns.Where(x => x.PathLength == paths.Select(k => paths[k.Key]).Min());
                    subState = 1;
                    tempIndex = 0;
                }
            }
        }

        private bool PathsPopulated()
        {
            int count = 0;

            foreach (SpawnData item in spawns)
            {
                if (item.Path != null)
                {
                    count++;
                }
            }

            if (count >= requestsSent)
            {
                return true;
            }
            return false;

        }
    }

    public class SpawnData
    {
        public TrashSpawn Target;
        public Vector2[]  Path;
        public int        PathLength;
        private bool      activeTarget;

        public SpawnData(TrashSpawn target, Vector2[] path, int pathLength)
        {
            Target = target;
            Path = path;
            PathLength = pathLength;
            activeTarget = false;
        }

        public bool ActiveTarget
        {
            get { return activeTarget; }
            set { activeTarget = value; }
        }

        public Vector2[] SetAsCurrent()
        {
            activeTarget = true;
            return Path;
        }
    }

















    public class IdleState : State<HoboController>
    {
        private static IdleState instance;

        private IdleState()
        {
            if (instance != null)
            {
                return;
            }
            instance = this;
        }

        public static IdleState Instance
        {
            get
            {
                if (instance == null)
                {
                    new IdleState();
                }
                return instance;
            }
        }

        public override void EnterState(HoboController owner)
        {
            Debug.Log("Entering idle state");
            //owner.StateMachine.ChangeState(ScavengeState.Instance);
        }

        public override void ExitState(HoboController owner)
        {
            Debug.Log("Exiting idle state");
        }

        public override void UpdateState(HoboController owner)
        {
            
        }
    }
}
/*//Pathfinding variables
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
           if (spawnsToSearch.Count <= 0)
           {
               requestsSent = 0; 
           }
           spawnsToSearch.Remove(temp as TrashSpawn);
           Debug.Log(spawnsToSearch.Count);
           paths.Clear();
       }
   }
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
}

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
GetInput();
RecoverStamina();
ExhaustTimer();
Collision();
AnimationChanger();
ApplyMovement();
}
*/
