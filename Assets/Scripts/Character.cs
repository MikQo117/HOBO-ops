using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    //Character stats
    protected int     health;
    protected int     sanity;
    protected float   stamina;
    protected float   staminaRecoveryRate;

    //Max stats
    protected int     maxHealth;
    protected int     maxSanity;
    protected int     maxStamina;

    //Character movement
    protected float   movementSpeed;
    protected float   sprintSpeed;
    protected Vector2 movementDirection;
    protected bool    sprinting;

    //Exhaust variables
    protected bool    exhausted; //Must disable sprinting
    private float     exhaustTimer;
    protected float   exhaustDuration;

    //Get & Set
    protected int Health
    {
        get
        {
            return health;
        }

        set
        {
            if (value <= 0)
            {
                Death();
            }
            else
            {
                health = Mathf.Clamp(value, 0, maxHealth); 
            }
        }
    }
    protected int Sanity
    {
        get
        {
            return sanity;
        }

        set
        {
            if (value <= 0)
            {
                Death();
            }
            sanity = Mathf.Clamp(value, 0, maxSanity);
        }
    }
    protected float Stamina
    {
        get
        {
            return stamina;
        }

        set
        {
            if (value <= 0f)
            {
                Exhausted();
                stamina = 0f;
            }
            else
            {
                stamina = Mathf.Clamp(value, 0f, maxStamina);
            }
        }
    }


    // Use this for initialization
    protected virtual void Start()
    {
        exhaustTimer = exhaustDuration;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        GetInput();
        RecoverStamina();
        ExhaustTimer();
        ApplyMovement();
    }

    protected virtual void ApplyMovement()
    {
        transform.Translate(movementDirection.normalized * movementSpeed * Time.deltaTime);
    }

    protected abstract void GetInput();

    protected abstract void Death();
    protected abstract void Attack();
    protected abstract void ConsumeItem();
    protected abstract void Gather();
    protected abstract void Beg();

    public void TakeHealthDamage(int amount)
    {
        Health -= amount;
    }
    public void TakeSanityDamage(int amount)
    {
        Sanity -= amount;
    }

    protected virtual void RecoverStamina()
    {
        if (!sprinting)
        {
            if (Stamina < maxStamina)
            {
                Stamina += staminaRecoveryRate * Time.deltaTime;  
            }
        }
    }

    protected virtual void Exhausted()
    {
        if (exhausted == false)
        {
            exhausted = true;
        }
    }
    protected void ExhaustTimer()
    {
        //Only tick when exhausted
        if (exhausted)
        {
            //Still exhausted
            if (exhaustTimer > 0f)
            {
                exhaustTimer -= Time.deltaTime; 
            }
            //Not exhausted
            else
            {
                exhausted = false;
                exhaustTimer = exhaustDuration;
            }
        }
    }
}
