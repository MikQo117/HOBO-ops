using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianTarget : MonoBehaviour   //, IInteractable
{
    private void Awake()
    {
        GameManager.Instance.GetPedestrianTargets.Add(this);
    }
}
