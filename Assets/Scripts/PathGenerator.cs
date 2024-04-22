using System;
using System.Collections.Generic;
using System.Collections;
using Random = System.Random;
using System.Linq;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    [SerializeField] private Tile[] tileObjects;
    [SerializeField] private List<Cell> gridComponents;
    [SerializeField] private Cell cellObj;
    [SerializeField] private GameObject gateObject;
    [SerializeField] private GameObject treasureObject;
    [SerializeField] private WFC wfc;


    private int dimX;
    private int dimY;
    private int dimZ;

    private List<Vector3> path;

    private bool goneUp;
    private bool goneDown;
    private Vector3 prev;

    private Vector3 left;
    private Vector3 right;
    private Vector3 forward;
    private Vector3 back;
    private Vector3 up;
    private Vector3 down;
    private Vector3 up2;
    private Vector3 down2;

    private int _count;
    private Random rand = new Random();
    private void Start()
    {
        
        dimX = wfc.dimX;
        dimY = wfc.dimY;
        dimZ = wfc.dimZ;

        gridComponents = new List<Cell>();
        InitializePathGrid();
        wfc.InitializeGrid();
        GeneratePathBetweenStartAndExit();
        PrecollapsePath();
        wfc.creatingPath = false;
        wfc.startingCell = path[0];
        wfc.finishCell = path.Last();
        Instantiate(gateObject, wfc.startingCell + new Vector3(-0.5f,-0.3f,0), gateObject.transform.rotation);
        Instantiate(treasureObject, wfc.finishCell + new Vector3(0,-0.22f,0), treasureObject.transform.rotation);
        wfc.RunWfc();
    }

    private void InitializePathGrid()
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
                    _count++;
                }
            }
        }
    }

    private void GeneratePathBetweenStartAndExit()
    {
        int exit_X = rand.Next(0, dimX);
        int exit_Z = rand.Next(0, dimZ);
        Vector3 start = new Vector3(0, 0, dimZ / 2);
        Vector3 exit = new Vector3(exit_X, dimY-1, exit_Z);

        path = new List<Vector3>();

        List<Vector3> untouchable = new List<Vector3>();
        
		Vector3 current = start;
        prev = current;

        goneUp = false;
        goneDown = false;

        int index;

        bool random = true;
        // Initialize global counter to close the path after a tolerance
        int globalCounter = 0;
        int tolerance = dimX * dimY;

        // Initialize random counter to switch between random and minimizing distance
        int methodCounter = rand.Next(dimX / 5, dimX / 2);

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        Destroy(cube);

        while (current != exit)
        {
            ComputeNeighbors(current);
            List<Vector3> possibleDirections = PossibleDirections(current, untouchable);

            // Restart if no possible directions
            if (possibleDirections.Count == 0)
            {
                Debug.Log("No possible directions, restarting");
                ResetPathGeneration(start, ref current, ref prev, ref path, ref untouchable, ref random, ref globalCounter);
                continue;
            }

            if (!random)
            {
                // Choose the minimizing distance direction from possibleDirections
                float minDistance = float.MaxValue;
                index = 0;
                for (int i = 0; i < possibleDirections.Count; i++)
                {
                    float distance = CalculateDistance(possibleDirections[i], exit);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        index = i;
                    }
                }
            }
            else
            {
                // Choose a random direction from possibleDirections
                index = rand.Next(0, possibleDirections.Count);
            }

            if (globalCounter == 0)
            {
                possibleDirections.Clear();
                possibleDirections.Add(forward);
                index = 0;
            }

            Vector3 next = possibleDirections[index];
            Vector3 lastDirectionVector = current - prev;

            AddUntouchables(next, current, possibleDirections, untouchable);

            possibleDirections.Clear();

            path.Add(current);
            untouchable.Add(current);

            if (next == up || next == down)
            {
                current = next;
                path.Add(current);
                untouchable.Add(current);
                next += lastDirectionVector;
            }
            prev = current;
            current = next;

            // foreach (var item in untouchable)
            // {
            //     Instantiate(backupTile, item, Quaternion.identity);
            // }

            if (globalCounter < tolerance)
            {
                globalCounter++;
                methodCounter--;
                if (methodCounter == 0)
                {
                    random = !random;
                    methodCounter = rand.Next(dimX / 6, dimX / 3);
                }
            }
            else
            {
                random = false;
            }
        }
        foreach (Vector3 pathCell in path)
        {
            // var newCube = Instantiate(cube, pathCell, Quaternion.identity);
            foreach (Cell cell in gridComponents)
            {
                if (cell.transform.position == pathCell)
                {
                    // Instantiate the cube
                    // var newCube = Instantiate(cube, pathCell, Quaternion.identity);
                    // // Set the cell as the parent of the cube
                    // newCube.transform.parent = cell.transform;
                    break; // Exit the loop once we find the corresponding cell
                }
            }
        }
    }

    private bool CheckInGrid(Vector3 position)
    {
        if (position.x < dimX && position.y < dimY && position.z < dimZ && position.x >= 0 && position.y >= 0 && position.z >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void AddYRing(Vector3 position, List<Vector3> list)
    {
        int x = (int)position.x;
        int y = (int)position.y;
        int z = (int)position.z;

        for (int i = -2; i < 3; i++)
        {
            for (int j = -2; j < 3; j++)
            {
                Vector3 newPosition = new Vector3(x + i, y, z + j);
                if (CheckInGrid(newPosition))
                {
                    list.Add(newPosition);
                }
            }
        }
        list.Remove(position);
    }

    private void AddXRing(Vector3 position, List<Vector3> list)
    {
        int x = (int)position.x;
        int y = (int)position.y;
        int z = (int)position.z;

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                Vector3 newPosition = new Vector3(x, y + i, z + j);
                if (CheckInGrid(newPosition))
                {
                    list.Add(newPosition);
                }
            }
        }
        list.Remove(position);
    }

    private void AddZRing(Vector3 position, List<Vector3> list)
    {
        int x = (int)position.x;
        int y = (int)position.y;
        int z = (int)position.z;

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                Vector3 newPosition = new Vector3(x + i, y + j, z);
                if (CheckInGrid(newPosition))
                {
                    list.Add(newPosition);
                }
            }
        }
        list.Remove(position);
    }

    private float CalculateDistance(Vector3 start, Vector3 end)
    {
        return Vector3.Distance(start, end);
    }


    private void ComputeNeighbors(Vector3 current)
    {
        int x = (int)current.x;
        int y = (int)current.y;
        int z = (int)current.z;

        left = new Vector3(x, y, z + 1);
        right = new Vector3(x, y, z - 1);
        forward = new Vector3(x + 1, y, z);
        back = new Vector3(x - 1, y, z);
        up = new Vector3(x, y + 1, z);
        down = new Vector3(x, y - 1, z);
        up2 = new Vector3(x, y + 2, z);
        down2 = new Vector3(x, y - 2, z);
    }

    private List<Vector3> PossibleDirections(Vector3 current, List<Vector3> untouchable)
    {
        var possibleDirections = new List<Vector3>();

        if (CheckInGrid(forward) && !untouchable.Contains(forward))
        {
            possibleDirections.Add(forward);
        }
        if (CheckInGrid(back) && !untouchable.Contains(back))
        {
            possibleDirections.Add(back);
        }
        if (CheckInGrid(right) && !untouchable.Contains(right))
        {
            possibleDirections.Add(right);
        }
        if (CheckInGrid(left) && !untouchable.Contains(left))
        {
            possibleDirections.Add(left);
        }

        Vector3 upNext = up + (current - prev);
        Vector3 downNext = down + (current - prev);

        if (CheckInGrid(up) && !untouchable.Contains(up) && CheckInGrid(upNext) && !untouchable.Contains(upNext))
        {
            // Don't allow multiple up steps
            if (!goneUp)
            {
                possibleDirections.Add(up);
            }
            else
            {
                goneUp = false;
            }
        }
        if (CheckInGrid(down) && !untouchable.Contains(down) && CheckInGrid(downNext) && !untouchable.Contains(downNext))
        {
            // Don't allow multiple down steps
            if (!goneDown)
            {
                possibleDirections.Add(down);
            }
            else
            {
                goneDown = false;
            }
        }

        return possibleDirections;
    }

    private void AddUntouchables(Vector3 next, Vector3 current, List<Vector3> possibleDirections, List<Vector3> untouchable)
    {
        // Check the chosen direction
        if (next == up)
        {
            goneUp = true;
            AddYRing(current, untouchable);
            // Avoid next cells to be up;
            AddYRing(up2, untouchable);
        }
        else if (next == down)
        {
            goneDown = true;
            AddYRing(current, untouchable);
            // Avoid next cells to be down;
            AddYRing(down2, untouchable);
        }
        else if (next == forward || next == back)
        {
            AddXRing(current, untouchable);
        }
        else if (next == right || next == left)
        {
            AddZRing(current, untouchable);
        }
        // Now add every cell of possibleDirections to untouchable (except for index)
        foreach (var direction in possibleDirections)
        {
            if (direction != next)
            {
                untouchable.Add(direction);
            }
        }
    }

    private void ResetPathGeneration(Vector3 start, ref Vector3 current, ref Vector3 prev, ref List<Vector3> path, ref List<Vector3> untouchable, ref bool random, ref int globalCounter)
    {
        path.Clear();
        untouchable.Clear();
        //path.Add(start);
        //untouchable.Add(start);
        //untouchable.Add(start + new Vector3(0,1,0));
        //start.z++;

        current = start;
        prev = current;
        globalCounter = 0;
        goneUp = false;
        goneDown = false;
        random = true;
    }

    private void PrecollapsePath()
    {
        int floorIndex = 7;
        int stairsEastIndex = 8;
        int stairsSouthIndex = 9;
        int stairsWestIndex = 10;
        int stairsNorthIndex = 11;

        Vector3 lastDirection = new Vector3(1, 0, 0);

        for (int i = 0; i < path.Count - 1; i++)
        {
            Vector3 current = path[i];
            Vector3 next = path[i + 1];

            int x = (int)current.x;
            int y = (int)current.y;
            int z = (int)current.z;

            int indexCurrent = z + y * dimZ + x * dimY * dimZ;

            x = (int)next.x;
            y = (int)next.y;
            z = (int)next.z;

            int indexNext = z + y * dimZ + x * dimY * dimZ;

            if (current.y == next.y)
            {
                wfc.SetSpecificTile(indexCurrent, floorIndex);
                lastDirection = next - current;
            }
            else if (current.y < next.y)
            {
                // Going Up: Current cell is a stairs cell
                if (lastDirection == new Vector3(0, 0, 1))
                {
                    wfc.SetSpecificTile(indexCurrent, stairsWestIndex);
                }
                else if (lastDirection == new Vector3(1, 0, 0))
                {
                    wfc.SetSpecificTile(indexCurrent, stairsNorthIndex);
                }
                else if (lastDirection == new Vector3(0, 0, -1))
                {
                    wfc.SetSpecificTile(indexCurrent, stairsEastIndex);
                }
                else if (lastDirection == new Vector3(-1, 0, 0))
                {
                    wfc.SetSpecificTile(indexCurrent, stairsSouthIndex);
                }
                // Skip the next cell, will have to be empty
                i++;
            }
            else if (current.y > next.y)
            {
                // Going Down: Current cell is an empty cell
                // Skip to the next cell, which will have to be a stairs cell
                i++;
                if (lastDirection == new Vector3(0, 0, 1))
                {
                    wfc.SetSpecificTile(indexNext, stairsEastIndex);
                }
                else if (lastDirection == new Vector3(1, 0, 0))
                {
                    wfc.SetSpecificTile(indexNext, stairsSouthIndex);
                }
                else if (lastDirection == new Vector3(0, 0, -1))
                {
                    wfc.SetSpecificTile(indexNext, stairsWestIndex);
                }
                else if (lastDirection == new Vector3(-1, 0, 0))
                {
                    wfc.SetSpecificTile(indexNext, stairsNorthIndex);
                }
            }
        }
    }
}