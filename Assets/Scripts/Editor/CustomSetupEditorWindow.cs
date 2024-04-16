using UnityEngine;
using UnityEditor;

public class CustomSetupEditorWindow : EditorWindow
{
    [MenuItem("Custom Tools/Iterate Final Models")]
    static void IterateFinalModels()
    {
        string folderPath = "Assets/FinalModels";
        string[] assetGUIDs = AssetDatabase.FindAssets("", new[] { folderPath });
        
        foreach (string assetGuid in assetGUIDs)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);
            Debug.Log("Found asset: " + assetPath);
            
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            Tile prefabTile = prefab.GetComponent<Tile>();
            
            prefabTile.UploadAndRotateNeighborhood(prefabTile);
        }
    }
}