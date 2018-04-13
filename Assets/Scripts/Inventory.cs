using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Inventory : MonoBehaviour
{
    //Inventory Variables Which can be called by other scripts
    public List<BaseItem>     InventoryList;


    public void AddItemToInventory(Consumable item)
    {
        var itemOfInterest = item.ItemBase.Where(x => x != null && x.BaseItemID == item.ConsumableID);
        InventoryList.Add(itemOfInterest.First());
    }  

    public void RemoveItemFromInventory(BaseItem item)
    {
        InventoryList.Remove(item);
    }
}
