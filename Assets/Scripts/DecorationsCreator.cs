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
    [SerializeField] private GameObject treasureChest;
    [SerializeField] private GameObject clockObj;

    public void AddDecorations(int dimX, int dimY, int dimZ, Cell[] gridComponents, Vector3 startingCell)
    {
        var decorations = GameObject.Find("Decorations").transform;
        GameObject railings = new GameObject("Railings");
        railings.transform.parent = decorations;
        GameObject stairsRailings = new GameObject("Stairs Railings");
        stairsRailings.transform.parent = decorations;

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
                    else if (cell.tileOptions[0].name.Contains("Corner"))
                    {
                        int spawnObject = _rand.Next(0, 6);
                        if (spawnObject != 0)
                        {
                            cell.transform.GetChild(0).transform.GetChild(spawnObject).gameObject.SetActive(true);
                            cell.hasObject = true;
                        }
                    }
                }
            }
        }

        AddTreasureChest(dimX, dimY, dimZ, gridComponents);
        AddClocks(dimX, dimY, dimZ, gridComponents);
    }

    private void AddTreasureChest(int dimX, int dimY, int dimZ, Cell[] gridComponents)
    {
        var spawnPoint = new Vector3(_rand.Next(0, dimX - 1), dimY - 1, _rand.Next(0, dimZ - 1));
        var treasure = Instantiate(treasureChest, spawnPoint + new Vector3(0, -0.22f, 0), Quaternion.identity);
        treasure.transform.parent = GameObject.Find("Decorations").transform;
        int maxIndex = dimX * dimY * dimZ;
        while (true)
        {
            var pos = treasure.transform.position + new Vector3(0, 0.22f, 0);
            int index = (int)pos.z + ((int)pos.y - 1) * dimZ + (int)pos.x * dimY * dimZ;
            
            if (pos.x < (dimX / 2) - 1)
            {
                treasure.transform.rotation = Quaternion.Euler(0, 270, 0);
            }
            else if (pos.x >= (dimX / 2))
            {
                treasure.transform.rotation = Quaternion.Euler(0, 90, 0);
            }

            if (pos.z <= 1)
            {
                treasure.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (pos.z >= dimZ - 2)
            {
                treasure.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            var forwardPos = pos - treasure.transform.forward;
            int diagForPos = (int)forwardPos.z + ((int)forwardPos.y - 1) * dimZ + (int)forwardPos.x * dimY * dimZ;
            var backPos = pos + treasure.transform.forward;
            int diagBackPos = (int)backPos.z + ((int)backPos.y - 1) * dimZ + (int)backPos.x * dimY * dimZ;
            var rightPos = pos - treasure.transform.right;
            int diagRightPos = (int)rightPos.z + ((int)rightPos.y - 1) * dimZ + (int)rightPos.x * dimY * dimZ;
            var leftPos = pos + treasure.transform.right;
            int diagLeftPos = (int)leftPos.z + ((int)leftPos.y - 1) * dimZ + (int)leftPos.x * dimY * dimZ;
            
            if (gridComponents[index].tileOptions[0].name.Contains("Stairs") ||
                (diagForPos >= 0 && diagForPos < maxIndex && gridComponents[diagForPos].tileOptions[0].name.Contains("Stairs")) ||
                (diagBackPos >= 0 && diagBackPos < maxIndex && gridComponents[diagBackPos].tileOptions[0].name.Contains("Stairs")) ||
                (diagRightPos >= 0 && diagRightPos < maxIndex && gridComponents[diagRightPos].tileOptions[0].name.Contains("Stairs")) ||
                (diagLeftPos >= 0 && diagLeftPos < maxIndex && gridComponents[diagLeftPos].tileOptions[0].name.Contains("Stairs")))
            {
                treasure.transform.position = new Vector3(_rand.Next(0, dimX - 1), dimY - 1 - 0.22f, _rand.Next(0, dimZ - 1));
            }
            else
            {
                break;
            }
        }
    }

    private void AddClocks(int dimX, int dimY, int dimZ, Cell[] gridComponents)
    {
        var decorations = GameObject.Find("Decorations").transform;
        GameObject clocks = new GameObject("Clocks");
        clocks.transform.parent = decorations;

        for (int x = 0; x < dimX; x++)
        {
            for (int y = 0; y < dimY; y++)
            {
                for (int z = 0; z < dimZ; z++)
                {
                    int index = z + y * dimZ + x * dimY * dimZ;
                    Cell cell = gridComponents[index];

                    if (!cell.tileOptions[0].name.Contains("Stairs") && !cell.tileOptions[0].name.Contains("Empty") &&
                        y < dimY - 1 && !cell.hasObject)
                    {
                        if (_rand.Next(0, 30) == 0)
                        {
                            var clockSpawn = new Vector3(x, y, z);
                            var clock = Instantiate(clockObj, clockSpawn + new Vector3(0, -0.25f, 0),
                                Quaternion.identity);
                            clock.transform.parent = clocks.transform;
                        }
                    }
                }
            }
        }
    }
}