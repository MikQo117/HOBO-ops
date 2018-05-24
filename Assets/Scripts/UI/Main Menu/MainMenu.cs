using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public static MainMenu Instance;
    public GameObject Buttons;
    public GameObject Instruction;
    public GameObject Exit;
    public GameObject Return;
    public GameObject Credits;



    public void MainMenuActive()
    {
        Instruction.SetActive(false);
        Return.SetActive(false);
        Credits.SetActive(false);
        Buttons.SetActive(true);
        Exit.SetActive(true);
    }
    public void InstructionsActive()
    {
        Buttons.SetActive(false);
        Exit.SetActive(false);
        Credits.SetActive(false);
        Instruction.SetActive(true);
        Return.SetActive(true);
    }

    public void CreditsActive()
    {
        Buttons.SetActive(false);
        Exit.SetActive(false);
        Instruction.SetActive(false);
        Return.SetActive(true);
        Credits.SetActive(true);
    }

    public void Awake()
    {
        Instance = this;
        MainMenuActive();
    }
}
