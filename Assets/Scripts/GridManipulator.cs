using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManipulator : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private GridCreator gridCreator;

    public void ManipulateGrid()
    {
        if (gridCreator.cubesArray == null) // Check if cubesArray is null
        {
            gridCreator.GenerateCubeGrid(); // Call GenerateCubeGrid() to ensure cubesArray is initialized
        }
        for (int x = 0; x < gridCreator.cubesArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridCreator.cubesArray.GetLength(1); y++)
            {
                for (int z = 0; z < gridCreator.cubesArray.GetLength(2); z++)
                {
                    gridCreator.ChangeGridCubePrefab(x, y, z, prefab);
                }
            }
        }
    }
}
