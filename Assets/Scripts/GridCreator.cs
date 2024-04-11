using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridCreator : MonoBehaviour
{
    [SerializeField]
    private GameObject basePrefab; // Assign the cube prefab in the Unity Editor
    [SerializeField]
    private int gridSizeX = 5; // Number of cubes along X axis
    [SerializeField]
    private int gridSizeY = 5; // Number of cubes along Y axis
    [SerializeField]
    private int gridSizeZ = 5; // Number of cubes along Z axis
    [SerializeField]
    private float spacing = 1f; // Spacing between cubes

    public GameObject[,,] cubesArray; // 3D array to store references to cubes

    void Start()
    {
        GenerateCubeGrid(); // Call this method automatically when the script starts
    }

    void OnValidate()
    {
        if (gridSizeX < 1)
            gridSizeX = 1;
        if (gridSizeY < 1)
            gridSizeY = 1;
        if (gridSizeZ < 1)
            gridSizeZ = 1;
    }

    public GameObject[,,] GenerateCubeGrid()
    {
        if (cubesArray != null)
        {
            foreach (var cube in cubesArray)
            {
                DestroyImmediate(cube);
            }
        }

        cubesArray = new GameObject[gridSizeX, gridSizeY, gridSizeZ]; // Initialize the array

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int z = 0; z < gridSizeZ; z++)
                {
                    Vector3 spawnPosition = new Vector3(x * spacing, y * spacing, z * spacing);
                    Quaternion spawnRotation = basePrefab.transform.rotation;
                    GameObject cube = Instantiate(basePrefab, spawnPosition, spawnRotation);
                    cube.transform.parent = transform;
                    cubesArray[x, y, z] = cube; // Store reference to cube in the array
                }
            }
        }

        return cubesArray;
    }

    public void ChangeGridCubePrefab(int x, int y, int z, GameObject prefab)
    {
        if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY && z >= 0 && z < gridSizeZ)
        {
            if (cubesArray == null)
            {
                Debug.LogError("Cubes not defined");
            }
            Vector3 position = cubesArray[x, y, z].transform.position;
            DestroyImmediate(cubesArray[x, y, z]);
            GameObject newPrefab = Instantiate(prefab, position, Quaternion.identity);
            newPrefab.transform.parent = transform;
            cubesArray[x, y, z] = newPrefab;
            
        }
        else
        {
            Debug.LogError("Invalid coordinates");
        }
    }

    public void DeleteGrid()
    {
        int childCount = transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
        cubesArray = null;

        // if (cubesArray != null)
        // {
        //     foreach (GameObject cube in cubesArray)
        //     {
        //         DestroyImmediate(cube);
        //     }
        //     cubesArray = null; // Reset the cubesArray
        // }
    }
}
