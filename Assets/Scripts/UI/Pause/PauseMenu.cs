using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;
    public GameObject buttons;
    public GameObject instruction;
    public GameObject pausetext;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(instruction.activeInHierarchy)
        {
            buttons.SetActive(false);
            pausetext.SetActive(false);
        }
        else
        {
            buttons.SetActive(true);
            pausetext.SetActive(true);
        }
    }
}
