using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UImanager_Player : MonoBehaviour
{

    //StatusBars variables
    private const float     maxValue = 100;
    public Image            HealthBar;
    public Image            SanityBar;
    public Image            StaminaBar;

    //Inventory variables
    private List<Transform> inventoryObjects = new List<Transform>();
    private bool showing = false;
    // Use this for initialization
    private void StatusBarValueChanger()
    {
        HealthBar.fillAmount = Mathf.Clamp01(PlayerController.pl.HealthGetter() / maxValue);
        SanityBar.fillAmount = Mathf.Clamp01(PlayerController.pl.SanityGetter() / maxValue);
        StaminaBar.fillAmount = Mathf.Clamp01(PlayerController.pl.StaminaGetter() / maxValue);

    }

    private void Inventory()
    {
        transform.GetChild(0).gameObject.SetActive(showing);

        for (int i = 0; i < inventoryObjects.Count; i++)
        {
            inventoryObjects[i].GetComponentInChildren<Text>().text = PlayerController.pl.CharacterInventory.InventoryList.Count(x => x.BaseItemID == i && x != null).ToString();
        }
    }

    private void UIInput()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            showing = !showing;
        }
    }

    void Start()
    {
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            inventoryObjects.Add(transform.GetChild(0).GetChild(i).transform);
        }
    }

    void Update()
    {
        UIInput();
        StatusBarValueChanger();
        Inventory();
    }
}
