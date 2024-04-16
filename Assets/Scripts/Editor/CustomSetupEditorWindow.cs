using UnityEngine;
using UnityEditor;

public class CustomSetupEditorWindow : EditorWindow
{
    [MenuItem("Custom Tools/Rotate Constraints")]
    static void RotateConstraints()
    {
        string folderPath = "Assets/FinalModels";
        string[] assetGUIDs = AssetDatabase.FindAssets("", new[] { folderPath });
        
        foreach (string assetGuid in assetGUIDs)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(assetGuid);
            Debug.Log("Found asset: " + assetPath);
            
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            Tile prefabTile = prefab.GetComponent<Tile>();
            Debug.Log("Prefab name is: " + prefab.name);
            if (prefab.name.Contains("90") | prefab.name.Contains("180") | prefab.name.Contains("270"))
            {
                prefabTile.UploadAndRotateNeighborhood();
            }
            
        }
    }
}