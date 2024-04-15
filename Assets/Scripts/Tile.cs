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

    [SerializeField] private Tile motherTile;
    
    public void UploadNeighborhood()
    {
        frontNeighbors = new Tile[motherTile.frontNeighbors.Length];
        for (int i = 0; i < motherTile.frontNeighbors.Length; i++)
        {
            frontNeighbors[i] = motherTile.frontNeighbors[i];
        }
    
        backNeighbors = new Tile[motherTile.backNeighbors.Length];
        for (int i = 0; i < motherTile.backNeighbors.Length; i++)
        {
            backNeighbors[i] = motherTile.backNeighbors[i];
        }
    
        rightNeighbors = new Tile[motherTile.rightNeighbors.Length];
        for (int i = 0; i < motherTile.rightNeighbors.Length; i++)
        {
            rightNeighbors[i] = motherTile.rightNeighbors[i];
        }
    
        leftNeighbors = new Tile[motherTile.leftNeighbors.Length];
        for (int i = 0; i < motherTile.leftNeighbors.Length; i++)
        {
            leftNeighbors[i] = motherTile.leftNeighbors[i];
        }
    
        upNeighbors = new Tile[motherTile.upNeighbors.Length];
        for (int i = 0; i < motherTile.upNeighbors.Length; i++)
        {
            upNeighbors[i] = motherTile.upNeighbors[i];
        }
    
        downNeighbors = new Tile[motherTile.downNeighbors.Length];
        for (int i = 0; i < motherTile.downNeighbors.Length; i++)
        {
            downNeighbors[i] = motherTile.downNeighbors[i];
        }
    }

    public void RotateNeighborLists(Tile newTile)
    {
        Tile[] tempFrontNeighbors = newTile.frontNeighbors;
        newTile.frontNeighbors = newTile.leftNeighbors;
        newTile.leftNeighbors = newTile.backNeighbors;
        newTile.backNeighbors = newTile.rightNeighbors;
        newTile.rightNeighbors = tempFrontNeighbors;

        RotateNeighbors(newTile.frontNeighbors);
        RotateNeighbors(newTile.backNeighbors);
        RotateNeighbors(newTile.rightNeighbors);
        RotateNeighbors(newTile.leftNeighbors);
    }

    void RotateNeighbors(Tile[] neighbors)
    {
        for (int i = 0; i < neighbors.Length; i++)
        {
            string tileName = neighbors[i].name;
            string floorCheck = "Floor";
            string emptyCheck = "Empty";
            string corridorCheck = "Corridor";
            if (!tileName.Contains(floorCheck) && !tileName.Contains(emptyCheck))
            {
                string lastChars = tileName.Substring(tileName.Length - 3);
                if (lastChars == "p90")
                {
                    lastChars = tileName.Contains(corridorCheck) ? "" : "180";
                    tileName = tileName.Substring(0, tileName.Length - 2);
                }
                else if (lastChars == "180")
                {
                    lastChars = "270";
                    tileName = tileName.Substring(0, tileName.Length - 3);
                }
                else if (lastChars == "270")
                {
                    lastChars = "";
                    tileName = tileName.Substring(0, tileName.Length - 3);
                }
                else
                {
                    lastChars = "90";
                }
                string path = AssetDatabase.GetAssetPath(gameObject);
                string directory = System.IO.Path.GetDirectoryName(path);
                Debug.Log(path + " " + directory);
                tileName = tileName + lastChars + ".prefab";
                string newPrefabPath = System.IO.Path.Combine(directory, tileName);
                Debug.Log(newPrefabPath);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(newPrefabPath);
                if (prefab != null)
                {
                    neighbors[i] = prefab.GetComponent<Tile>();
                }
                else
                {
                    Debug.LogError("Prefab not found: " + tileName);
                }
            }
        }
    }
}