using UnityEditor;
using UnityEngine;
using System;

public class AssetBundle 
{

    [MenuItem("Assets/Build Asset Bundles")]
    public static void BuildAllAssetBundles()
    {
        string bundleDirectory = "Assets/StreamingAssets/Bundles";// duong dan noi luu asset bundle
        if (!System.IO.Directory.Exists(bundleDirectory))
        {
            System.IO.Directory.CreateDirectory(bundleDirectory);
        }

        try
        {
            // set bundle name and build
            BuildPipeline.BuildAssetBundles(bundleDirectory, BuildAssetBundleOptions.None, 
                EditorUserBuildSettings.activeBuildTarget);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        
    }
}
