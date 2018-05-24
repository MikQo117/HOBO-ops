using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;
    public GameObject buttons;
    public GameObject instruction;
    public GameObject pausetext;
    public GameObject Return;

    public void PauseMenuActive()
    {
        buttons.SetActive(true);
        pausetext.SetActive(true);
        Return.SetActive(false);
        instruction.SetActive(false);
    }

    public void InstructionScreenActive()
    {
        buttons.SetActive(false);
        pausetext.SetActive(false);
        Return.SetActive(true);
        instruction.SetActive(true);
    }

    public void Awake()
    {
        Instance = this;
    }
}
