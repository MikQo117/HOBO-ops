using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopButtonCollection : ButtonCollection
{
    public List<BaseItem> ItemsToSell;
    public Button ReturnBottleButton;

    protected override void Awake()
    {
    }

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
        UnityEngine.Events.UnityAction returnAction = () => ActionToAssign.ReturnBottle();
        ReturnBottleButton.onClick.AddListener(returnAction);
    }

}
