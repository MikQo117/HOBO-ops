using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LitterPickUp : MonoBehaviour
{
    public static LitterPickUp pickup;
    [SerializeField]
   public List<GameObject> PickUps = new List<GameObject>();

    public GameObject TempPickUp;
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
            spawner = Spawner(2.0f);
            StartCoroutine(spawner);
        }
    }

    IEnumerator Spawner(float timer)
    {
        Spawnbottler = false;
        yield return new WaitForSeconds(timer);
        Spawnbottler = true;
    }
    public void SpawnBottle()
    {
        GameObject clone = Instantiate(TempPickUp, Spawnlocations[Random.Range(0, 2)].position, Quaternion.identity);
        PickUps.Add(clone);
    }
}
