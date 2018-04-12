using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner pickup;
    [SerializeField]
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
        int randomNumber = Random.Range(1, 1);
        GameObject clone = Instantiate(TempPickUp, Spawnlocations[Random.Range(0, 2)].position, Quaternion.identity);
    }

    public void SpawnSandwich()
    {
        GameObject clone = Instantiate(TempPickUp, Spawnlocations[Random.Range(0, 1)].position, Quaternion.identity);
    }
    enum PickUpNameList
    {
        SandWich,
        Bottle
    }
}
