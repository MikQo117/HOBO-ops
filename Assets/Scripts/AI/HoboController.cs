using System;
using System.Collections;
using System.Collections.Generic;
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

    protected override float Health
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
                hpState = ThresholdState.Well;
            }
            else if (value >= 40 && value < 20)
            {
                hpState = ThresholdState.Low;
            }
            else
            {
                hpState = ThresholdState.Critical;
            }
        }
    }

    protected override float Sanity
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
                spState = ThresholdState.Well;
            }
            else if (value >= 30 && value < 15)
            {
                spState = ThresholdState.Low;
            }
            else
            {
                spState = ThresholdState.Critical;
            }
        }
    }

    private void AnalyzeStates()
    {
        if (hpState == ThresholdState.Well && spState == ThresholdState.Well)
        {
            //Idle actions
            //Debug.Log("Hobo AI idle");
        }
        else if (hpState == ThresholdState.Low)
        {
            //Scavenge
            //Debug.Log("Hobo AI scavenging");
        }
        else
        {
            //Beg?!?
            //Debug.Log("Hobo AI critical action");
        }
    }

    private void Scavenge()
    {
        //Get nearest trashcan
        //Move to it
        //when reached, search it
        //If food found, eat it
        //bottle do nothing special
        //repeat
    }

    /// <summary>
    /// Called when callback occurs from PathRequestManager.RequestPath().
    /// </summary>
    /// <param name="newPath">The new calulated path.</param>
    /// <param name="pathSuccess">Was the pathfind successful?</param>
    public void OnPathFound(Vector2[] newPath, bool pathSuccess)
    {
        if (pathSuccess)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
            pathSuccess = false;
        }
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
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, sprintSpeed * Time.deltaTime);
        }
    }


    public override void ConsumeItem(int itemID)
    {
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
    }

    public override void ReturnBottle()
    {
    }
    protected override void GetInput()
    {
        //Test
        if (Input.GetMouseButtonDown(0))
        {
            PathRequestManager.RequestPath(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), OnPathFound);
        }
    }

    public override void Buy(BaseItem item)
    {
        
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        AnalyzeStates();
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
    Well,
    Low,
    Critical
}
