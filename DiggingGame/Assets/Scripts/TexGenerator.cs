using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TexGenerator))]
public class TexGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Generate"))
        {
            ((TexGenerator)target).Generate();
        }
    }
}
public class TexGenerator : MonoBehaviour
{
    public void Generate()
    {
        Mesh mesh = new Mesh();

        var pos = new Vector3[]
        {
            new Vector3(-2, 2),
            new Vector3(-2, -2),
            new Vector3(2, -2),
            new Vector3(2, 2)
        };

        var uv = new Vector2[]
        {
            new Vector2(-0.5f, 1.5f),
            new Vector2(-0.5f, -0.5f),
            new Vector2(0.5f, -0.5f),
            new Vector2(0.5f, 1.5f)
        };

        var triangles = new int[]
        {
            0, 2, 1,
            0, 3, 2
        };
        
        mesh.vertices = pos;
        mesh.uv = uv;
        mesh.triangles = triangles;
        GetComponent<MeshFilter>().mesh = mesh;
    }
}
