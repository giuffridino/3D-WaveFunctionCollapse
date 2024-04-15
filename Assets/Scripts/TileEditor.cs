using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tile))]
public class TileEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Tile tile = (Tile)target;

        if (GUILayout.Button("Rotate Tile"))
        {
            tile.RotateTile();
        }
    }
}