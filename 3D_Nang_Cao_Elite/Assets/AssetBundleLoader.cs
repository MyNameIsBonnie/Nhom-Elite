using System.Collections;
using UnityEngine;

public class AssetBundleLoader : MonoBehaviour
{
    //vào prefabs của GameObject đó, trong cửa sổ Inpcetor phía dưới cùng "Asset Bundle" -> new cái name của prefabs đó rồi gán vào bundleName.
    string bundleName =  "items" ; // Đặt tên bundle đúng với Asset Bundle Name
    //gán tên gốc asset của prefab đó 
    string[] assetNames = { "potions", "enemies" }; //tên asset

    IEnumerator Start()
    {
        string path = Application.streamingAssetsPath + "/Bundles/" + bundleName;
        AssetBundle bundle = AssetBundle.LoadFromFile(path);

        if (bundle == null)
        {
            Debug.LogError("Failed to load AssetBundle!");
            yield break;
        }

        foreach (string assetName in assetNames)
        {
            GameObject prefab = bundle.LoadAsset<GameObject>(assetName);
            Instantiate(prefab);
        }

        bundle.Unload(false);
    }
}

