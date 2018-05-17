using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiqourStoreButtonAction : ButtonAction
{
    public override void Action(BaseItem item)
    {
        PlayerController.pl.Buy(item);   
    }

}
