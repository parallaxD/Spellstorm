using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AbstractDungeonGenerator), true)]
public class RandomDungeonGeneratorEditor : Editor
{
    AbstractDungeonGenerator generator;

    private void Awake()
    {
        generator = (AbstractDungeonGenerator)target;
        InitializeTileManager();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Create Dungeon"))
        {
            generator.GenerateDungeon();
        }
    }

    private void InitializeTileManager()
    {
        var tileManager = FindFirstObjectByType<TileManager>();

        if (tileManager != null)
        {
            tileManager.Initialize();
        }
        else
        {
            Debug.LogError("TileManager not found in the scene! (editor)");
        }
    }
}
