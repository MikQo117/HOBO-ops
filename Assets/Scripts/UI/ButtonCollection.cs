using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCollection : MonoBehaviour
{
    public ButtonAction ActionToAssign;
    protected List<Button> buttons;

    // Use this for initialization
    protected virtual void Awake()
    {
        AssignButtonClicks();
    }

    protected virtual void OnEnable()
    {
        if (buttons == null)
        {
            AssignButtonClicks();
        }
    }

    protected virtual void AssignButtonClicks()
    {
        buttons = GetComponentsInChildren<Button>().ToList();
        foreach (Button item in buttons)
        {
            item.onClick.AddListener(delegate { ActionToAssign.Action(); });
        }
    }

}