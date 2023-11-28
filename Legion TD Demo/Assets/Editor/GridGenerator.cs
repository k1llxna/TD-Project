using UnityEditor;
using UnityEngine;
public class GridGenerator : EditorWindow
{
    int gridSizeX = 5;
    int gridSizeY = 1;
    int gridSizeZ = 5;

    [SerializeField]
    Vector3 scale = new Vector3(1, .5f, 1);

    float cellSize = 1.0f;

    [MenuItem("Editor Tools/Create 3D Grid")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(GridGenerator));
    }

    void OnGUI()
    {
        GUILayout.Label("3D Grid Settings", EditorStyles.boldLabel);
        gridSizeX = EditorGUILayout.IntField("Grid Size X", gridSizeX);
        gridSizeY = EditorGUILayout.IntField("Grid Size Y", gridSizeY);
        gridSizeZ = EditorGUILayout.IntField("Grid Size Z", gridSizeZ);
        cellSize = EditorGUILayout.FloatField("Cell Size", cellSize);

        if (GUILayout.Button("Generate Grid"))
        {
            GenerateGrid();
        }
    }

    void GenerateGrid()
    {
        GameObject gridParent = new GameObject("Grid");

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int z = 0; z < gridSizeZ; z++)
                {
                    Vector3 position = new Vector3(x * cellSize, y * cellSize, z * cellSize);
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.localScale = scale;
                    cube.transform.position = position;
                    cube.transform.parent = gridParent.transform;
                    cube.AddComponent<Node>();
                    cube.AddComponent<BoxCollider>();

                    Node node = cube.GetComponent<Node>();
                    node.hoverColor = Color.black;
                    node.notEnoughColor = Color.red;
                    node.positionOffset.y = .5f;
                }
            }
        }
    }
}