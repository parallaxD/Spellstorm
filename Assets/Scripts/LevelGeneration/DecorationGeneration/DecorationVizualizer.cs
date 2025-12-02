using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class DecorationVizualizer : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase tile;
    [SerializeField] private DecorationManager decorationManager;

    private readonly List<GameObject> spawnedDecorations = new List<GameObject>();

    public void Vizualize(List<HashSet<Vector2>> emptyPositionsRooms)
    {
        Clear();
        var decorations = decorationManager.GetDecorations();

        if (decorations == null || decorations.Count == 0)
        {
            Debug.LogError("Decorations not found!");
            return;
        }

        foreach (var emptyPositionsRoom in emptyPositionsRooms)
        {
            var positions = emptyPositionsRoom.ToList();
            var minCount = Mathf.Min(decorations.Count, emptyPositionsRoom.Count);

            for (var i = 0; i < minCount; i++)
            {
                var cell = tilemap.WorldToCell((Vector3)positions[i]);
                var world = tilemap.GetCellCenterWorld(cell);

                var go = new GameObject($"Decoration{i}");
                var sr = go.AddComponent<SpriteRenderer>();

                sr.sprite = decorations[i].Sprite;
                sr.sortingLayerName = "Decorations";
                sr.sortingOrder = 100;
                go.transform.SetParent(tilemap.transform, worldPositionStays: false);
                go.transform.position = world;

                spawnedDecorations.Add(go);

                tilemap.SetTile(cell, tile);
            }
        }
    }

    public void Clear()
    {
        foreach (var deco in spawnedDecorations)
        {
            if (deco != null) DestroyImmediate(deco);
                //Destroy(deco);
        }

        spawnedDecorations.Clear();
    }
}