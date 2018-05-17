using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopButtonCollection : ButtonCollection
{
    public List<BaseItem> ItemsToSell;

    protected override void Awake()
    {
        AssignButtonClicks();
    }

    protected override void AssignButtonClicks()
    {
        ActionToAssign = new ShopButtonAction();
        if (ItemsToSell.Count < 0)
        {
            buttons = GetComponentsInChildren<Button>().ToList();
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].onClick.AddListener(delegate
                {
                    ((ShopButtonAction)ActionToAssign).Action(ItemsToSell[i]);
                });
            }
        }
    }
}
