using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AbstractDungeonGenerator), true)]
public class RandomDungeonGeneratorEditor : Editor
{
    AbstractDungeonGenerator tileGenerator;
    TileManager tileManager;
    DecorationManager decorationManager;

    private void Awake()
    {
        tileGenerator = (AbstractDungeonGenerator)target;
        tileManager = FindFirstObjectByType<TileManager>();
        decorationManager = FindFirstObjectByType<DecorationManager>();

        InitializeTileManager();
        InitializeDecorationManager();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Create Dungeon"))
        {
            tileGenerator.GenerateDungeon();
            tileGenerator.GenerateDecoration();
        }
    }

    private void InitializeTileManager()
    {
        if (tileManager != null) tileManager.Initialize();
        else Debug.LogError("TileManager not found in the scene! (editor)");
    }

    private void InitializeDecorationManager()
    {
        if (decorationManager != null) decorationManager.Initialize();
        else Debug.LogError("DecorationManager not found in the scene! (editor)");
    }
}
