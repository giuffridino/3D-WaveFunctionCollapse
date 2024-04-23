using System;
using System.Collections.Generic;
using System.Collections;
using Random = System.Random;
using System.Linq;
using UnityEngine;

public class WFC : MonoBehaviour
{
    [SerializeField] public int dimX;
    [SerializeField] public int dimY;
    [SerializeField] public int dimZ;
    [SerializeField] private Tile[] tileObjects;
    [SerializeField] private Cell cellObj;
    [SerializeField] private Tile backupTile;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private DecorationsCreator decorationsCreator;

    private int _iteration;
    private int _count;
    
    [HideInInspector]
    public List<Cell> gridComponents;
    [HideInInspector]
    public bool creatingPath = true;
    [HideInInspector]
    public Vector3 startingCell;
    [HideInInspector]
    public Vector3 finishCell;

    private readonly Random _rand = new Random();

    public void RunWfc()
    {
        StartCoroutine(Observe());
    }

    public void InitializeGrid()
    {
        GameObject grid = new GameObject("Cells");
        for (int x = 0; x < dimX; x++)
        {
            for (int y = 0; y < dimY; y++)
            {
                for (int z = 0; z < dimZ; z++)
                {
                    Cell newCell = Instantiate(cellObj, new Vector3(x, y, z), Quaternion.identity);
                    newCell.transform.parent = grid.transform;
                    newCell.name = "Cell_" + _count;
                    newCell.CreateCell(false, tileObjects);
                    gridComponents.Add(newCell);
                    _count++;
                }
            }
        }
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

        if (!allCollapsed)
        {
            float time = 0.001f;
            yield return new WaitForSeconds(time);
            CollapseCell(cellToCollapse);
        }
        else
        {
            Debug.Log("Entropy is 1 everywhere");
            decorationsCreator.AddDecorations(dimX, dimY, dimZ, gridComponents.ToArray(), startingCell);
            player.gameStarted = true;
        }
    }

    void CollapseCell(int index)
    {
        Cell cellToCollapse = gridComponents[index];

        int x = index / (dimY * dimZ);
        int y = (index / dimZ) % dimY;
        int z = index % dimZ;

        cellToCollapse.collapsed = true;
        Tile selectedTile;

        if (cellToCollapse.tileOptions.Length != 0)
        {
            List<Tile> weightedOptions = new List<Tile>();

            foreach (Tile tile in cellToCollapse.tileOptions)
            {
                if (tile.name == "Floor_p")
                {
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
                    if (!(y == 0 && tile == tileObjects[6]))
                        weightedOptions.Add(tile);
                }
            }

            if (x == 0 || x == dimX - 1 || z == 0 || z == dimZ - 1)
            {
                weightedOptions.RemoveAll(tile => tile.name.Contains("Stairs"));
            }

            selectedTile = weightedOptions[_rand.Next(0, weightedOptions.Count)];

            if (y == dimY - 1 && selectedTile != tileObjects[6])
            {
                selectedTile = tileObjects[7];
            }
        }
        else
        {
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
            if (!back.collapsed)
            {
                int validOption = Array.FindIndex(tileObjects, obj => obj == tileOfCollapsedCell);
                Tile[] valid = tileObjects[validOption].backNeighbors;
                back.RecreateCell(NewEntropy(back.tileOptions, tileOfCollapsedCell, valid));
            }
        }

        if (x < dimX - 1)
        {
            Cell front = gridComponents[z + y * dimZ + (x + 1) * dimY * dimZ];
            if (!front.collapsed)
            {
                int validOption = Array.FindIndex(tileObjects, obj => obj == tileOfCollapsedCell);
                Tile[] valid = tileObjects[validOption].frontNeighbors;
                front.RecreateCell(NewEntropy(front.tileOptions, tileOfCollapsedCell, valid));
            }
        }

        if (y > 0)
        {
            Cell down = gridComponents[z + (y - 1) * dimZ + x * dimY * dimZ];
            if (!down.collapsed)
            {
                int validOption = Array.FindIndex(tileObjects, obj => obj == tileOfCollapsedCell);
                Tile[] valid = tileObjects[validOption].downNeighbors;
                down.RecreateCell(NewEntropy(down.tileOptions, tileOfCollapsedCell, valid));
            }
        }

        if (y < dimY - 1)
        {
            Cell up = gridComponents[z + (y + 1) * dimZ + x * dimY * dimZ];
            if (!up.collapsed)
            {
                int validOption = Array.FindIndex(tileObjects, obj => obj == tileOfCollapsedCell);
                Tile[] valid = tileObjects[validOption].upNeighbors;
                up.RecreateCell(NewEntropy(up.tileOptions, tileOfCollapsedCell, valid));
            }
        }

        if (z > 0)
        {
            Cell right = gridComponents[(z - 1) + y * dimZ + x * dimY * dimZ];
            if (!right.collapsed)
            {
                int validOption = Array.FindIndex(tileObjects, obj => obj == tileOfCollapsedCell);
                Tile[] valid = tileObjects[validOption].rightNeighbors;
                right.RecreateCell(NewEntropy(right.tileOptions, tileOfCollapsedCell, valid));
            }
        }

        if (z < dimZ - 1)
        {
            Cell left = gridComponents[(z + 1) + y * dimZ + x * dimY * dimZ];
            if (!left.collapsed)
            {
                int validOption = Array.FindIndex(tileObjects, obj => obj == tileOfCollapsedCell);
                Tile[] valid = tileObjects[validOption].leftNeighbors;
                left.RecreateCell(NewEntropy(left.tileOptions, tileOfCollapsedCell, valid));
            }
        }

        if (x > 0 && y > 0)
        {
            Cell backDown = gridComponents[z + (y - 1) * dimZ + (x - 1) * dimY * dimZ];
            if (!backDown.collapsed)
            {
                int validOption = Array.FindIndex(tileObjects, obj => obj == tileOfCollapsedCell);
                Tile[] valid = tileObjects[validOption].back_downNeighbors;
                backDown.RecreateCell(NewEntropy(backDown.tileOptions, tileOfCollapsedCell, valid));
            }
        }

        if (x > 0 && y < dimY - 1)
        {
            Cell backUp = gridComponents[z + (y + 1) * dimZ + (x - 1) * dimY * dimZ];
            if (!backUp.collapsed)
            {
                int validOption = Array.FindIndex(tileObjects, obj => obj == tileOfCollapsedCell);
                Tile[] valid = tileObjects[validOption].back_upNeighbors;
                backUp.RecreateCell(NewEntropy(backUp.tileOptions, tileOfCollapsedCell, valid));
            }
        }

        if (x < dimX - 1 && y > 0)
        {
            Cell frontDown = gridComponents[z + (y - 1) * dimZ + (x + 1) * dimY * dimZ];
            if (!frontDown.collapsed)
            {
                int validOption = Array.FindIndex(tileObjects, obj => obj == tileOfCollapsedCell);
                Tile[] valid = tileObjects[validOption].front_downNeighbors;
                frontDown.RecreateCell(NewEntropy(frontDown.tileOptions, tileOfCollapsedCell, valid));
            }
        }

        if (x < dimX - 1 && y < dimY - 1)
        {
            Cell frontUp = gridComponents[z + (y + 1) * dimZ + (x + 1) * dimY * dimZ];
            if (!frontUp.collapsed)
            {
                int validOption = Array.FindIndex(tileObjects, obj => obj == tileOfCollapsedCell);
                Tile[] valid = tileObjects[validOption].front_upNeighbors;
                frontUp.RecreateCell(NewEntropy(frontUp.tileOptions, tileOfCollapsedCell, valid));
            }
        }

        if (z > 0 && y > 0)
        {
            Cell rightDown = gridComponents[(z - 1) + (y - 1) * dimZ + x * dimY * dimZ];
            if (!rightDown.collapsed)
            {
                int validOption = Array.FindIndex(tileObjects, obj => obj == tileOfCollapsedCell);
                Tile[] valid = tileObjects[validOption].right_downNeighbors;
                rightDown.RecreateCell(NewEntropy(rightDown.tileOptions, tileOfCollapsedCell, valid));
            }
        }

        if (z > 0 && y < dimY - 1)
        {
            Cell rightUp = gridComponents[(z - 1) + (y + 1) * dimZ + x * dimY * dimZ];
            if (!rightUp.collapsed)
            {
                int validOption = Array.FindIndex(tileObjects, obj => obj == tileOfCollapsedCell);
                Tile[] valid = tileObjects[validOption].right_upNeighbors;
                rightUp.RecreateCell(NewEntropy(rightUp.tileOptions, tileOfCollapsedCell, valid));
            }
        }

        if (z < dimZ - 1 && y > 0)
        {
            Cell leftDown = gridComponents[(z + 1) + (y - 1) * dimZ + x * dimY * dimZ];
            if (!leftDown.collapsed)
            {
                int validOption = Array.FindIndex(tileObjects, obj => obj == tileOfCollapsedCell);
                Tile[] valid = tileObjects[validOption].left_downNeighbors;
                leftDown.RecreateCell(NewEntropy(leftDown.tileOptions, tileOfCollapsedCell, valid));
            }
        }

        if (z < dimZ - 1 && y < dimY - 1)
        {
            Cell leftUp = gridComponents[(z + 1) + (y + 1) * dimZ + x * dimY * dimZ];
            if (!leftUp.collapsed)
            {
                int validOption = Array.FindIndex(tileObjects, obj => obj == tileOfCollapsedCell);
                Tile[] valid = tileObjects[validOption].left_upNeighbors;
                leftUp.RecreateCell(NewEntropy(leftUp.tileOptions, tileOfCollapsedCell, valid));
            }
        }

        if (!creatingPath)
        {
            StartCoroutine(Observe());
        }
    }

    List<Tile> CheckValidity(List<Tile> optionList, List<Tile> validOption)
    {
        var newOptions = new List<Tile>();
        for (int x = optionList.Count - 1; x >= 0; x--)
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

    Tile[] NewEntropy(Tile[] oldOptions, Tile tileOfCollapsedCell, Tile[] valid)
    {
        var validOptions = new List<Tile>();
        var options = oldOptions.ToList();

        validOptions = validOptions.Concat(valid).ToList();

        options = CheckValidity(options, validOptions);

        var newTileList = new Tile[options.Count];
        for (int i = 0; i < options.Count; i++)
        {
            newTileList[i] = options[i];
        }

        return newTileList;
    }

    public void SetSpecificTile(int index, int tileIndex)
    {
        var cellToCollapse = gridComponents[index];
        var selectedTile = tileObjects[tileIndex];

        cellToCollapse.tileOptions = new Tile[] { selectedTile };
        cellToCollapse.collapsed = true;
        var foundTile = cellToCollapse.tileOptions[0];
        var newTile = Instantiate(foundTile, cellToCollapse.transform.position, foundTile.transform.rotation);
        newTile.transform.parent = cellToCollapse.transform;
        Propagate(index);
    }
}