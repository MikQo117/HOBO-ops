using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiqourStoreButtonCollection : ButtonCollection
{
    public List<BaseItem> ItemsToSell;
    protected override void AssignButtonClicks()
    {
        base.AssignButtonClicks();

        ActionToAssign = new ShopButtonAction();
        if (ItemsToSell.Count > 0)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                BaseItem item = ItemsToSell[i];
                UnityEngine.Events.UnityAction buyAction = () => ActionToAssign.Action(item);
                buttons[i].onClick.AddListener(buyAction);
            }
        }
    }
}
