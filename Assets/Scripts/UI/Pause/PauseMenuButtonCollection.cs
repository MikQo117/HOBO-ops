using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuButtonCollection : ButtonCollection
{
    public Button InstructionButton;

    protected override void OnEnable()
    {
        Debug.Log("Active");
        transform.GetComponentInParent<PauseMenu>().PauseMenuActive();
    }


    protected override void AssignButtonClicks()
    {
        base.AssignButtonClicks();
        ActionToAssign = new PauseMenuButtonAction();
        
        for(int i = 0; i < buttons.Count; i++)
        {
            int temp = i;
            UnityEngine.Events.UnityAction pressAction = () => ActionToAssign.Action(temp);
            buttons[i].onClick.AddListener(pressAction);
        }

        UnityEngine.Events.UnityAction returnaction = () => ActionToAssign.Action(4);
        InstructionButton.onClick.AddListener(returnaction);
    }
}
