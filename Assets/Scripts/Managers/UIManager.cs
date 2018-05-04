using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    //StatusBars variables
    private const float maxValue = 100;
    public Image HealthBar;
    public Image SanityBar;
    public Image StaminaBar;
    public Text BottleText;

    //Inventory variables
    private List<Transform> inventoryObjects = new List<Transform>();
    private bool showing = false;
    private Text moneyText;

    //Daytime indicator variables
    float arrowrotationChanger = 0.0f;
    public GameObject Arrow;

    //ShopWindow Variables
    private GameObject shopWindow;
    private GameObject liqourStoreWindow;
    private Text liqourStoreText;
    private Text ReturnBottleText;

    //Pick up Variables
    private Vector3 originalPoint;
    public GameObject PickupObject;
    private float timer;
    public bool CRisRunning = false;

    private void StatusBarValueChanger()
    {
        HealthBar.fillAmount = Mathf.Clamp01(PlayerController.pl.Health / maxValue);
        SanityBar.fillAmount = Mathf.Clamp01(PlayerController.pl.Sanity / maxValue);
        StaminaBar.fillAmount = Mathf.Clamp01(PlayerController.pl.Stamina / maxValue);
    }

    private void Inventory()
    {
        if (!shopWindow.activeInHierarchy || !liqourStoreWindow.activeInHierarchy)
        {
            for (int i = 0; i < transform.childCount - 5; i++)
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
        arrowrotationChanger = GameManager.Instance.DayTimer * 0.79f;
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

        PickupObject.SetActive(true);
        CRisRunning = true;
        var temp = items.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count()).ToList();

        foreach (var item in temp)
        {
            PickupObject.GetComponentInChildren<Image>().sprite = item.Key.ObjectSprite;
            PickupObject.GetComponentInChildren<Text>().text = "+ " + item.Value.ToString();
            yield return new WaitForSeconds(0.5f);
            PickupObject.transform.position = originalPoint; 
        }
        
        CRisRunning = false;
        PickupObject.SetActive(false);
    }

    private void Awake()
    {
        shopWindow = transform.GetChild(4).gameObject;
        liqourStoreWindow = transform.GetChild(5).gameObject;
        ReturnBottleText = transform.GetChild(4).transform.GetChild(3).GetComponent<Text>();
        moneyText = transform.GetChild(0).transform.GetChild(2).GetComponent<Text>();
        liqourStoreText = transform.GetChild(5).transform.GetChild(3).GetComponent<Text>();
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
        originalPoint = PickupObject.transform.position;
    }

    private void Update()
    {
        UIInput();
        DaytimeIndicator();
        StatusBarValueChanger();
        if(CRisRunning)
        {
            PickupObject.transform.position += new Vector3(0, 10) * Time.deltaTime;
        }
        timer -= Time.deltaTime;
        Inventory();
    }
}
