using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject Player;

    private SpriteRenderer PlayerSR;

    public int PlayerOrderInLayer
    {
        get
        {
            return PlayerSR.sortingOrder;
        }
    }
    public Transform PlayerTransform
    {
        get
        {
            return Player.transform;
        }
    }

    //Interaction variables
    public List<IInteractable> interactables = new List<IInteractable>();
    private List<TrashSpawn> trashSpawns = new List<TrashSpawn>();
    private List<PedestrianTarget> pedestrianTargets = new List<PedestrianTarget>();
    public List<Collider2D> interactablesColliders;
    [SerializeField]
    private float originalSpawnTimer = 30.0f;
    float spawntimer;

    //Daysystem variables
    private float[] intervals = { 0.0f, 114.0f, 228.0f, 342.0f,456.0f };
    private float dayTimer = 250.0f;
    private const float regulartimeDrop = 30.0f;
    private const float rushHourTimeDrop = 15.0f;
    private bool rushHour;
    private const float hour = 18.75f;
    private int  daycount;

    private static GameManager instance;
    //Statistic variables
    private float timer;
    public int TrashcansLooted = 0;
    public int BeersBought = 0;
    public int WhiskeyBought = 0;
    public int FoodBought = 0;
    public int TimesSlept = 0;
    public int BeersConsumed = 0;
    public int WhiskeyConsumed = 0;
    public int FoodConsumed = 0;

    public List<PedestrianTarget> GetPedestrianTargets
    {
        get { return pedestrianTargets; }
    }

    public List<TrashSpawn> GetTrashSpawns
    {
        get { return trashSpawns; }
    }

    static public GameManager Instance
    {
        get { return instance; }
    }

    public bool RushHour
    {
        get { return rushHour; }
    }

    public float DayTimer
    {
        get { return dayTimer; }
        set { dayTimer = value; }
    }

    public float Hour
    {
        get
        {
            return hour;
        }
    }

    //Methods

    private void AddItemToTrashCans()
    {

        if (spawntimer <= 0)
        {
            for (int i = 0; i < trashSpawns.Count; i++)
            {
                trashSpawns[i].SpawnItems();
            }
            ResetTimer();
        }
    }

    private void IntervalChecker()
    {
        RushHourChecker();
        if (!rushHour)
        {
            if (DayTimer > intervals[1])
            {
                originalSpawnTimer = regulartimeDrop;
                UIManager.Instance.DaytimeColorTintChanger(1);
            }

            if (DayTimer >= intervals[4])
            {
                DayTimer = 0.0f;
            }

            if (DayTimer > intervals[3])
            {
                ResetTimer();
                UIManager.Instance.DaytimeColorTintChanger(3);
            }
        }
        DayIncreaser();
    }

    public void DayTimeIncreaser(float hours)
    {
        DayTimer = 0.0f;
        DayIncreaser();
        DayTimer += hours;
    }

    private void DayIncreaser()
    {
        if (DayTimer == 0.0f)
        {
            daycount++;
            UIManager.Instance.DayCounterUpdater(daycount);
        }
    }

    private void RushHourChecker()
    {
        if (DayTimer > intervals[1] && DayTimer < intervals[2])
        {
            rushHour = true;
            originalSpawnTimer = rushHourTimeDrop;
            UIManager.Instance.DaytimeColorTintChanger(2);
        }
        else
        {
            rushHour = false;
        }
    }

    private void ResetTimer()
    {
        spawntimer = originalSpawnTimer;
    }

    private void TimeChanger()
    {
        spawntimer -= Time.deltaTime;
        DayTimer   += Time.deltaTime;
    }

    //Unity methods

    private void Awake()
    {
        timer = 0;
        TrashcansLooted = 0;
        BeersBought = 0;
        WhiskeyBought = 0;
        FoodBought = 0;
        TimesSlept = 0;

        if (instance == null)
        {
            instance = this;
        }

        Player = GameObject.Find("Player");
        PlayerSR = Player.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        foreach (IInteractable item in interactables)
        {
            interactablesColliders.Add(item.GetCollider());
            if (item is TrashSpawn)
            {
                trashSpawns.Add(((TrashSpawn)item));
            }
        }
        ResetTimer();
        daycount = 1;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            DayTimer += 20;
        }
        if (!PlayerController.pl.Paused)
        {
            TimeChanger();
            IntervalChecker();
            AddItemToTrashCans();
            timer += Time.deltaTime;
        }
    }

    private void LateUpdate()
    {
        if(Input.GetKey(KeyCode.R))
        {
            for (int i = 0; i < AssetManager.Instance.AssetBundlesList.Count; i++)
            {
                AssetManager.Instance.AssetBundlesList[i].Unload(true);
            }

            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string name = "/Hobo-Ops QA " + DateTime.Now.ToString().Replace("/", "-").Replace(":", "-") + ".txt";

            if(!File.Exists(path + name))
            {
                using (StreamWriter sw = File.CreateText(path + name))
                {
                    Debug.Log("Write session data to desktop");
                    sw.WriteLine("Elapsed time in seconds: " + (int)timer);
                    sw.WriteLine("Trashcans looted:  " + TrashcansLooted);
                    sw.WriteLine("Beers purchased:   " + BeersBought);
                    sw.WriteLine("Whiskey purchased: " + WhiskeyBought);
                    sw.WriteLine("Food purchased:    " + FoodBought);
                    sw.WriteLine("Times slept:       " + TimesSlept);
                    sw.WriteLine("Whiskey consumed:  " + WhiskeyConsumed);
                    sw.WriteLine("Beer consumed:     " + BeersConsumed);
                    sw.WriteLine("Food consumed:     " + FoodConsumed);
                }
            }
            int index = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(index);
        }
    }
}
