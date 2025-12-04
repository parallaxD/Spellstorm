using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AbstractDungeonGenerator), true)]
public class RandomDungeonGeneratorEditor : Editor
{
    AbstractDungeonGenerator tileGenerator;
    DecorationGenerator decorationGenerator;
    TileManager tileManager;
    DecorationManager decorationManager;

    private void Awake()
    {
        tileGenerator = (AbstractDungeonGenerator)target;
        decorationGenerator = FindFirstObjectByType<DecorationGenerator>();
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
            decorationGenerator.GenerateDecoration();
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
