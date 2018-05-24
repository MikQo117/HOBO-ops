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
                break;
            default:
                break;

        }
    }
}
