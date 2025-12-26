using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DecorationVizualizer : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private DecorationManager decorationManager;
    [SerializeField] private DecorationTilesGenerator decorationTilesGenerator;

    private readonly List<GameObject> spawnedDecorations = new List<GameObject>();

    public void PaintDecorations(List<DecorationObject> decorationObjects)
    {
        CreateDecorationsInTilemap(decorationObjects);
    }

    private void CreateDecorationsInTilemap(List<DecorationObject> decorations)
    {
        var positions = new HashSet<Vector2>();

        for (var i = 0; i < decorations.Count; i++)
        {
            var position = decorations[i].Center;
            var sprite = decorations[i].Sprite;

            var cell = tilemap.WorldToCell((Vector3)position);
            var world = tilemap.GetCellCenterWorld(cell);

            var go = GetDecorationGameObject($"Decoration{i}", sprite, world);
            spawnedDecorations.Add(go);

            positions.Add(position);
        }

        // Генерация тайлов
        decorationTilesGenerator.GenerateDecorationTiles(positions);
    }

    private GameObject GetDecorationGameObject(string name, Sprite sprite, Vector3 position)
    {
        var go = new GameObject(name);
        var sr = go.AddComponent<SpriteRenderer>();

        sr.sprite = sprite;
        sr.sortingLayerName = "Decorations";
        sr.sortingOrder = 2;
        sr.spriteSortPoint = SpriteSortPoint.Pivot;
        go.transform.SetParent(tilemap.transform, worldPositionStays: false);
        go.transform.position = position;

        return go;
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