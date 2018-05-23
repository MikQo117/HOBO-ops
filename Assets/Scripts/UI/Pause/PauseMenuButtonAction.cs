using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuButtonAction : ButtonAction
{
    public override void Action(BaseItem item)
    {
    }

    public override void Action(int ActionID)
    {
        Debug.Log(ActionID);
        switch(ActionID)
        {
            case 0:
                PlayerController.pl.Paused = false;
                UIManager.Instance.PausemenuActive(false);
                break;
            case 1:
                break;
            case 2:
                //Insert main menu call here
                break;
            case 3:
                Application.Quit();
                break;
            case 4:
                PauseMenu.Instance.instruction.SetActive(false);
                PauseMenu.Instance.buttons.SetActive(true);
                PauseMenu.Instance.buttons.SetActive(true);
                break;
            default:
                Debug.Log("u dun guufd");
                break;
        }
    }

}
