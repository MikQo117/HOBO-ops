using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtonAction : ButtonAction
{
    public override void Action(BaseItem item)
    {
    }

    public override void Action(int ActionID)
    {
        switch(ActionID)
        {
            case 0:
                UnityEngine.SceneManagement.SceneManager.LoadScene("HoboOpsMap1");
                break;
            case 1:
                MainMenu.Instance.InstructionsActive();
                break;
            case 2:
                MainMenu.Instance.CreditsActive();
                break;
            case 3:
                Application.Quit();
                break;
            case 4:
                MainMenu.Instance.MainMenuActive();
                break;
            default:
                break;

        }
    }
}
