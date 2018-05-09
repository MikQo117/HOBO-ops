﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    //StatusBars variables
    private const float maxValue = 100;
    public Image        HealthBar;
    public Image        SanityBar;
    public Image        StaminaBar;
    public Text         BottleText;

    //Inventory variables
    private List<Transform> inventoryObjects = new List<Transform>();
    private bool            showing = false;
    private Text            moneyText;

    //Daytime indicator variables
    float             arrowrotationChanger = 0.0f;
    public GameObject Arrow;

    //ShopWindow Variables
    private GameObject shopWindow;
    private GameObject liqourStoreWindow;
    private Text       liqourStoreText;
    private Text       ReturnBottleText;

    //Pick up Variables
    public GameObject     PickupObject;
    private bool          CRisRunning = false;
    private Vector2       minAnchor;
    private Vector2       maxAnchor;
    private RectTransform recT;
    private Vector2       Offset = new Vector2(0, 0.1f);

    //E-prompt
    private GameObject prompt;
    public Sprite[] EpromptSprites;
    private bool eCoroutineRunning;

    private void StatusBarValueChanger()
    {
        HealthBar.fillAmount  = Mathf.Clamp01(PlayerController.pl.Health / maxValue);
        SanityBar.fillAmount  = Mathf.Clamp01(PlayerController.pl.Sanity / maxValue);
        StaminaBar.fillAmount = Mathf.Clamp01(PlayerController.pl.Stamina / maxValue);
    }

    private void Inventory()
    {
        if (!shopWindow.activeInHierarchy || !liqourStoreWindow.activeInHierarchy)
        {
            for (int i = 0; i < transform.childCount - 6; i++)
            {
                transform.GetChild(i).gameObject.SetActive(showing);
            }
            BottleText.text = PlayerController.pl.Inventory.InventoryList.Count(x => x.BaseItemID == 8).ToString();
            moneyText.text = "Money count " + System.Environment.NewLine + PlayerController.pl.MoneyAmount.ToString("0.##") + "$";
            for (int i = 0; i < inventoryObjects.Count; i++)
            {
                inventoryObjects[i].GetComponentInChildren<Text>().text = PlayerController.pl.Inventory.InventoryList.Count(x => x.BaseItemID == i && x != null).ToString();
            }
        }
    }

    private void UIInput()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            showing = !showing;
        }
    }

    private void DaytimeIndicator()
    {
        arrowrotationChanger                     = GameManager.Instance.DayTimer * 0.79f;
        Arrow.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, -arrowrotationChanger);
    }

    public void ShopWindow(bool Active)
    {
        ReturnBottleText.text = "You have " + PlayerController.pl.Inventory.InventoryList.Count(x => x.BaseItemID == 8 && x != null) + " Bottles and " + PlayerController.pl.MoneyAmount.ToString("0.##") + "  $$$";
        shopWindow.SetActive(Active);
    }

    public void LiqourStoreWindow(bool Active)
    {
        liqourStoreText.text = "You have " + PlayerController.pl.MoneyAmount.ToString("0.##") + " $$$";
        liqourStoreWindow.SetActive(Active);
    }

    public IEnumerator PickupIndicator(List<BaseItem> items)
    {
        if (CRisRunning) yield break;

        if (items != null)
        {
            PickupObject.SetActive(true);
            CRisRunning = true;
            List<KeyValuePair<BaseItem, int>> temp = items.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count()).ToList();

            foreach (KeyValuePair<BaseItem, int> item in temp)
            {
                PickupObject.GetComponentInChildren<Image>().sprite = item.Key.ObjectSprite;
                PickupObject.GetComponentInChildren<Text>().text    = "+ " + item.Value.ToString();
                yield return new WaitForSeconds(0.5f);
                PickupObject.transform.GetChild(0).GetComponent<RectTransform>().anchorMin = minAnchor;
                PickupObject.transform.GetChild(0).GetComponent<RectTransform>().anchorMax = maxAnchor;
            }

            CRisRunning = false;
            PickupObject.SetActive(false);
        }
        else
        {
            PickupObject.GetComponentInChildren<Text>().text   = "No Items Found";
            PickupObject.GetComponentInChildren<Image>().color = new Color32(0, 0, 0, 0);
            PickupObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            PickupObject.SetActive(false);
            PickupObject.GetComponentInChildren<Image>().color = new Color32(255, 255, 255, 255);

        }
    }

    public void NoItemIndicator()
    {
        StartCoroutine(PickupIndicator(null));
    }

    private void AnchorChanger(bool active)
    {
        if(active)
        {
            recT.anchorMax += Offset * Time.deltaTime;
            recT.anchorMin += Offset * Time.deltaTime;
        }

    }

    public void Eprompt(bool active)
    {
        prompt.SetActive(active);
    }

    private IEnumerator EpromptSpriteChanger()
    {
        eCoroutineRunning = true;
        prompt.GetComponentInChildren<Image>().sprite = EpromptSprites[0];
        yield return new WaitForSeconds(1.0f);
        prompt.GetComponentInChildren<Image>().sprite = EpromptSprites[1];
        eCoroutineRunning = false;
    }

    private void Awake()
    {
        moneyText         = transform.GetChild(0).transform.GetChild(2).GetComponent<Text>();
        shopWindow        = transform.GetChild(4).gameObject;
        ReturnBottleText  = transform.GetChild(4).transform.GetChild(3).GetComponent<Text>();
        liqourStoreWindow = transform.GetChild(5).gameObject;
        liqourStoreText   = transform.GetChild(5).transform.GetChild(3).GetComponent<Text>();
        prompt            = transform.GetChild(6).transform.gameObject;
    }

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        for (int i = 0; i < transform.GetChild(1).childCount; i++)
        {
            inventoryObjects.Add(transform.GetChild(1).GetChild(i).transform);
        }

        recT      = PickupObject.transform.GetChild(0).GetComponent<RectTransform>();
        minAnchor = PickupObject.transform.GetChild(0).GetComponent<RectTransform>().anchorMin;
        maxAnchor = PickupObject.transform.GetChild(0).GetComponent<RectTransform>().anchorMax;
    }

    private void Update()
    {
        UIInput();
        DaytimeIndicator();
        StatusBarValueChanger();
        if(!eCoroutineRunning)
        {
            StartCoroutine(EpromptSpriteChanger());
        }
        AnchorChanger(CRisRunning);
        Inventory();
    }
}
