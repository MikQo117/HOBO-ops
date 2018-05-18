using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;

public class PedestrianController : Character
{
    //Pathfinding variables
    private Vector2[]                         path;
    private int                               targetIndex;
    private Vector2                           currentWaypoint;
    private bool                              movingToTarget = false;

    //States                                  Do these need getters?
    public StateMachine<PedestrianController> StateMachine { get; set; }
    public MovementState                      movementState;
    public IdleState                          idleState;

    //Treshold stuff
    public bool                               tryInteract = false;

    private void AnalyzeStatus()
    {
        
    }

    public bool MovingToTarget
    {
        get { return movingToTarget; }
        set { movingToTarget = value; }
    }

    protected override void Start()
    {
        base.Start();
        StateMachine = new StateMachine<PedestrianController>(this);
        movementState = new MovementState();
        idleState = new IdleState();


        StateMachine.ChangeState(movementState);
    }

    protected override void Update()
    {
        AnalyzeStatus();
        StateMachine.Update();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    public void StartMovement(Vector2[] newPath)
    {
        movingToTarget = true;
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
                    if (StateMachine.currentState.GetType() == typeof(MovementState))
                    {
                        movingToTarget = false;
                        inputDirection = Vector2.zero;
                        ((MovementState)StateMachine.currentState).PathEndReached();
                    }
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            inputDirection = ((Vector3)currentWaypoint - transform.position) * Time.deltaTime * movementSpeed;

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, movementSpeed * Time.deltaTime);
            yield return null;
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
                //Call Interact and pass this as parameter
                IInteractable temp = item.GetComponent<IInteractable>();
                if (tryInteract)
                {
                    //Debug.Log("Try to interact");
                    temp.Interact(this);
                    tryInteract = false;
                }
            }
        }
    }

    protected override void SpriteFlip(bool inverted)
    {
        bool flip;
        flip = inputDirection.x > 0 ? true : false;
        Sr.flipX = flip;
        if (inverted)
        {
            Sr.flipX = !flip;
        }
    }


    //Unnecessary stuff for pedestrian
    protected override void GetInput()
    {
    }
    protected override void Death()
    {
    }
    protected override void Attack()
    {
    }
    protected override void Beg()
    {
    }
    public override void ConsumeItem(BaseItem item)
    {
    }
    public override void Gather(List<BaseItem> items)
    {
    }
    public override void ReturnBottle()
    {
    }
    public override void Buy(BaseItem item)
    {
    }
    public override void Sleep()
    {
    }
    public override void Sleep(int hours)
    {
    }
}