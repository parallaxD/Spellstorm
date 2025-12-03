using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DecorationVizualizer : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase tile;
    [SerializeField] private DecorationManager decorationManager;

    private List<MyDecoration> decorations = new List<MyDecoration>();
    private readonly List<GameObject> spawnedDecorations = new List<GameObject>();

    public void PaintDecorations(List<HashSet<Vector2>> emptyPositionsRooms)
    {
        InitializeDecorationsList();
        CreateDecorationsInTilemap(emptyPositionsRooms);
    }

    private void CreateDecorationsInTilemap(List<HashSet<Vector2>> positionsRooms)
    {
        foreach (var positionsRoom in positionsRooms)
        {
            var positions = positionsRoom.ToList();
            var minCount = Mathf.Min(decorations.Count, positionsRoom.Count);

            for (var i = 0; i < minCount; i++)
            {
                var cell = tilemap.WorldToCell((Vector3)positions[i]);
                var world = tilemap.GetCellCenterWorld(cell);

                var go = GetDecorationGameObject($"Decoration{i}", decorations[i].Sprite, world);
                spawnedDecorations.Add(go);
            }
        }
    }

    private GameObject GetDecorationGameObject(string name, Sprite sprite, Vector3 position)
    {
        var go = new GameObject(name);
        var sr = go.AddComponent<SpriteRenderer>();

        sr.sprite = sprite;
        sr.sortingLayerName = "Decorations";
        sr.sortingOrder = 100;
        go.transform.SetParent(tilemap.transform, worldPositionStays: false);
        go.transform.position = position;

        return go;
    }

    private void InitializeDecorationsList()
    {
        decorations = decorationManager.decorations;

        if (decorations == null || decorations.Count == 0)
        {
            Debug.LogError("Decorations not found!");
            return;
        }
    }

    public void Clear()
    {
        for (int i = tilemap.transform.childCount - 1; i >= 0; i--)
        {
            var child = tilemap.transform.GetChild(i);

            if (Application.isPlaying)
                Destroy(child.gameObject);
            else
                DestroyImmediate(child.gameObject);
        }

        spawnedDecorations.Clear();
    }
}