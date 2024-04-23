using System;
using System.Collections.Generic;
using System.Collections;
using Random = System.Random;
using System.Linq;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    [SerializeField] private WFC wfc;
    [SerializeField] private GameObject gateObject;
    [SerializeField] private GameObject doorObject;


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
    private readonly Random _rand = new Random();
    private void Start()
    {
        StartGame();
    }

    private void GeneratePathBetweenStartAndExit()
    {
        Vector3 start = new Vector3(0, 0, dimZ / 2);
        Vector3 exit = new Vector3(dimX - 2, dimY-1, dimZ / 2);

        path = new List<Vector3>();

        List<Vector3> untouchable = new List<Vector3>();

        Vector3 current = start;
        prev = current;

        goneUp = false;
        goneDown = false;

        int index;

        bool random = true;
        int globalCounter = 0;
        int tolerance = dimX * dimY;

        int methodCounter = _rand.Next(dimX / 5, dimX / 2);

        while (current != exit)
        {
            ComputeNeighbors(current);
            List<Vector3> possibleDirections = PossibleDirections(current, untouchable);

            if (possibleDirections.Count == 0)
            {
                ResetPathGeneration(start, ref current, ref prev, ref path, ref untouchable, ref random, ref globalCounter);
                continue;
            }

            if (!random)
            {
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
                index = _rand.Next(0, possibleDirections.Count);
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

            if (globalCounter < tolerance)
            {
                globalCounter++;
                methodCounter--;
                if (methodCounter == 0)
                {
                    random = !random;
                    methodCounter = _rand.Next(dimX / 6, dimX / 3);
                }
            }
            else
            {
                random = false;
            }
        }
    }

    private bool CheckInGrid(Vector3 position)
    {
        if (position.x < dimX-1 && position.y < dimY && position.z < dimZ-1 && position.x > 0 && position.y >= 0 && position.z > 0)
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
        if (next == up)
        {
            goneUp = true;
            AddYRing(current, untouchable);
            AddYRing(up2, untouchable);
        }
        else if (next == down)
        {
            goneDown = true;
            AddYRing(current, untouchable);
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

            if ((int)current.y == (int)next.y)
            {
                wfc.SetSpecificTile(indexCurrent, floorIndex);
                lastDirection = next - current;
            }
            else if (current.y < next.y)
            {
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
                i++;
            }
            else if (current.y > next.y)
            {
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

    private void StartGame()
    {
        dimX = wfc.dimX;
        dimY = wfc.dimY;
        dimZ = wfc.dimZ;

        wfc.gridComponents.Clear();

        wfc.InitializeGrid();
        GeneratePathBetweenStartAndExit();
        PrecollapsePath();
        wfc.creatingPath = false;
        wfc.startingCell = path[0];
        wfc.finishCell = path.Last();
        var decorations = new GameObject("Decorations");
        var gate = Instantiate(gateObject, wfc.startingCell + new Vector3(-0.5f, -0.3f, 0), gateObject.transform.rotation);
        gate.transform.parent = decorations.transform;
        var door = Instantiate(doorObject, wfc.startingCell + new Vector3(-0.5f, -0.5f, 0.35f), doorObject.transform.rotation);
        door.transform.parent = decorations.transform;
        door.name = "Door";
        wfc.RunWfc();
    }
}