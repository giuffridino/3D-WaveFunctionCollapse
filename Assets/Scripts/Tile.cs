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


		////////////////////////////////////////////////////////////////////


 	public Tile[] front_upNeighbors;
    public Tile[] back_upNeighbors;
    public Tile[] right_upNeighbors;
    public Tile[] left_upNeighbors;
	public Tile[] front_downNeighbors;
    public Tile[] back_downNeighbors;
    public Tile[] right_downNeighbors;
    public Tile[] left_downNeighbors;

    [SerializeField] private Tile motherTile;


    public void UploadAndRotateNeighborhood()
    {
        UploadNeighborhood();
        RotateNeighborLists();
    }

   
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


		////////////////////////////////////////////////////////////////////


		front_upNeighbors = new Tile[motherTile.front_upNeighbors.Length];
        for (int i = 0; i < motherTile.front_upNeighbors.Length; i++)
        {
            front_upNeighbors[i] = motherTile.front_upNeighbors[i];
        }
    
        back_upNeighbors = new Tile[motherTile.back_upNeighbors.Length];
        for (int i = 0; i < motherTile.back_upNeighbors.Length; i++)
        {
            back_upNeighbors[i] = motherTile.back_upNeighbors[i];
        }
    
        right_upNeighbors = new Tile[motherTile.right_upNeighbors.Length];
        for (int i = 0; i < motherTile.right_upNeighbors.Length; i++)
        {
            right_upNeighbors[i] = motherTile.right_upNeighbors[i];
        }
    
        left_upNeighbors = new Tile[motherTile.left_upNeighbors.Length];
        for (int i = 0; i < motherTile.left_upNeighbors.Length; i++)
        {
            left_upNeighbors[i] = motherTile.left_upNeighbors[i];
        }

		front_downNeighbors = new Tile[motherTile.front_downNeighbors.Length];
        for (int i = 0; i < motherTile.front_downNeighbors.Length; i++)
        {
            front_downNeighbors[i] = motherTile.front_downNeighbors[i];
        }
    
        back_downNeighbors = new Tile[motherTile.back_downNeighbors.Length];
        for (int i = 0; i < motherTile.back_downNeighbors.Length; i++)
        {
            back_downNeighbors[i] = motherTile.back_downNeighbors[i];
        }
    
        right_downNeighbors = new Tile[motherTile.right_downNeighbors.Length];
        for (int i = 0; i < motherTile.right_downNeighbors.Length; i++)
        {
            right_downNeighbors[i] = motherTile.right_downNeighbors[i];
        }
    
        left_downNeighbors = new Tile[motherTile.left_downNeighbors.Length];
        for (int i = 0; i < motherTile.left_downNeighbors.Length; i++)
        {
            left_downNeighbors[i] = motherTile.left_downNeighbors[i];
        }

    }

    public void RotateNeighborLists()
    {
        Tile[] tempFrontNeighbors = frontNeighbors;
        frontNeighbors = leftNeighbors;
        leftNeighbors = backNeighbors;
        backNeighbors = rightNeighbors;
        rightNeighbors = tempFrontNeighbors;

        RotateNeighbors(frontNeighbors);
        RotateNeighbors(backNeighbors);
        RotateNeighbors(rightNeighbors);
        RotateNeighbors(leftNeighbors);


		////////////////////////////////////////////////////////////////////


		Tile[] tempFront_upNeighbors = front_upNeighbors;
		front_upNeighbors = left_upNeighbors;
        left_upNeighbors = back_upNeighbors;
        back_upNeighbors = right_upNeighbors;
        right_upNeighbors = tempFront_upNeighbors;

        RotateNeighbors(front_upNeighbors);
        RotateNeighbors(back_upNeighbors);
        RotateNeighbors(right_upNeighbors);
        RotateNeighbors(left_upNeighbors);

		Tile[] tempFront_downNeighbors = front_downNeighbors;
		front_downNeighbors = left_downNeighbors;
        left_downNeighbors = back_downNeighbors;
        back_downNeighbors = right_downNeighbors;
        right_downNeighbors = tempFront_downNeighbors;

        RotateNeighbors(front_downNeighbors);
        RotateNeighbors(back_downNeighbors);
        RotateNeighbors(right_downNeighbors);
        RotateNeighbors(left_downNeighbors);
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