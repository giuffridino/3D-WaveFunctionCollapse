using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridManipulator))]
public class GridManipulatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridManipulator gridManipulator = (GridManipulator)target;

        if (GUILayout.Button("Manipulate Grid"))
        {
            gridManipulator.ManipulateGrid();
        }
    }
}