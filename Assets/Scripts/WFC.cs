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
	private int _count;
    
    private readonly Random _rand = new Random();


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
                    newCell.name = "Cell_" + _count;
                    newCell.CreateCell(false, tileObjects);
                    gridComponents.Add(newCell);
             		_count++;
                }
            }
        }
        StartCoroutine(Observe());
    }

    IEnumerator Observe()
    {
        bool allCollapsed = true;
        int cellToCollapse = -1;
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
                        cellToCollapse = z + y * dimZ + x * dimY * dimZ;
                    }
                }
            }
        }

        //Debug.LogWarning("collapsingCell:" + cellToCollapse + "with entropy:" + minEntropy);

        if(!allCollapsed)
        {
            float time = 0.001f;
            yield return new WaitForSeconds(time);
            CollapseCell(cellToCollapse);
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
            List<Tile> weightedOptions = new List<Tile>();
            foreach(Tile tile in cellToCollapse.tileOptions)
            {
                if(tile.name == "Floor_p")
                {
                    weightedOptions.Add(tile);
                    weightedOptions.Add(tile);
                    weightedOptions.Add(tile);
                    weightedOptions.Add(tile);
                    weightedOptions.Add(tile);
                    weightedOptions.Add(tile);
                    weightedOptions.Add(tile);
                    weightedOptions.Add(tile);
                    weightedOptions.Add(tile);
                    weightedOptions.Add(tile);
                }
                else
                {
                    weightedOptions.Add(tile);
                }
            }
            selectedTile = weightedOptions[_rand.Next(0, weightedOptions.Count)];
        }
        else
        {
            //Debug.Log("error");
            selectedTile = backupTile;
        }
        
        cellToCollapse.tileOptions = new Tile[] { selectedTile };
        Tile foundTile = cellToCollapse.tileOptions[0];
        Tile newTile = Instantiate(foundTile, cellToCollapse.transform.position, foundTile.transform.rotation);
        newTile.transform.parent = cellToCollapse.transform;
        Propagate(index);
    }

    private void Propagate(int collapsedCell)
    {
        int x = collapsedCell / (dimY * dimZ);
        int y = (collapsedCell / dimZ) % dimY;
        int z = collapsedCell % dimZ;
        
        int index = z + y * dimZ + x * dimY * dimZ;
        Tile tileOfCollapsedCell = gridComponents[collapsedCell].tileOptions[0];
        
        if (x > 0)
        {
            Cell back = gridComponents[z + y * dimZ + (x - 1) * dimY * dimZ];
            if(!back.collapsed){
                //Debug.Log("back");
               
                back.RecreateCell(newEntropy(back.tileOptions, tileOfCollapsedCell));
                    
            }
            
        } 
        if (x < dimX - 1)
        {
            Cell front = gridComponents[z + y * dimZ + (x + 1) * dimY * dimZ];
            if (!front.collapsed)
            {
                //Debug.Log("front");
                front.RecreateCell(newEntropy(front.tileOptions, tileOfCollapsedCell));
            }

        } 
        if (y > 0)
        {
            Cell down = gridComponents[z + (y - 1) * dimZ + x * dimY * dimZ];
            if (!down.collapsed)
            {
                //Debug.Log("down");
                down.RecreateCell(newEntropy(down.tileOptions, tileOfCollapsedCell));
            }
        } 
        if (y < dimY - 1)
        {
            
            Cell up = gridComponents[z + (y + 1) * dimZ + x * dimY * dimZ];
            if (!up.collapsed)
            {
                //Debug.Log("up");
                up.RecreateCell(newEntropy(up.tileOptions, tileOfCollapsedCell));
            }
        }
        if (z > 0)
        {
            Cell right = gridComponents[(z - 1) + y * dimZ + x * dimY * dimZ];
            if (!right.collapsed)
            {
                //Debug.Log("right");
                right.RecreateCell(newEntropy(right.tileOptions, tileOfCollapsedCell));
            }
        } 
        if (z < dimZ - 1)
        {
            Cell left = gridComponents[(z + 1) + y * dimZ + x * dimY * dimZ];
            if (!left.collapsed)
            {
                //Debug.Log("left");
                left.RecreateCell(newEntropy(left.tileOptions, tileOfCollapsedCell));
            }
        }
       
        if (x > 0 && y > 0)
        {
            Cell back_down = gridComponents[z + (y - 1) * dimZ + (x - 1) * dimY * dimZ];
            if(!back_down.collapsed){
                //Debug.Log("back_down");
                back_down.RecreateCell(newEntropy(back_down.tileOptions, tileOfCollapsedCell));
            }
            
        } 
            
        if (x > 0 && y < dimY - 1)
        {
            Cell back_up = gridComponents[z + (y + 1) * dimZ + (x - 1) * dimY * dimZ];
            if(!back_up.collapsed){
                //Debug.Log("back_up");
                back_up.RecreateCell(newEntropy(back_up.tileOptions, tileOfCollapsedCell));
            }
            
        }
        
        if (x < dimX - 1 && y > 0)
        {
            Cell front_down = gridComponents[z + (y - 1) * dimZ + (x + 1) * dimY * dimZ];
            if(!front_down.collapsed){
                //Debug.Log("front_down");
                front_down.RecreateCell(newEntropy(front_down.tileOptions, tileOfCollapsedCell));
            }
            
        }
        
        if (x < dimX - 1 && y < dimY - 1)
        {
            Cell front_up = gridComponents[z + (y + 1) * dimZ + (x + 1) * dimY * dimZ];
            if(!front_up.collapsed){
                //Debug.Log("front_up");
                front_up.RecreateCell(newEntropy(front_up.tileOptions, tileOfCollapsedCell));
            }
            
        }

        if (z > 0 && y > 0)
        {
            Cell right_down = gridComponents[(z - 1) + (y - 1) * dimZ + x * dimY * dimZ];
            if (!right_down.collapsed)
            {
                //Debug.Log("right_down");
                right_down.RecreateCell(newEntropy(right_down.tileOptions, tileOfCollapsedCell));
            }
        }

        if (z > 0 && y < dimY - 1)
            {
                Cell right_up = gridComponents[(z - 1) + (y + 1) * dimZ + x * dimY * dimZ];
                if (!right_up.collapsed)
                {
                    //Debug.Log("right_up");
                    right_up.RecreateCell(newEntropy(right_up.tileOptions, tileOfCollapsedCell));
                }
            }
        
        if (z < dimZ - 1 && y > 0)
        {
            Cell left_down = gridComponents[(z + 1) + (y - 1) * dimZ + x * dimY * dimZ];
            if (!left_down.collapsed)
            {
                //Debug.Log("left_down");
                left_down.RecreateCell(newEntropy(left_down.tileOptions, tileOfCollapsedCell));
            }
        }

        if (z < dimZ - 1 && y < dimY - 1)
        {
            Cell left_up = gridComponents[(z + 1) + (y + 1) * dimZ + x * dimY * dimZ];
            if (!left_up.collapsed)
            {
                //Debug.Log("left_up");
                left_up.RecreateCell(newEntropy(left_up.tileOptions, tileOfCollapsedCell));
            }
        }
            
        StartCoroutine(Observe());
    }
    
    List<Tile> CheckValidity(List<Tile> optionList, List<Tile> validOption)
    {
        //Debug.Log("__________check validity___________");
        //Debug.Log("options before:" + optionList.Count);
        string opts = "";
        foreach (Tile op in optionList)
        {
            opts += op.ToString() + " ";
        }

        //Debug.Log(opts);
        opts = "";
        
        //Debug.Log("valid options:" + validOption.Count);
        foreach (Tile op in validOption)
        {
            opts += op.ToString() + " ";
        }
        //Debug.Log(opts);
        opts = "";

        List<Tile> newOptions = new List<Tile>();
        for(int x = optionList.Count - 1; x >=0; x--)
        {
            
            Tile element = optionList[x];
            //Debug.Log("comparing: "+ element);
            foreach (var opt in validOption)
            {
                if (element.name == opt.name)
                {
                    //Debug.Log("inserito");
                    newOptions.Add(element);
                }
            }
            
        }
        
        //Debug.Log("options after" + optionList.Count);
        foreach (Tile op in optionList)
        {
            opts += op.ToString() + " ";
        }
        //Debug.Log(opts);

        return newOptions;
    }

    Tile[] newEntropy(Tile[] oldOptions, Tile tileOfCollapsedCell)
    {
        List<Tile> validOptions = new List<Tile>();
        List<Tile> options = oldOptions.ToList();

        int validOption = Array.FindIndex(tileObjects, obj => obj == tileOfCollapsedCell);
        Tile[] valid = tileObjects[validOption].backNeighbors;

        validOptions = validOptions.Concat(valid).ToList();
                
        options = CheckValidity(options, validOptions);
                
        Tile[] newTileList = new Tile[options.Count];
        for(int i = 0; i < options.Count; i++) {
            newTileList[i] = options[i];
        }

        return newTileList;
    }
    
}