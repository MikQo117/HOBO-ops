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
    [SerializeField]
    protected float   movementSpeed;
    [SerializeField]
    protected float   sprintSpeed;
    protected Vector2 movementDirection;
    protected bool    sprinting;

    //Collision variables
    [SerializeField]
    protected int NoOfRays;
    [SerializeField]
    private LayerMask raycastMask;
    private float lengthOfRay;

    //Animation variables
    protected Animator animator;
    [SerializeField]
    private Sprite currentIdleSprite = null;
    [SerializeField]
    private Sprite[] idleSprites;
    private SpriteRenderer Sr;

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
        lengthOfRay = GetComponent<Collider2D>().bounds.extents.magnitude;
        Sr = GetComponent<SpriteRenderer>();
        
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        GetInput();
        RecoverStamina();
        ExhaustTimer();
        AnimationChanger();
        ApplyMovement();
    }

    protected virtual void ApplyMovement()
    {
        if (!Collision())
        {
            if (sprinting)
            {
                transform.Translate(movementDirection.normalized * sprintSpeed * Time.deltaTime); 
            }
            else
            {
                transform.Translate(movementDirection.normalized * movementSpeed* Time.deltaTime);
            }
        }
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
    protected virtual bool Collision()
    {

        Vector2 origin = new Vector2(GetComponent<Collider2D>().bounds.min.x + 0.015f, GetComponent<Collider2D>().bounds.min.y);
        Ray2D ray;

        float distanceBetweenRays = (GetComponent<Collider2D>().bounds.size.x - 2 * 0.015f) / (NoOfRays - 1);

        for (int i = 0; i < NoOfRays; i++)
        {

            ray = new Ray2D(origin, (movementDirection));

            Debug.DrawRay(origin, (movementDirection), Color.blue);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, lengthOfRay, raycastMask);
            if (hit)
            {
                transform.Translate((movementDirection.normalized + hit.normal) * Time.deltaTime);
                return true;
            }

            /*if (hit.collider != null)
            {
                if (GameManager.manager.PickUps.Contains(hit.collider.gameObject))
                {
                    Debug.Log("äksdee");
                    health += 10;
                    GameManager.manager.PickUps.Remove(hit.collider.gameObject);
                    return true;

                }
            }*/

            origin += new Vector2(distanceBetweenRays, 0);
        }
        return false;
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

    protected void AnimationChanger()
    {
        //sideways movement animator changer
        if (movementDirection.x != 0 && movementDirection.y == 0) { animator.Play(AnimationClips.WalkSideways.ToString()); currentIdleSprite = idleSprites[3]; SpriteFlip(); }

        //walking down animator changer
        if (movementDirection.x == 0 && movementDirection.y < 0) { animator.Play(AnimationClips.WalkDown.ToString()); currentIdleSprite = idleSprites[0]; SpriteFlip(); }

        //walking upwards animation changer
        if (movementDirection.x == 0 && movementDirection.y > 0) { animator.Play(AnimationClips.WalkUp.ToString()); currentIdleSprite = idleSprites[4]; SpriteFlip(); }

        //strafing upwards animation changer
        if (movementDirection.x != 0 && movementDirection.y > 0) { animator.Play(AnimationClips.WalkStrafeUp.ToString()); currentIdleSprite = idleSprites[1]; SpriteFlip(); }

        //strafing downwards animation changer
        if (movementDirection.x != 0 && movementDirection.y < 0) { animator.Play(AnimationClips.WalkstrafeDown.ToString()); currentIdleSprite = idleSprites[2]; SpriteFlip(); }

        //Idle
        if (movementDirection.x == 0 && movementDirection.y == 0) { animator.Play(AnimationClips.Idle.ToString()); Sr.sprite = idleSprites[2]; }

    }

    protected void SpriteFlip()
    {
        bool flip;
        flip = movementDirection.x > 0 ? true : false;
        Sr.flipX = flip;
    }

    enum AnimationClips
    {
        Idle,
        WalkSideways,
        WalkDown,
        WalkStrafeUp,
        WalkstrafeDown,
        WalkUp
    }
}
