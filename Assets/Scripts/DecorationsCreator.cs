using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class DecorationsCreator : MonoBehaviour
{
    private readonly Random _rand = new Random();
    [SerializeField] private GameObject railing;
    [SerializeField] private GameObject stairsRailing;
    
    public void AddDecorations(int dimX, int dimY, int dimZ, Cell[] gridComponents, Vector3 startingCell)
    {
        GameObject railings = new GameObject("Railings");
        GameObject stairsRailings = new GameObject("Stairs Railings");
        
        for (int x = 0; x < dimX; x++)
        {
            for (int y = 0; y < dimY; y++)
            {
                for (int z = 0; z < dimZ; z++)
                {
                    GameObject instantiatedTile;
                    
                    if (!((int)startingCell.x == x && (int)startingCell.y == y && (int)startingCell.z == z))
                    {
                        if (z == 0)
                        {
                            instantiatedTile = Instantiate(railing,
                                new Vector3((float)x - 0.45f, (float)y - 0.5f, -0.45f),
                                Quaternion.identity);
                            instantiatedTile.transform.parent = railings.transform;
                        }

                        if (z == dimZ - 1)
                        {
                            instantiatedTile = Instantiate(railing,
                                new Vector3((float)x - 0.45f, (float)y - 0.5f, z + 0.45f),
                                Quaternion.identity);
                            instantiatedTile.transform.parent = railings.transform;
                        }

                        if (x == 0)
                        {
                            instantiatedTile = Instantiate(railing,
                                new Vector3(-0.45f, (float)y - 0.5f, (float)z + 0.45f),
                                Quaternion.Euler(0f, 90f, 0f));
                            instantiatedTile.transform.parent = railings.transform;
                        }

                        if (x == dimX - 1)
                        {
                            instantiatedTile = Instantiate(railing,
                                new Vector3(x + 0.45f, (float)y - 0.5f, (float)z + 0.45f),
                                Quaternion.Euler(0f, 90f, 0f));
                            instantiatedTile.transform.parent = railings.transform;
                        }
                    }

                    int index = z + y * dimZ + x * dimY * dimZ;
                    Cell cell = gridComponents[index];

                    if (cell.tileOptions[0].name.Contains("Stairs"))
                    {
                        if (cell.tileOptions[0].name.Contains("90"))
                        {
                            instantiatedTile = Instantiate(stairsRailing,
                                new Vector3(x + 0.5f, (float)y + 0.5f, (float)z + 0.5f),
                                Quaternion.Euler(0f, 90f, 0f));
                            instantiatedTile.transform.parent = stairsRailings.transform;
                            instantiatedTile = Instantiate(stairsRailing,
                                new Vector3(x + 0.5f, (float)y + 0.5f, (float)z + 0.5f),
                                Quaternion.Euler(0f, 180f, 0f));
                            instantiatedTile.transform.parent = stairsRailings.transform;
                            instantiatedTile = Instantiate(stairsRailing,
                                new Vector3(x + 0.5f, (float)y + 0.5f, (float)z - 0.4f),
                                Quaternion.Euler(0f, 180f, 0f));
                            instantiatedTile.transform.parent = stairsRailings.transform;
                        }
                        else if (cell.tileOptions[0].name.Contains("270"))
                        {
                            instantiatedTile = Instantiate(stairsRailing,
                                new Vector3(x - 0.5f, (float)y + 0.5f, (float)z + 0.5f),
                                Quaternion.Euler(0f, 90f, 0f));
                            instantiatedTile.transform.parent = stairsRailings.transform;
                            instantiatedTile = Instantiate(stairsRailing,
                                new Vector3(x + 0.5f, (float)y + 0.5f, (float)z + 0.5f),
                                Quaternion.Euler(0f, 180f, 0f));
                            instantiatedTile.transform.parent = stairsRailings.transform;
                            instantiatedTile = Instantiate(stairsRailing,
                                new Vector3(x + 0.5f, (float)y + 0.5f, (float)z - 0.4f),
                                Quaternion.Euler(0f, 180f, 0f));
                            instantiatedTile.transform.parent = stairsRailings.transform;
                        }
                        else if (cell.tileOptions[0].name.Contains("180"))
                        {
                            instantiatedTile = Instantiate(stairsRailing,
                                new Vector3(x - 0.5f, (float)y + 0.5f, (float)z + 0.5f),
                                Quaternion.Euler(0f, 90f, 0f));
                            instantiatedTile.transform.parent = stairsRailings.transform;
                            instantiatedTile = Instantiate(stairsRailing,
                                new Vector3(x + 0.5f, (float)y + 0.5f, (float)z + 0.5f),
                                Quaternion.Euler(0f, 90f, 0f));
                            instantiatedTile.transform.parent = stairsRailings.transform;
                            instantiatedTile = Instantiate(stairsRailing,
                                new Vector3(x + 0.5f, (float)y + 0.5f, (float)z - 0.4f),
                                Quaternion.Euler(0f, 180f, 0f));
                            instantiatedTile.transform.parent = stairsRailings.transform;
                        }
                        else
                        {
                            instantiatedTile = Instantiate(stairsRailing,
                                new Vector3(x - 0.5f, (float)y + 0.5f, (float)z + 0.5f),
                                Quaternion.Euler(0f, 90f, 0f));
                            instantiatedTile.transform.parent = stairsRailings.transform;
                            instantiatedTile = Instantiate(stairsRailing,
                                new Vector3(x + 0.5f, (float)y + 0.5f, (float)z + 0.5f),
                                Quaternion.Euler(0f, 90f, 0f));
                            instantiatedTile.transform.parent = stairsRailings.transform;
                            instantiatedTile = Instantiate(stairsRailing,
                                new Vector3(x + 0.5f, (float)y + 0.5f, (float)z + 0.5f),
                                Quaternion.Euler(0f, 180f, 0f));
                            instantiatedTile.transform.parent = stairsRailings.transform;
                        }
                    }
                    else if (cell.tileOptions[0].name.Contains("Wall"))
                    {
                        if (_rand.Next(0, 10) == 1)
                            cell.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(true);
                    }
                    else if (cell.tileOptions[0].name.Contains("Corner"))
                    {
                        int spawnObject = _rand.Next(0, 6);
                        if (spawnObject != 0)
                        {
                            cell.transform.GetChild(0).transform.GetChild(spawnObject).gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
    }
}
