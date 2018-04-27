using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character
{

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        if (InputManager.Instance == null)
        {
            GameObject inputManager = new GameObject();
            inputManager.AddComponent<InputManager>();
            inputManager.name = "InputManager";
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    protected override void Attack()
    {
    }

    protected override void Beg()
    {
    }

    protected override void ConsumeItem()
    {
    }

    protected override void Death()
    {
    }

    protected override void Gather()
    {
    }

    protected override void GetInput()
    {
        if (InputManager.Instance.AxisDown("Horizontal") || InputManager.Instance.AxisDown("Vertical"))
        {
            //Determines wanted direction
            Vector2 direction = Vector2.right * InputManager.Instance.XAxis + Vector2.up * InputManager.Instance.YAxis;

            float directionMagnitude = direction.magnitude;

            if (directionMagnitude > 1)
            {
                directionMagnitude = 1;
            }

            //Destination is unit vector * Speed and directions magnitude effects on how much speed is used;
            movementDirection = direction.normalized *  directionMagnitude;
        }
        else
        {
            movementDirection = Vector3.zero;
        }

        //Sprint
        if (InputManager.Instance.AxisDown("Fire3"))
            sprinting = true;
        else
            sprinting = false;
    }
}
