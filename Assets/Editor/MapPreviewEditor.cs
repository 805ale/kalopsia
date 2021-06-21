using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Extend from Editor instead of MonoBehaviour
// This is a custom Editor of the MapGenerator class
[CustomEditor(typeof(MapPreview))]
public class MapPreviewEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // get reference to MapGenerator
        MapPreview mapPreview = (MapPreview)target;

        // if any value is changed, then we can also generate the map
        if (DrawDefaultInspector())
        {
            if (mapPreview.autoUpdate)
            {
                mapPreview.DrawMapInEditor();
            }
        }

        // add a button - if the button is pressed, generate map
        if (GUILayout.Button("Generate"))
        {
            mapPreview.DrawMapInEditor();
        }
    }
}
