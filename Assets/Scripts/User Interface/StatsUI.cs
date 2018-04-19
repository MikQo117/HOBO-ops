using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.ObjectModel;

public class StatsUI : MonoBehaviour
{
    [SerializeField]
    private const float maxValue = 100;
    public  Image HealthBar;
    public  Image SanityBar;
    public  Image StaminaBar;
    // Update is called once per frame
    void Update()
    {
        HealthBar.fillAmount  = Mathf.Clamp01( PlayerController.pl.Health / maxValue);
        SanityBar.fillAmount  = Mathf.Clamp01( PlayerController.pl.Sanity / maxValue);
        StaminaBar.fillAmount = Mathf.Clamp01( PlayerController.pl.Stamina / maxValue);
    }    
}
