using System.Collections.Generic;

using UnityEngine;

// Made by Sipi Raussi 25.4.2018
public class SettingsManager : MonoBehaviour
{
    private static SettingsManager _instance;
    public static SettingsManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<SettingsManager>();

                if(_instance == null)
                {
                    GameObject temp = new GameObject("Settings");
                    _instance = temp.AddComponent<SettingsManager>();
                }
            }

            return _instance;
        }
    }

    public Settings Settings;

    private void FindSettings()
    {
        List<AssetBundle> abl = AssetManager.Instance.AssetBundlesList;

        AssetBundle ab = AssetManager.Instance.AssetBundlesList.Find(AB => AB.Contains("settings"));

        for (int i = 0; i < abl.Count; i++)
        {
            if(abl[i].name == "settings")
            {
                ab = abl[i];
                break;
            }
        }
    
        Settings = ab.LoadAsset<Settings>("Settings.asset");
    }

    private void Awake()
    {
        if(Settings == null)
        {
            FindSettings();
        }
    }
}
