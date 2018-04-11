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
    public List<Transform> Locations = new List<Transform>();
    //UI Variables
    [SerializeField]
    protected List<GameObject> inventoryButtons;

    //Player Variables
    public int Health;
    public int Sanity;
    public int Drunk;
    public int i;

    //inventorylist on suurempi kuin napit 

    void Awake()
    {
        Inv = this;
    }

    void Start()
    {

        for (i = 0; i < transform.childCount; i++)
        {
            inventoryButtons.Add(gameObject.transform.GetChild(i).gameObject);
            Locations.Add(gameObject.transform.GetChild(i).transform);
        }   
    }

    // Update is called once per frame
    void Update()
    {
        for (int a = 0; a < InventoryList.Count; a++)
        {
            inventoryButtons[a].GetComponent<Button>().image.sprite = InventoryList[a].sprite;
        }
    }

   public void Click(int index)
    {
        if (!InventoryList.Any()) return;

        InventoryList.RemoveAt(index);
        inventoryButtons[index].transform.position = Locations[i-1].transform.position; // indexin valitsema palikka oikeaan paikkaan

       IEnumerable<GameObject> kk = inventoryButtons.Where(x => x != inventoryButtons[index]); // valitaan kaikki muut paitsi se, jonka indexi on valinnut
          GameObject[] aa =  kk.ToArray();
        for (int a = 0; a < inventoryButtons.Count -1; a++)
            aa[a].transform.position = Locations[a].transform.position;
    }

}
