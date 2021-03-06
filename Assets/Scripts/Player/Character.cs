﻿using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    //Character stats
    protected float        health = 50;
    protected float        sanity = 50;
    protected float        drunkAmount;
    protected float        stamina = 100;
    protected float        staminaRecoveryRate = 2;
    protected decimal      moneyAmount = 2M;

    //Max stats
    protected int          maxHealth = 100;
    protected int          maxSanity = 100;
    protected int          maxStamina= 100;

    //stat decay variables
    protected const float healthDecay  = 0.5f;
    protected const float sanityDecay  = 0.1f;
    protected const float staminaDecay = 10.0f;

    //Character movement
    [SerializeField]
    protected float       movementSpeed = 2.2f;
    [SerializeField]
    protected float       sprintSpeed = 3.7f;
    protected Vector3     movementDirection;
    protected bool        sprinting;
    protected Vector3     inputDirection;

    //Collision variables
    [SerializeField]
    protected int          NoOfRays;
    [SerializeField]
    private LayerMask      raycastMask;
    private float          lengthOfRay;

    //Animation variables
    protected Animator     animator;
    private Sprite         currentIdleSprite;
    [SerializeField]
    private Sprite[]       idleSprites;
    private SpriteRenderer sr;

    //Audio variables
    protected AudioSource  audioSource;
    [SerializeField]       
    protected string       characterName = "";
    [SerializeField]       
    protected int          minVoiceLineInterval, maxVoiceLineInterval;
    protected float        randomAudioTimer = 0f;
    protected float        currentAudioInterval = 25f;

    //Exhaust variables
    [SerializeField]
    protected bool         exhausted; //Must disable sprinting
    private float          exhaustTimer;
    protected float        exhaustDuration = 5;

    //Inventory variable
    protected Inventory characterInventory;

    //Interaction variables
    [SerializeField]
    protected new Collider2D collider;
    protected bool           canInteract;
    private Collider2D       interactableCollider;

    //Sleeping variables
    protected bool        canSleep;
    protected float       sleepTimer;
    protected bool        sleeping;
    protected float       sanityGain = 0.375f;
    protected float       healthGain = 1.5f;
    protected const float sleepTime  = 180.0f;

    //Get & Set
    public virtual float Health
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
    public virtual float Sanity
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
    public virtual float Stamina
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
    public virtual decimal MoneyAmount
    {
        get { return moneyAmount; }        
    }
    public bool Sprinting
    {
        get { return sprinting; }
    }
    public Inventory Inventory
    {
        get
        {
            return characterInventory;
        }
        set
        {
            characterInventory = value;
        }
    }
    public float SleepTimer
    {
        get { return sleepTimer; }
    }

    public Collider2D InteractableCollider
    {
        get
        {
            return interactableCollider;
        }

        set
        {
            interactableCollider = value;
        }
    }

    public SpriteRenderer Sr
    {
        get
        {
            return sr;
        }

        set
        {
            sr = value;
        }
    }

    //Methods Start Here

    protected virtual void ApplyMovement()
    {
        if (sprinting && !exhausted)
        {
            Stamina -= staminaDecay * Time.deltaTime;
            transform.Translate(movementDirection * sprintSpeed * Time.deltaTime);
            
        }
        else
        {
            transform.Translate(movementDirection * movementSpeed * Time.deltaTime);
        }
    }

    protected abstract void GetInput();

    protected abstract void Death();

    //Minigame Methods
    protected abstract void Attack();
    protected abstract void Beg();

    //Interactable Methods
    public abstract void ConsumeItem(BaseItem item);
    public abstract void Gather(List<BaseItem> items);
    public abstract void ReturnBottle();
    public abstract void Buy(BaseItem item);
    public abstract void Sleep();
    public abstract void Sleep(int hours);
    
   

    /// <summary>
    /// Checks if the character is intersecting a interactable collider.
    /// </summary>
    protected virtual void CheckForInteraction()
    {
        //For through all interactable colliders, and see if intersects
        foreach (Collider2D item in GameManager.Instance.interactablesColliders)
        {
            //If contains, get component from collider, typeof IInteractable
            if (collider.bounds.Intersects(item.bounds))
            {
                UIManager.Instance.Eprompt(true);
                InteractableCollider = item;
                //Call Interact and pass this as parameter
                item.GetComponent<IInteractable>().Interact(this);
                canInteract = true;
                break;
            }
            else
            {
                UIManager.Instance.Eprompt(false);
                canInteract = false;
            }
        }
    }

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

            if (sprinting)
            {
                if (Stamina <= 0)
                {
                    Exhausted();
                    ExhaustTimer();
                }
            }
        }
    }
    protected virtual void Collision()
    {
        Ray2D ray;
        Vector2 origin;

        //Origin starting point determination from characters collider whenever the character is moving horizontally or vertically
        if (inputDirection.y != 0)
        {
            origin = new Vector2(collider.bounds.min.x , collider.bounds.center.y);
        }
        else
        {
            origin = new Vector2(collider.bounds.center.x , (collider.bounds.min.y));
        }


        //Distance between rays determined by direction
        float distanceBetweenRaysX = (collider.bounds.size.x) / (NoOfRays - 1);
        float distanceBetweenRaysY = (collider.bounds.size.y) / (NoOfRays - 1);

        //loop for raycasting
        for (int i = 0; i < NoOfRays; i++)
        {
            ray = new Ray2D(origin, (movementDirection).normalized);
            RaycastHit2D BuildingHit = Physics2D.Raycast(ray.origin, ray.direction, lengthOfRay, raycastMask);
            if (inputDirection.x != 0 && inputDirection.y == 0)
                origin += new Vector2(0, distanceBetweenRaysY);
            else
                origin += new Vector2(distanceBetweenRaysX, 0);

            if (BuildingHit)
            {
                movementDirection -= Vector3.Project(movementDirection.normalized, BuildingHit.normal.normalized);
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

    protected void SleepTimerChecker()
    {
        if(sleepTimer > 0)
        {
            sleepTimer -= Time.deltaTime;
        }
        else
        {
            canSleep = true;
        }
    }

    protected void AnimationChanger()
    {
        if (sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
        }
        if (Mathf.Abs(inputDirection.x) + Mathf.Abs(inputDirection.y) != 0)
        {
            //sideways movement animator changer
            if (inputDirection.x != 0 && inputDirection.y == 0)
            {
                animator.Play(AnimationClips.WalkSideways.ToString());
                currentIdleSprite = idleSprites[3];
                SpriteFlip(true);
            }

            //walking down animator changer
            if (inputDirection.x == 0 && inputDirection.y < 0)
            {
                animator.Play(AnimationClips.WalkDown.ToString());
                currentIdleSprite = idleSprites[0];
                SpriteFlip(false);
            }

            //walking upwards animation changer
            if (inputDirection.x == 0 && inputDirection.y > 0)
            {
                animator.Play(AnimationClips.WalkUp.ToString());
                currentIdleSprite = idleSprites[4];
                SpriteFlip(false);
            }

            //strafing upwards animation changer
            if (inputDirection.x != 0 && inputDirection.y > 0)
            {
                animator.Play(AnimationClips.WalkStrafeUp.ToString());
                currentIdleSprite = idleSprites[1];
                SpriteFlip(false);
            }

            //strafing downwards animation changer
            if (inputDirection.x != 0 && inputDirection.y < 0)
            {
                animator.Play(AnimationClips.WalkStrafeDown.ToString());
                currentIdleSprite = idleSprites[2];
                SpriteFlip(false);
            }
        }
        else
        {
            if (Sr.sprite != currentIdleSprite)
            {
                Sr.sprite = currentIdleSprite;
            }
        }
            
    }

    protected virtual void SpriteFlip(bool inverted)
    {
        bool flip;
        flip = inputDirection.x < 0 ? true : false;
        Sr.flipX = flip;
        if(inverted)
        {
            Sr.flipX = !flip;
        }
    }

    protected virtual void StatsDecay()
    { 
            Health -= healthDecay * Time.deltaTime;
            Sanity -= sanityDecay * Time.deltaTime;
    }

    enum AnimationClips
    {
        Idle,
        WalkSideways,
        WalkDown,
        WalkStrafeUp,
        WalkStrafeDown,
        WalkUp
    }

    protected virtual void PlayClip(AudioClip clip)
    {
        if (clip != audioSource.clip)
        {
            audioSource.clip = clip;
        }
        audioSource.Play();
    }

    protected virtual void RandomSounds()
    {
        randomAudioTimer += Time.deltaTime;
        if (randomAudioTimer >= currentAudioInterval)
        {
            randomAudioTimer = 0;
            currentAudioInterval = (int)Random.Range(minVoiceLineInterval, maxVoiceLineInterval);
            //Play clip
        }
    }

    protected virtual void LoadAudio()
    {

    }

    protected virtual void Awake()
    {

    }
    // Use this for initialization
    protected virtual void Start()
    {
        randomAudioTimer = 0f;
        exhaustTimer = exhaustDuration;
        lengthOfRay = collider.bounds.extents.magnitude;
        Sr = GetComponent<SpriteRenderer>();
        Inventory = gameObject.AddComponent<Inventory>();
        animator = GetComponent<Animator>();
        currentIdleSprite = idleSprites[0];
        collider = GetComponent<Collider2D>();
        canSleep = true;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
            GetInput();
            CheckForInteraction();
            StatsDecay();
            RecoverStamina();
            ExhaustTimer();
            SleepTimerChecker();
            Collision();
            ApplyMovement();
        RandomSounds();
    }
    protected virtual void LateUpdate()
    {
        AnimationChanger();
    }
}
