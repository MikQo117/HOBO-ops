using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryButtonAction : ButtonAction
{
    public override void Action(BaseItem item)
    {
        PlayerController.pl.ConsumeItem(item);
    }
}
