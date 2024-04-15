using System;
using System.Collections.Generic;
using System.Collections;
using Random = System.Random;
using System.Linq;
using UnityEngine;

public class WFC : MonoBehaviour
{
    [SerializeField] private int dimX;
    [SerializeField] private int dimY;
    [SerializeField] private int dimZ;
    [SerializeField] private Tile[] tileObjects;
    [SerializeField] private List<Cell> gridComponents;
    [SerializeField] private Cell cellObj;
    [SerializeField] private Tile backupTile;

    private int _iteration;

    private void Awake()
    {
        RunWfc();
    }

    private void RunWfc()
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
        StartCoroutine(Observe());
    }

    IEnumerator Observe()
    {
        bool allCollapsed = true;
        Random rand = new Random();
        int cellToCollaps = -1;
        int minEntropy = tileObjects.Length + 1;
        
        for (int x = 0; x < dimX; x++)
        {
            for (int y = 0; y < dimY; y++)
            {
                for (int z = 0; z < dimZ; z++)
                {
                    int entropy = gridComponents[z + y * dimZ + x * dimY * dimZ].tileOptions.Length;
                    if (!gridComponents[z + y * dimZ + x * dimY * dimZ].collapsed && entropy < minEntropy)
                    {
                        allCollapsed = false;
                        minEntropy = entropy;
                        cellToCollaps = z + y * dimZ + x * dimY * dimZ;
                    }
                }
            }
        }



        if(!allCollapsed)
        {
            float time = 0.01f;
            yield return new WaitForSeconds(time);
            CollapseCell(cellToCollaps);
        }
        else
        {
            Debug.Log("Entropy is 1 everywhere");
        }
    }

    void CollapseCell(int index)
    {
        Cell cellToCollapse = gridComponents[index];

        cellToCollapse.collapsed = true;
        Tile selectedTile;
        
        if(cellToCollapse.tileOptions.Length != 0)
        {
            Random rand = new Random();
            selectedTile = cellToCollapse.tileOptions[rand.Next(0, cellToCollapse.tileOptions.Length)];
        }
        else
        {
            Debug.Log("error");
            selectedTile = backupTile;
        }
        
        cellToCollapse.tileOptions = new Tile[] { selectedTile };
        Tile foundTile = cellToCollapse.tileOptions[0];
        Instantiate(foundTile, cellToCollapse.transform.position, foundTile.transform.rotation);
        Propagate(index);
    }

    private void Propagate(int collapsedCell)
    {
        
        for (int x = 0; x < dimX; x++)
        {
            for (int y = 0; y < dimY; y++)
            {
                for (int z = 0; z < dimZ; z++)
                {
                    int index = z + y * dimZ + x * dimY * dimZ;
                    Tile tileOfCollapsedCell = gridComponents[collapsedCell].tileOptions[0];
                    
                    if (index == collapsedCell)
                    {
                        if (x > 0)
                        {
                            Cell back = gridComponents[z + y * dimZ + (x - 1) * dimY * dimZ];
                            if(!back.collapsed){
                                List<Tile> validOptions = new List<Tile>();
                                List<Tile> options = back.tileOptions.ToList();

                                int validOption = Array.FindIndex(tileObjects, obj => obj == tileOfCollapsedCell);
                                Tile[] valid = tileObjects[validOption].backNeighbors;

                                validOptions = validOptions.Concat(valid).ToList();
                                
                                options = CheckValidity(options, validOptions);
                                
                                Tile[] newTileList = new Tile[options.Count];
                                for(int i = 0; i < options.Count; i++) {
                                    newTileList[i] = options[i];
                                }
                                back.RecreateCell(newTileList);
                            }
                            
                        } 
                        if (x < dimX - 1)
                        {
                            Cell front = gridComponents[z + y * dimZ + (x + 1) * dimY * dimZ];
                            if (!front.collapsed)
                            {
                                List<Tile> validOptions = new List<Tile>();
                                List<Tile> options = front.tileOptions.ToList();

                                int validOption = Array.FindIndex(tileObjects, obj => obj == tileOfCollapsedCell);
                                Tile[] valid = tileObjects[validOption].frontNeighbors;

                                validOptions = validOptions.Concat(valid).ToList();
                                
                                options = CheckValidity(options, validOptions);

                                Tile[] newTileList = new Tile[options.Count];
                                for (int i = 0; i < options.Count; i++)
                                {
                                    newTileList[i] = options[i];
                                }

                                front.RecreateCell(newTileList);
                            }

                        } 
                        if (y > 0)
                        {
                            Cell down = gridComponents[z + (y - 1) * dimZ + x * dimY * dimZ];
                            if (!down.collapsed)
                            {
                                List<Tile> validOptions = new List<Tile>();
                                List<Tile> options = down.tileOptions.ToList();

                                int validOption = Array.FindIndex(tileObjects, obj => obj == tileOfCollapsedCell);
                                Tile[] valid = tileObjects[validOption].downNeighbors;

                                validOptions = validOptions.Concat(valid).ToList();
                                options = CheckValidity(options, validOptions);

                                Tile[] newTileList = new Tile[options.Count];
                                for (int i = 0; i < options.Count; i++)
                                {
                                    newTileList[i] = options[i];
                                }

                                down.RecreateCell(newTileList);
                            }
                        } 
                        if (y < dimY - 1)
                        {
                            
                            Cell up = gridComponents[z + (y + 1) * dimZ + x * dimY * dimZ];
                            if (!up.collapsed)
                            {
                                List<Tile> validOptions = new List<Tile>();
                                List<Tile> options = up.tileOptions.ToList();

                                int validOption = Array.FindIndex(tileObjects, obj => obj == tileOfCollapsedCell);
                                Tile[] valid = tileObjects[validOption].upNeighbors;


                                validOptions = validOptions.Concat(valid).ToList();
                                options = CheckValidity(options, validOptions);


                                Tile[] newTileList = new Tile[options.Count];
                                for (int i = 0; i < options.Count; i++)
                                {
                                    newTileList[i] = options[i];
                                }

                                up.RecreateCell(newTileList);
                            }
                        }
                        if (z > 0)
                        {
                            Cell right = gridComponents[(z - 1) + y * dimZ + x * dimY * dimZ];
                            if (!right.collapsed)
                            {
                                List<Tile> validOptions = new List<Tile>();
                                List<Tile> options = right.tileOptions.ToList();

                                int validOption = Array.FindIndex(tileObjects, obj => obj == tileOfCollapsedCell);
                                Tile[] valid = tileObjects[validOption].rightNeighbors;

                                validOptions = validOptions.Concat(valid).ToList();
                                
                                options = CheckValidity(options, validOptions);
                                
                                Tile[] newTileList = new Tile[options.Count];
                                for (int i = 0; i < options.Count; i++)
                                {
                                    newTileList[i] = options[i];
                                }

                                right.RecreateCell(newTileList);
                            }
                        } 
                        if (z < dimZ - 1)
                        {
                            Cell left = gridComponents[(z + 1) + y * dimZ + x * dimY * dimZ];
                            if (!left.collapsed)
                            {
                                List<Tile> validOptions = new List<Tile>();
                                List<Tile> options = left.tileOptions.ToList();


                                int validOption = Array.FindIndex(tileObjects, obj => obj == tileOfCollapsedCell);
                                Tile[] valid = tileObjects[validOption].leftNeighbors;

                                validOptions = validOptions.Concat(valid).ToList();

                                options = CheckValidity(options, validOptions);
                                
                                
                                Tile[] newTileList = new Tile[options.Count];
                                for (int i = 0; i < options.Count; i++)
                                {
                                    newTileList[i] = options[i];
                                }
                                

                                left.RecreateCell(newTileList);
                            }
                        }
                        
                    }
                }
            }
        }
        
        StartCoroutine(Observe());
    }
    
    List<Tile> CheckValidity(List<Tile> optionList, List<Tile> validOption)
    {
        
        List<Tile> newOptions = new List<Tile>();
        for(int x = optionList.Count - 1; x >=0; x--)
        {
            
            Tile element = optionList[x];
            foreach (var opt in validOption)
            {
                if (element.name == opt.name)
                {

                    newOptions.Add(element);
                }
            }
            
        }
        return newOptions;
    }
    
}