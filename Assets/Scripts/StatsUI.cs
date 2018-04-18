using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class StatsUI : MonoBehaviour
{
    protected List<Transform> childobjects = new List<Transform>();
    public volatile List<float> DisplayValue = new List<float>();
    public string StatName { get; set; }
    // Use this for initialization
    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            childobjects.Add(transform.GetChild(i).transform);
        }
        AddToList(PlayerController.pl.Health, PlayerController.pl.Sanity, PlayerController.pl.DrunkAmount);
    }

    // Update is called once per frame
    void Update()
    {

        for(int i = 0; i< childobjects.Count; i++)
        {
            foreach(float value in DisplayValue)
            childobjects[i].GetComponentInChildren<Image>().fillAmount = DisplayValue[i];
        }
    }

    void AddToList(params float[] list)
    {
        for(int i = 0; i< list.Length; i++)
        {
            DisplayValue.Add(list[i]);
        }
    }
}
