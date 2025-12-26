using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileVizualizer : MonoBehaviour
{
    [SerializeField] private Tilemap baseTilemap;
    [SerializeField] private Tilemap collidersTilemap;
    [SerializeField] private TileManager tileManager;

    [SerializeField] private float sizeOffset = LevelGenerationConstants.backgroundSizeOffset;

    public void PaintFloor(HashSet<Vector2> positions)
    {
        PaintBackgroundTiles(positions);
        PaintFloorTiles(positions);
    }

    private void PaintBackgroundTiles(HashSet<Vector2> floorPositions)
    {
        var backgroundPositions = GetBackgroundPositions(floorPositions);

        foreach (var position in backgroundPositions)
        {
            var tile = tileManager.GetRandomBackgroundTile();
            PaintSingleTile(tile, position, baseTilemap);
        }
    }

    private void PaintFloorTiles(HashSet<Vector2> positions)
    {
        foreach (var position in positions)
        {
            var tile = tileManager.GetRandomFloorTile();
            PaintSingleTile(tile, position, baseTilemap);
        }
    }

    public void PaintDecorationTiles(HashSet<Vector2> positions)
    {
        foreach (var position in positions)
        {
            var tile = tileManager.GetRandomDecorationTile();
            PaintSingleTile(tile, position, baseTilemap);
        }
    }

    public void PaintColliderTiles(HashSet<Vector2> positions)
    {
        foreach (var position in positions)
        {
            var tile = tileManager.GetRandomColliderTile();
            PaintSingleTile(tile, position, collidersTilemap);
        }
    }

    private void PaintSingleTile(TileBase tile, Vector2 position, Tilemap tilemap)
    {
        var tilePosition = baseTilemap.WorldToCell((Vector3)position);
        tilemap.SetTile(tilePosition, tile);
    }

    private HashSet<Vector2> GetBackgroundPositions(HashSet<Vector2> floorPositions)
    {
        var xMin = floorPositions.Min(p => p.x);
        var xMax = floorPositions.Max(p => p.x);
        var yMin = floorPositions.Min(p => p.y);
        var yMax = floorPositions.Max(p => p.y);

        var backgroundPositions = new HashSet<Vector2>();

        for (float x = xMin - sizeOffset; x < xMax + sizeOffset; x += 0.5f)
        {
            for (float y = yMin - sizeOffset; y < yMax + sizeOffset; y += 0.5f)
            {
                backgroundPositions.Add(new Vector2(x, y));
            }
        }

        return backgroundPositions;
    }

    public void Clear()
    {
        baseTilemap.ClearAllTiles();
        collidersTilemap.ClearAllTiles();
    }
}
