using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner pickup;
    [SerializeField]
    public List<GameObject> PickUps = new List<GameObject>();
    public GameObject TempPickUp;
    public List<BaseItem> Items;
    public Transform[] Spawnlocations;
    bool Spawnbottler;
    IEnumerator spawner;
    // Use this for initialization
    void Start()
    {
        pickup = this;
        Spawnbottler = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Spawnbottler)
        {
            SpawnBottle();
            spawner = _Spawner(2.0f);
            StartCoroutine(spawner);
        }
    }

    IEnumerator _Spawner(float timer)
    {
        Spawnbottler = false;
        yield return new WaitForSeconds(timer);
        Spawnbottler = true;
    }
    public void SpawnBottle()
    {
         GameObject clone = Instantiate(TempPickUp, Spawnlocations[Random.Range(0, 2)].position, Quaternion.identity);
        clone.name = PickUpNameList.SandWich.ToString();
        
         
    }

    public void SpawnSandwich()
    {
        GameObject clone = Instantiate(TempPickUp, Spawnlocations[Random.Range(0, 1)].position, Quaternion.identity);
        clone.name = PickUpNameList.SandWich.ToString();
        PickUps.Add(clone);
    }
    enum PickUpNameList
    {
        SandWich,
        Bottle
    }
}
