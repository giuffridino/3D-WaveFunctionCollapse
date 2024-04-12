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

    public int rotationAngle;
    public void RotateTile()
    {
        //transform.Rotate(0, 90, 0);

        GameObject rotatedTile = Instantiate(gameObject);

        rotatedTile.transform.Rotate(0, rotationAngle, 0);

        string path = AssetDatabase.GetAssetPath(gameObject);
		Debug.Log(path);
        string directory = System.IO.Path.GetDirectoryName(path);
        string newPrefabName = System.IO.Path.GetFileNameWithoutExtension(path) + $"{rotationAngle}.prefab";
        string newPrefabPath = System.IO.Path.Combine(directory, newPrefabName);
        Debug.Log(newPrefabName);
        RotateNeighborLists();
        PrefabUtility.SaveAsPrefabAsset(rotatedTile, newPrefabPath);

        DestroyImmediate(rotatedTile);
    }

    private void RotateNeighborLists()
    {
        Tile[] tempFrontNeighbors = frontNeighbors;
        frontNeighbors = leftNeighbors;
        leftNeighbors = backNeighbors;
        backNeighbors = rightNeighbors;
        rightNeighbors = tempFrontNeighbors;
    }
}
