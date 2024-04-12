using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WFC : MonoBehaviour
{
    [SerializeField] private int dimX = 5;
    [SerializeField] private int dimY = 5;
    [SerializeField] private int dimZ = 5;

    [SerializeField] private Tile[] tileObjects;
    [SerializeField] private List<Cell> gridComponents;
    [SerializeField] private Cell cellObj;
    
    [SerializeField] private Tile backupTile;

    private int _iteration = 0;

    private void Awake()
    {
        gridComponents = new List<Cell>();
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        for (int x = 0; x < dimX; x++)
        {
            for (int y = 0; y < dimY; y++)
            {
                for (int z = 0; z < dimZ; z++)
                {
                    Cell newCell = Instantiate(cellObj, new Vector3(x, y, z), Quaternion.identity);
                    newCell.CreateCell(false, tileObjects);
                    gridComponents.Add(newCell);
                }
            }
        }
    }

    IEnumerator CheckEntropy()
    {
        List<Cell> tempGrid = new List<Cell>(gridComponents);
        tempGrid.RemoveAll(c => c.collapsed);
        tempGrid.Sort((a,b) => a.tileOptions.Length - b.tileOptions.Length);
        tempGrid.RemoveAll(a => a.tileOptions.Length != tempGrid[0].tileOptions.Length);
        
        yield return new WaitForSeconds(0.025f);

        CollapseCell(tempGrid);
    }

    void CollapseCell(List<Cell> tempGrid)
    {
        int randIndex = UnityEngine.Random.Range(0, tempGrid.Count);
        Cell cellToCollapse = tempGrid[randIndex];

        cellToCollapse.collapsed = true;
        try
        {
            Tile selectedTile = cellToCollapse.tileOptions[UnityEngine.Random.Range(0, cellToCollapse.tileOptions.Length)];
            cellToCollapse.tileOptions = new Tile[] { selectedTile };
        }
        catch
        {
            Tile selectedTile = backupTile;
            cellToCollapse.tileOptions = new Tile[] { selectedTile };
        }

        Tile foundTile = cellToCollapse.tileOptions[0];
        Instantiate(foundTile, cellToCollapse.transform.position, foundTile.transform.rotation);

        Propagate();
    }

    private void Propagate()
    {
        List<Cell> newPropagatedCells = new List<Cell>(gridComponents);
        for (int x = 0; x < dimX; x++)
        {
            for (int y = 0; y < dimY; y++)
            {
                for (int z = 0; z < dimZ; z++)
                {
                    int index = z + y * dimY + x * dimY * dimZ;
                    if (gridComponents[index].collapsed)
                    {
                        newPropagatedCells[index] = gridComponents[index];
                    }
                    else
                    {
                        List<Tile> options = new List<Tile>();
                        foreach(Tile t in tileObjects)
                        {
                            options.Add(t);
                        }

                        if (x > 0)
                        {
                            Cell back = gridComponents[z + y * dimY + (x - 1) * dimY * dimZ];
                            List<Tile> validOptions = new List<Tile>();

                            foreach (Tile possibleOptions in back.tileOptions)
                            {
                                int validOption = Array.FindIndex(tileObjects, obj => obj == possibleOptions);
                                Tile[] valid = tileObjects[validOption].frontNeighbors;

                                validOptions = validOptions.Concat(valid).ToList();
                            }

                            CheckValidity(options, validOptions);
                        } 
                        if (x < dimX - 1)
                        {
                            Cell front = gridComponents[z + y * dimY + (x + 1) * dimY * dimZ];
                            List<Tile> validOptions = new List<Tile>();

                            foreach (Tile possibleOptions in front.tileOptions)
                            {
                                int validOption = Array.FindIndex(tileObjects, obj => obj == possibleOptions);
                                Tile[] valid = tileObjects[validOption].backNeighbors;

                                validOptions = validOptions.Concat(valid).ToList();
                            }

                            CheckValidity(options, validOptions);
                        } 
                        if (y > 0)
                        {
                            Cell down = gridComponents[z + (y - 1) * dimY + x * dimY * dimZ];
                            List<Tile> validOptions = new List<Tile>();

                            foreach (Tile possibleOptions in down.tileOptions)
                            {
                                int validOption = Array.FindIndex(tileObjects, obj => obj == possibleOptions);
                                Tile[] valid = tileObjects[validOption].upNeighbors;

                                validOptions = validOptions.Concat(valid).ToList();
                            }

                            CheckValidity(options, validOptions);
                        } 
                        if (y < dimY - 1)
                        {
                            Cell up = gridComponents[z + (y + 1) * dimY + x * dimY * dimZ];
                            List<Tile> validOptions = new List<Tile>();

                            foreach (Tile possibleOptions in up.tileOptions)
                            {
                                int validOption = Array.FindIndex(tileObjects, obj => obj == possibleOptions);
                                Tile[] valid = tileObjects[validOption].downNeighbors;

                                validOptions = validOptions.Concat(valid).ToList();
                            }

                            CheckValidity(options, validOptions);
                        }
                        if (z > 0)
                        {
                            Cell right = gridComponents[(z - 1) + y * dimY + x * dimY * dimZ];
                            List<Tile> validOptions = new List<Tile>();

                            foreach (Tile possibleOptions in right.tileOptions)
                            {
                                int validOption = Array.FindIndex(tileObjects, obj => obj == possibleOptions);
                                Tile[] valid = tileObjects[validOption].leftNeighbors;

                                validOptions = validOptions.Concat(valid).ToList();
                            }

                            CheckValidity(options, validOptions);
                        } 
                        if (z < dimZ - 1)
                        {
                            Cell left = gridComponents[(z + 1) + y * dimY + x * dimY * dimZ];
                            List<Tile> validOptions = new List<Tile>();

                            foreach (Tile possibleOptions in left.tileOptions)
                            {
                                int validOption = Array.FindIndex(tileObjects, obj => obj == possibleOptions);
                                Tile[] valid = tileObjects[validOption].rightNeighbors;

                                validOptions = validOptions.Concat(valid).ToList();
                            }

                            CheckValidity(options, validOptions);
                        }
                    }
                }
            }
        }
        gridComponents = newPropagatedCells;
        _iteration++;

        if (_iteration < dimX * dimY * dimZ)
        {
            StartCoroutine(CheckEntropy());
        }
    }
    
    void CheckValidity(List<Tile> optionList, List<Tile> validOption)
    {
        for(int x = optionList.Count - 1; x >=0; x--)
        {
            Tile element = optionList[x];
            if (!validOption.Contains(element))
            {
                optionList.RemoveAt(x);
            }
        }
    }
}
