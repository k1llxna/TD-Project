using UnityEngine;
using UnityEditor;

// Create a custom editor script
[CustomEditor(typeof(MyObject))]
public class ColorChangeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MyObject myObject = (MyObject)target;

        // Add a button to change color
        if (GUILayout.Button("Change Color"))
        {
            // Change the color of the object
            myObject.ChangeColor();
        }
    }
}