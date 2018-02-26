using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SlopeGen))]
public class Editor_Slope : Editor {
    const string fileSavePath = "Assets/Editor/slope_points.json";

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SlopeGen script = (SlopeGen)target;



        if (GUILayout.Button("Save points"))
        {
            Vector2Array points = new Vector2Array()
            {
                array = script.points
            };

            StreamWriter writer = new StreamWriter(fileSavePath, false);
            writer.Write(JsonUtility.ToJson(points));
            writer.Close();
            Debug.Log("Saved new points");
        }

        if (GUILayout.Button("Load points"))
        {
            StreamReader reader = new StreamReader(fileSavePath);
            script.points = JsonUtility.FromJson<Vector2Array>(reader.ReadToEnd()).array;
            reader.Close();
            Debug.Log("Loaded new points");
        }
    }
}

public struct Vector2Array
{
    public Vector2[] array;
}