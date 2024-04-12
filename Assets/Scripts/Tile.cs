using UnityEngine;
using UnityEditor;

public class Tile : MonoBehaviour
{
    public Tile[] frontNeighbors;
    public Tile[] backNeighbors;
    public Tile[] rightNeighbors;
    public Tile[] leftNeighbors;
    public Tile[] upNeighbors;
    public Tile[] downNeighbors;

    // public int rotationAngle;
    public void RotateTile()
    {
        string newPrefabName = null;
        string path = AssetDatabase.GetAssetPath(gameObject);
        string directory = System.IO.Path.GetDirectoryName(path);
        int angle = 90;
        for (int i = 0; i < 3; i++)
        {
            GameObject rotatedTile = Instantiate(gameObject);
            rotatedTile.transform.Rotate(0, angle, 0);
            if (newPrefabName == null)
            {
                newPrefabName = GenerateNewPrefabName(System.IO.Path.GetFileNameWithoutExtension(path)) + ".prefab";
            }
            else
            {
                newPrefabName = newPrefabName.Replace(".prefab", "");
                newPrefabName = GenerateNewPrefabName(newPrefabName) + ".prefab";
            }
            // newPrefabName = System.IO.Path.GetFileNameWithoutExtension(path) + $"{rotationAngle}.prefab";
            string newPrefabPath = System.IO.Path.Combine(directory, newPrefabName);
            Debug.Log("newPrefabName: " + newPrefabName);
            Tile newTile = rotatedTile.GetComponent<Tile>();
            RotateNeighborLists(newTile);
            PrefabUtility.SaveAsPrefabAsset(rotatedTile, newPrefabPath);
    
            DestroyImmediate(rotatedTile);
            angle += 90;
        }
    }

    private void RotateNeighborLists(Tile newTile)
    {
        Tile[] tempFrontNeighbors = newTile.frontNeighbors;
        newTile.frontNeighbors = newTile.leftNeighbors;
        newTile.leftNeighbors = newTile.backNeighbors;
        newTile.backNeighbors = newTile.rightNeighbors;
        newTile.rightNeighbors = tempFrontNeighbors;
    }
    
    private string GenerateNewPrefabName(string originalName)
    {
        // Check if the last two or three characters are digits
        string numericPart = "";
        if (int.TryParse(originalName.Substring(originalName.Length - 3), out _))
        {
            numericPart = originalName.Substring(originalName.Length - 3);
        }
        else if (int.TryParse(originalName.Substring(originalName.Length - 2), out _))
        {
            numericPart = originalName.Substring(originalName.Length - 2);
        }
        if (!string.IsNullOrEmpty(numericPart))
        {
            int numericValue = int.Parse(numericPart);
            numericValue += 90;
            originalName = originalName.Substring(0, originalName.Length - numericPart.Length) + numericValue.ToString();
        }
        else
        {
            // If no numeric part, simply append "_90" to the original name
            originalName += "_90";
        }
        return originalName;
    }
}
