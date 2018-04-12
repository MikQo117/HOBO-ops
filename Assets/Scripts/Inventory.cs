using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Inventory : MonoBehaviour
{
    //Inventory Variables
    public static Inventory Inv;
    public List<BaseItem> InventoryList;
    public Texture[] Locations;
    //UI Variables
    [SerializeField]
    protected List<Transform> childObjects = new List<Transform>();

    //Player Variables
    public int Health;
    public int Sanity;
    public int Drunk;
    public int i = 0;

    public void Click(int index)
    {
        if (InventoryList.ElementAtOrDefault(index) == null) return;

        PlayerController.pl.ConsumeItem(index);

        childObjects[index].GetComponent<Image>().sprite = null;

        InventoryList.RemoveAt(index);
        /*   childObjects[index].transform.position = Locations[i - 1]; // indexin valitsema palikka oikeaan paikkaan

           IEnumerable<Transform> kk = childObjects.Where(x => x != childObjects[index]); // valitaan kaikki muut paitsi se, jonka indexi on valinnut

              aa = kk.ToArray();

           for (int a = 0; a < childObjects.Count - 1; a++)
           {
               aa[a].gameObject.transform.position = Locations[a];
           }
           */

    }



    public void AddButtonSprite()
    {
        for (int a = 0; a < InventoryList.Count; a++) 
        childObjects[a].GetComponent<Image>().sprite = InventoryList[a].sprite;
    }
    void Awake()
    {
        Inv = this;
    }

    void Start()
    {
        for (i = 0; i < transform.childCount; i++)
        {

            childObjects.Add(gameObject.transform.GetChild(i).transform);
            //Locations.Add(gameObject.transform.GetChild(i).transform.position);
        }
    }









}
