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
    private float       alphaValue = 0f;
    private bool        increase;
    public Image HealthBar;
    public Image SanityBar;
    public Image StaminaBar;
    public Text BottleText;

    //Inventory variables
    private List<Transform> inventoryObjects = new List<Transform>();
    private Text moneyText;

    //Daytime indicator variables
    float              arrowrotationChanger = 0.0f;
    public  GameObject Arrow;
    private GameObject dayCounter;
    public  GameObject ColorTint;

    //ShopWindow Variables
    private GameObject shopWindow;
    private GameObject liqourStoreWindow;
    private Text liqourStoreText;
    private Text ReturnBottleText;

    //Pick up Variables
    public GameObject PickupObject;
    public bool CRisRunning = false;
    private Vector2 minAnchor;
    private Vector2 maxAnchor;
    private RectTransform recT;
    private Vector2 Offset = new Vector2(0, 0.1f);

    //E-prompt
    public GameObject prompt;
    public Sprite[] EpromptSprites;
    private bool eCoroutineRunning;
    private Vector3 ePromptOffset = new Vector3(0.5f, 0.5f, -1);

    //Sleeping System
    private GameObject sleepWindow;
    private InputField sleepField;
    private int sleepHours;
    public GameObject SleepText;
    private bool sCoroutineIsRunning;
    private GameObject fadeWindow;
    private float sleepTimer;

    //Death Screen Variables
    private GameObject deathScreen;

    //Pausemenu Variables
    public GameObject Pausemenu;

    //get & set
    public GameObject ShopWindow1
    {
        get
        {
            return shopWindow;
        }

        set
        {
            shopWindow = value;
        }
    }

    public GameObject LiqourStoreWindow1
    {
        get
        {
            return liqourStoreWindow;
        }

        set
        {
            liqourStoreWindow = value;
        }
    }

    public int SleepHours
    {
        get
        {
            return sleepHours;
        }

        set
        {
            sleepHours = value;
        }
    }

    public float SleepTimer
    {
        get
        {
            return sleepTimer;
        }

        set
        {

            sleepTimer = Mathf.Clamp(value, 0, 4);

        }
    }



    //Inventory methods
    public void Inventory(bool showing)
    {
        if (!ShopWindow1.activeInHierarchy && !LiqourStoreWindow1.activeInHierarchy)
        {
            for (int i = 0; i < transform.childCount - 8; i++)
            {
                transform.GetChild(i).gameObject.SetActive(showing);
            }
            BottleText.text = PlayerController.pl.Inventory.InventoryList.Count(x => x.BaseItemID == 8).ToString();
            moneyText.text = string.Format("{0:C2}", PlayerController.pl.MoneyAmount);
            for (int i = 0; i < inventoryObjects.Count; i++)
            {
                inventoryObjects[i].GetComponentInChildren<Text>().text = PlayerController.pl.Inventory.InventoryList.Count(x => x.BaseItemID == i && x != null).ToString();
            }
        }
    }

    //Pick Up methods
    public IEnumerator PickupIndicator(List<BaseItem> items, bool shop)
    {
        if (CRisRunning) yield break;

        if (items != null)
        {
            if (items.Count > 0)
            {
                if (!shop)  //Item drop display
                {
                    PickupObject.SetActive(true);
                    CRisRunning = true;
                    List<KeyValuePair<BaseItem, int>> temp = items.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count()).ToList();

                    foreach (KeyValuePair<BaseItem, int> item in temp)
                    {
                        PickupObject.GetComponentInChildren<Image>().sprite = item.Key.ObjectSprite;
                        PickupObject.GetComponentInChildren<Text>().text = "+ " + item.Value.ToString();
                        yield return new WaitForSeconds(1.0f);
                        PickupObject.transform.GetChild(0).GetComponent<RectTransform>().anchorMin = minAnchor;
                        PickupObject.transform.GetChild(0).GetComponent<RectTransform>().anchorMax = maxAnchor;
                    }
                }
                else // used to display the money amount player is given when he/she return bottles
                {
                    PickupObject.SetActive(true);
                    PickupObject.GetComponentInChildren<Image>().sprite = items.Find(x => x.BaseItemID == 8).ObjectSprite;
                    float value = (items.Count * 0.100f);
                    PickupObject.GetComponentInChildren<Text>().text = "+   " + string.Format("{0:C2}", value);
                    yield return new WaitForSeconds(1.0f);
                    PickupObject.transform.GetChild(0).GetComponent<RectTransform>().anchorMin = minAnchor;
                    PickupObject.transform.GetChild(0).GetComponent<RectTransform>().anchorMax = maxAnchor;
                }
                CRisRunning = false;
                PickupObject.SetActive(false);
            }
        }
        else
        {
            PickupObject.GetComponentInChildren<Text>().text = "No Items Found";
            PickupObject.GetComponentInChildren<Image>().color = new Color32(0, 0, 0, 0);
            PickupObject.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            PickupObject.SetActive(false);
            PickupObject.GetComponentInChildren<Image>().color = new Color32(255, 255, 255, 255);
        }
    }

    public void NoItemIndicator()
    {
        StartCoroutine(PickupIndicator(null, false));
    }

    private void AnchorChanger(bool active)
    {
        if (active)
        {
            recT.anchorMax += Offset * Time.deltaTime;
            recT.anchorMin += Offset * Time.deltaTime;
        }

    }

    //StatusBar methods

    private void StatusBarValueChanger()
    {
        HealthBar.fillAmount = Mathf.Clamp01(PlayerController.pl.Health / maxValue);
        SanityBar.fillAmount = Mathf.Clamp01(PlayerController.pl.Sanity / maxValue);
        StaminaBar.fillAmount = Mathf.Clamp01(PlayerController.pl.Stamina / maxValue);
    }

    public void StatusBarLowIndicator(int StatusID)
    {
        return;
        if(StatusID == 1)
        {
            Color HealthAlpha = HealthBar.color;
            HealthAlpha.a = alphaValue;
            HealthBar.color = HealthAlpha;
        }
        else if(StatusID == 2)
        {
            Color SanityAlpha = SanityBar.color;
            SanityAlpha.a = alphaValue;
            SanityBar.color = SanityAlpha;
        }
        else if(StatusID == 3)
        {
            Color StaminaAlpha = StaminaBar.color;
            StaminaAlpha.a = alphaValue;
            StaminaBar.color = StaminaAlpha;
        }
        
    }

    private void AlphaValueChanger()
    {
        if (sleepTimer > 0)
        {
            if (alphaValue <= 0)
            {
                increase = true;
            }

            if (alphaValue >= 1)
            {
                increase = false;
            }

            if (increase)
            {
                alphaValue += Time.deltaTime;
            }
            else
            {
                alphaValue -= Time.deltaTime;
            }
        }
        else
        {
            alphaValue = 0;
        }
    }

    //Sleeping methods
    public void SleepWindow(bool active)
    {
        sleepWindow.SetActive(active);
        int temp;
        sleepField.Select();
        if (PlayerController.pl.Submit)
        {
            if (int.TryParse(sleepField.text, out temp) && sleepField.text != null && active)
            {
                if (temp > 0 && temp < 9)
                {
                    SleepHours = int.Parse(sleepField.text);
                    PlayerController.pl.Sleep(SleepHours);
                }
            }
        }

        if (!active)
        {
            sleepField.text = null;
        }
    }

    public void FadetoBlack(bool active)
    {
        fadeWindow.SetActive(active);
        SleepTimer = 3.5f;
    }

    private void FadeToBlackChanger()
    {
        Color temp = fadeWindow.GetComponent<Image>().color;
        if (SleepTimer > 0)
        {
            temp = new Color(0f, 0f, 0f);
            sleepTimer -= Time.deltaTime;
            temp.a = alphaValue;
            fadeWindow.GetComponent<Image>().color = temp;
        }
        else
        {
            temp.a = alphaValue;
            fadeWindow.GetComponent<Image>().color = temp;
        }
    }

    public IEnumerator SleepTextActivator()
    {
        if (sCoroutineIsRunning) yield break;
        sCoroutineIsRunning = true;
        SleepText.GetComponent<Text>().text = "You can't sleep now." + System.Environment.NewLine + "Time remaining: " + (int)PlayerController.pl.SleepTimer;
        SleepText.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        SleepText.SetActive(false);
        sCoroutineIsRunning = false;
    }

    //Shop methods
    public void LiqourStoreWindow(bool Active)
    {
        liqourStoreText.text = string.Format("{0:C2}", PlayerController.pl.MoneyAmount);
        LiqourStoreWindow1.SetActive(Active);
    }

    public void ShopWindow(bool Active)
    {
        ReturnBottleText.text = "      " + PlayerController.pl.Inventory.InventoryList.Count(x => x.BaseItemID == 8 && x != null) + System.Environment.NewLine
                              + string.Format("{0:C2}", PlayerController.pl.MoneyAmount);
        ShopWindow1.SetActive(Active);
    }

    //Eprompt methods
    private IEnumerator EpromptSpriteChanger()
    {
        if (eCoroutineRunning) yield break;
        eCoroutineRunning = true;
        prompt.GetComponentInChildren<SpriteRenderer>().sprite = EpromptSprites[0];
        yield return new WaitForSeconds(1.0f);
        prompt.GetComponentInChildren<SpriteRenderer>().sprite = EpromptSprites[1];
        yield return new WaitForSeconds(1.0f);
        eCoroutineRunning = false;
    }

    private void TransformChanger()
    {
        if (PlayerController.pl.InteractableCollider != null)
        {
            prompt.transform.position = PlayerController.pl.InteractableCollider.bounds.max + ePromptOffset;
        }
    }

    public void Eprompt(bool active)
    {
        prompt.SetActive(active);
    }

    //Daytime methods
    private void DaytimeIndicator()
    {
        arrowrotationChanger = GameManager.Instance.DayTimer * 0.79f;
        Arrow.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, -arrowrotationChanger);
    }

    public void DayCounterUpdater(int day)
    {
        dayCounter.GetComponent<Text>().text = "Day " + day;
    }

    public void DaytimeColorTintChanger(int StatusID)
    {
        if (sleepTimer <= 0)
        {
            if (StatusID == 1) // morning
            {
                fadeWindow.GetComponent<Image>().color = new Color(0.99f, 1.0f, 0.855f, 0);

                //ColorTint.GetComponent<MeshRenderer>().material.SetColor(Shader.PropertyToID("_Color"), new Color(1.0f, 1.0f, 1.0f, 1.0f));
            }

            if (StatusID == 2) // rushHour
            {
                //253,255,212
                fadeWindow.GetComponent<Image>().color = new Color(0.99f, 1.0f, 0.855f, 0.05f);
                //ColorTint.GetComponent<MeshRenderer>().material.SetColor(Shader.PropertyToID("_Color"), new Color(0.99f,1.0f,0.855f,1));
            }

            if (StatusID == 3) // NightTime
            {
                fadeWindow.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.7f);

                //ColorTint.GetComponent<MeshRenderer>().material.SetColor(Shader.PropertyToID("_Color"), new Color(0.7f,0.7f,0.7f,1));
            }
        }
    }

    //Pausemenu methdos

    public void PausemenuActive(bool active)
    {
        this.gameObject.SetActive(!active);
        PauseMenu.Instance.transform.gameObject.SetActive(active);
        PauseMenu.Instance.PauseMenuActive();   
    }

    //Sets deathscreen to active
    public void DeathScreen()
    {
        deathScreen.SetActive(true);
    }

    //Unity methods
    private void Awake()
    {
        moneyText = transform.GetChild(0).transform.GetChild(2).GetComponent<Text>();
        fadeWindow = transform.GetChild(2).gameObject;
        dayCounter = transform.GetChild(4).GetChild(2).gameObject;
        ShopWindow1 = transform.GetChild(5).gameObject;
        ReturnBottleText = transform.GetChild(5).transform.GetChild(0).GetChild(3).GetComponent<Text>();
        LiqourStoreWindow1 = transform.GetChild(6).gameObject;
        liqourStoreText = transform.GetChild(6).GetChild(0).transform.GetChild(3).GetComponent<Text>();
        sleepWindow = transform.GetChild(8).transform.gameObject;
        sleepField = transform.GetChild(8).transform.GetChild(0).GetComponent<InputField>();
        deathScreen = transform.GetChild(9).gameObject;
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

        recT = PickupObject.transform.GetChild(0).GetComponent<RectTransform>();
        minAnchor = PickupObject.transform.GetChild(0).GetComponent<RectTransform>().anchorMin;
        maxAnchor = PickupObject.transform.GetChild(0).GetComponent<RectTransform>().anchorMax;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        TransformChanger();
        DaytimeIndicator();
        AlphaValueChanger();
        StatusBarValueChanger();
        SleepTimer -= Time.deltaTime;
        FadeToBlackChanger();
        if (!eCoroutineRunning)
        {
            StartCoroutine(EpromptSpriteChanger());
        }
        AnchorChanger(CRisRunning);
    }
}
