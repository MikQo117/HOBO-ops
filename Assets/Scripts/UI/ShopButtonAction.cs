using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButtonAction : ButtonAction
{

    public void Action(BaseItem item)
    {
        PlayerController.pl.Buy(item);
    }
}
