using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButtonAction : ButtonAction
{
    public override void Action(BaseItem item)
    {
        PlayerController.pl.Buy(item);
    }

    public override void ReturnBottle()
    {
        PlayerController.pl.ReturnBottle();
    }
}
