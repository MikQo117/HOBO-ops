using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public static MainMenu Instance;
    public GameObject Buttons;
    public GameObject Instruction;
    public GameObject Text;
    public GameObject Return;

    public void MainMenuActive()
    {
        Buttons.SetActive(true);
        Instruction.SetActive(false);
        Return.SetActive(false);
        Text.SetActive(true);
    }
    public void InstructionsActive()
    {
        Buttons.SetActive(false);
        Text.SetActive(false);
        Instruction.SetActive(true);
        Return.SetActive(true);

    }
}
