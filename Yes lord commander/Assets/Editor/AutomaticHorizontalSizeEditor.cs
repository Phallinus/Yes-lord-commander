using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AutomaticHorizontalSize))]
public class AutomaticHorizontalSizeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Recalc Size"))
        {
            AutomaticHorizontalSize myscript = ((AutomaticHorizontalSize)target);
            myscript.AdjustSize();
        }
    }
}
