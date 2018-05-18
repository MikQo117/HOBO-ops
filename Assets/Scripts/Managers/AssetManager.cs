using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Made by Sipi Raussi 25.4.2018
public class AssetManager : MonoBehaviour
{
    private static AssetManager _instance;
    public static AssetManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AssetManager>();

                if (_instance == null)
                {
                    GameObject temp = new GameObject("AssetManager");
                    _instance = temp.AddComponent<AssetManager>();
                }
            }

            return _instance;
        }
    }

    private string path;

    public List<AssetBundle> AssetBundlesList = new List<AssetBundle>();

    private void Awake()
    {
        path = Application.dataPath + "/AssetBundles";

        string[] files = Directory.GetFiles(path);

        for (int i = 0; i < files.Length; i++)
        {
            if (Path.GetExtension(files[i]) == "")
            {
                AssetBundlesList.Add(AssetBundle.LoadFromFile(files[i]));
            }
        }
    }

}
