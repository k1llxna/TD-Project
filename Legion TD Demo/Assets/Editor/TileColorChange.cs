using UnityEngine;
using UnityEditor;

// Create a custom editor script
[CustomEditor(typeof(Node))]
public class ColorChangeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Node myObject = (Node)target;

        // Add a button to change color
        if (GUILayout.Button("Black"))
        {
            myObject.ChangeColor("Black");
        }
        if (GUILayout.Button("White"))
        {
            myObject.ChangeColor("White");
        }
        if (GUILayout.Button("Gray"))
        {
            myObject.ChangeColor("Gray");
        }
        if (GUILayout.Button("Red"))
        {
            myObject.ChangeColor("Red");
        }
        if (GUILayout.Button("Blue"))
        {
            myObject.ChangeColor("Blue");
        }
        if (GUILayout.Button("Green"))
        {
            myObject.ChangeColor("Green");
        }
    }
}