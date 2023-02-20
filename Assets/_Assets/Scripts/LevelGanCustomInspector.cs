using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelGenerator))]
public class LevelGanCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LevelGenerator gen = (LevelGenerator)target;
        if(GUILayout.Button("Generate Level"))
        {
            gen.ClearTerrain();
            gen.GenerateTerrain();
        }
    }
}
