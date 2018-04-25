using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Inventory : MonoBehaviour
{
    //Inventory variable which is used by character class
    public List<BaseItem> InventoryList = new List<BaseItem>();

    public void AddItemToInventory(Consumable item)
    {
        var itemOfInterest = item.ItemBase.Where(x => x != null && x.BaseItemID == item.ConsumableID);
        InventoryList.Add(itemOfInterest.First());
    }

    public void AddItemToInventory(List<BaseItem> items)
    {
        InventoryList.AddRange(items);
    }

    public void RemoveItemFromInventory(int ItemID)
    {
        if (InventoryList.Count(x => x.BaseItemID == ItemID) != 0)
        {
            InventoryList.Remove(InventoryList.Where(x => x != null && x.BaseItemID == ItemID).FirstOrDefault());
        }
    }
    public void RemoveItemFromInventory(BaseItem item)
    {
        InventoryList.Remove(item);
    }
}
