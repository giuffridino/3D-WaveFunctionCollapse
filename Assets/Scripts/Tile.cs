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

    [SerializeField] public Tile[] motherTile;

    // public void RotateTile()
    // {
    //     GameObject rotatedTile = Instantiate(gameObject);

    //     rotatedTile.transform.Rotate(0, rotationAngle, 0);

    //     string path = AssetDatabase.GetAssetPath(gameObject);
	// 	Debug.Log(path);
    //     string directory = System.IO.Path.GetDirectoryName(path);
    //     string newPrefabName = GenerateNewPrefabName(System.IO.Path.GetFileNameWithoutExtension(path)) + ".prefab";
    //     // newPrefabName = System.IO.Path.GetFileNameWithoutExtension(path) + $"{rotationAngle}.prefab";
    //     string newPrefabPath = System.IO.Path.Combine(directory, newPrefabName);
    //     Debug.Log("newPrefabName: " + newPrefabName);
    //     Tile newTile = rotatedTile.GetComponent<Tile>();
    //     RotateNeighborLists(newTile);
    //     PrefabUtility.SaveAsPrefabAsset(rotatedTile, newPrefabPath);

    //     DestroyImmediate(rotatedTile);
    // }

    public void UploadNeighborhood(Tile motherTile)
    {
        frontNeighbors = motherTile.frontNeighbors;
        backNeighbors = motherTile.backNeighbors;
        rightNeighbors = motherTile.rightNeighbors;
        leftNeighbors = motherTile.leftNeighbors;
        upNeighbors = motherTile.upNeighbors;
        downNeighbors = motherTile.downNeighbors;
    }

    public void RotateNeighborLists(Tile newTile)
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
