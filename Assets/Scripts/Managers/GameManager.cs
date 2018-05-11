using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Interaction variables
    public List<IInteractable> interactables = new List<IInteractable>();
    private List<TrashSpawn>   trashSpawns = new List<TrashSpawn>();
    public List<Collider2D>    interactablesColliders;
    private float              spawnableItemIndex;
    private const float        originalSpawnTimer = 30.0f;
    float                      spawntimer;

    private static GameManager instance;

    private float timer;
    public int TrashcansLooted = 0;
    public int BeersBought = 0;
    public int WhiskeyBought = 0;
    public int FoodBought = 0;
    public int TimesSlept = 0;


    public List<TrashSpawn> GetTrashSpawns
    {
        get
        {
            return trashSpawns;
        }
    }

    //Getters
    static public GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void AddItemToTrashCans()
    {
        spawntimer -= Time.deltaTime;
        if (spawntimer <= 0)
        {
            for (int i = 0; i < trashSpawns.Count; i++)
            {
                trashSpawns[i].SpawnItems();
            }
            ResetTimer();
        }
    }

    private void ResetTimer()
    {
        spawntimer = originalSpawnTimer;
    }

    private void Awake()
    {
        timer = 0;
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        foreach (IInteractable item in interactables)
        {
            interactablesColliders.Add(item.GetCollider());
        }
        foreach (IInteractable item in interactables)
        {
            if (item is TrashSpawn)
            {
                trashSpawns.Add((TrashSpawn)item);
            }
        }
        ResetTimer();
    }

    private void Update()
    {
        AddItemToTrashCans();
        timer += Time.deltaTime;
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
                    Debug.Log("Write");
                    sw.WriteLine("Hola");
                    sw.WriteLine("Henlo");
                    sw.WriteLine("Elapsed time in seconds: " + (int)timer);
                }
            }
            int index = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(index);
        }
    }
}
