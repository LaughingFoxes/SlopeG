using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SlopeGen))]
public class Editor_Slope : Editor {
    const string fileSaveTmpPath = "Assets/Editor/slope_points.json";
    const string fileSavePath = "Assets/Game/Slope_States/{0}.json";

    bool tmpFile = true;
    int fileVersion
    {
        get
        {
            return _fileVersion;
        }
        set
        {
            if (_fileVersion != value)
                tmpFile = false;
            _fileVersion = value;
        }
    }
    int _fileVersion = 0;

    string getFileSavePath
    {
        get
        {
            return tmpFile ? fileSaveTmpPath : string.Format(fileSavePath, fileVersion);
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SlopeGen script = (SlopeGen)target;

        tmpFile = GUILayout.Toggle(tmpFile, "Temporary");

        fileVersion = EditorGUILayout.IntField("Slope Version", fileVersion);

        if (GUILayout.Button("Save points"))
        {
            Vector2Array points = new Vector2Array()
            {
                array = script.points
            };

            StreamWriter writer = new StreamWriter(getFileSavePath, false);
            writer.Write(JsonUtility.ToJson(points));
            writer.Close();
            Debug.Log("Saved new points");
        }

        if (GUILayout.Button("Load points"))
        {
            StreamReader reader = new StreamReader(getFileSavePath);
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