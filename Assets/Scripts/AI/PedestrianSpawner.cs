using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianSpawner : MonoBehaviour
{
    private bool oldRushHourState, currentRushHourState;
    public List<Vector2> SpawnLocations = new List<Vector2>();
    public int SpawnAmt = 0;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckForRushHour();
    }

    private void CheckForRushHour()
    {
        oldRushHourState = currentRushHourState;
        currentRushHourState = GameManager.Instance.RushHour;

        if (currentRushHourState != oldRushHourState)
        {
            if (currentRushHourState == true)
            {
                //Start spawning
            }
            else
            {
                //Stop spawning
            }
        }
    }
}
