using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtonCollection : ButtonCollection
{
    public Button InstructionButton;
    public Button CreditsButton;

    protected override void AssignButtonClicks()
    {
        base.AssignButtonClicks();
        ActionToAssign = new MainMenuButtonAction();

        for (int i = 0; i < buttons.Count; i++)
        {
            int temp = i;
            UnityEngine.Events.UnityAction pressAction = () => ActionToAssign.Action(temp);
            buttons[i].onClick.AddListener(pressAction);
        }

        UnityEngine.Events.UnityAction returnaction = () => ActionToAssign.Action(4);
        InstructionButton.onClick.AddListener(returnaction);
        CreditsButton.onClick.AddListener(returnaction);
    }
}
