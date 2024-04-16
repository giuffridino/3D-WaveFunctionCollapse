using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tile))]
public class TileEditor : Editor
{
    private bool _showManualSetup = false; 
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Tile tile = (Tile)target;
        
        if (GUILayout.Button("Upload and rotate neighbors"))
        {
            tile.UploadAndRotateNeighborhood();
        }

        _showManualSetup = EditorGUILayout.Foldout(_showManualSetup, "Manual setup");
        if (_showManualSetup)
        {
            if (GUILayout.Button("Upload connections from existing tile"))
            {
                tile.UploadNeighborhood();
            }
            
            if (GUILayout.Button("90° Neighborhood Rotation"))
            {
                tile.RotateNeighborLists();
            }
		}
	}
}