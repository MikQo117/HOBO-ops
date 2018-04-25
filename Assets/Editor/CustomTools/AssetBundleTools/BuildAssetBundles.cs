﻿using System.IO;

using UnityEngine;
using UnityEditor;

// Made by Sipi Raussi 25.4.2018
public class BuildAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string path = Application.dataPath + "/AssetBundles";
        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
    }
}
