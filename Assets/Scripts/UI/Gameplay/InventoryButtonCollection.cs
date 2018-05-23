using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryButtonCollection : ButtonCollection
{
    public List<BaseItem> InventoryItems;

    protected override void AssignButtonClicks()
    {
        base.AssignButtonClicks();
        ActionToAssign = new InventoryButtonAction();
        if(InventoryItems.Count > 0)
        {
            for(int i = 0; i < buttons.Count; i++)
            {
                BaseItem item = InventoryItems[i];
                UnityEngine.Events.UnityAction consumeAction = () => ActionToAssign.Action(item);
                buttons[i].onClick.AddListener(consumeAction);
            }
        }
    }
}
