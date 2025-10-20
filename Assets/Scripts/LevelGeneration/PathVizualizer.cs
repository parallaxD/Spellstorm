using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathVizualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap;
    [SerializeField]
    private TileManager tileManager;

    public void PaintFloorTiles(HashSet<Vector2> floorPositions)
    {
        PaintBackgroundTiles(floorPositions, floorTilemap);
        PaintTiles(floorPositions, floorTilemap);
    }

    public void PaintBackgroundTiles(HashSet<Vector2> floorPositions, Tilemap tilemap)
    {
        var backgroundPositions = GetBackgroundPositions(floorPositions);

        foreach (var position in backgroundPositions)
        {
            var tile = tileManager.GetRandomBackgroundTile();
            PaintSingleTile(tilemap, tile, position);
        }
    }

    private void PaintTiles(HashSet<Vector2> positions, Tilemap tilemap)
    {
        foreach (var position in positions)
        {
            var tile = tileManager.GetRandomFloorTile();
            PaintSingleTile(tilemap, tile, position);
        }
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2 position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3)position);
        tilemap.SetTile(tilePosition, tile);
    }

    private HashSet<Vector2> GetBackgroundPositions(HashSet<Vector2> floorPositions)
    {
        var minX = floorPositions.Min(position => position.x);
        var maxX = floorPositions.Max(position => position.x);

        var minY = floorPositions.Min(position => position.y);
        var maxY = floorPositions.Max(position => position.y);

        var backgroundPositions = new HashSet<Vector2>();

        for (var x = minX - 1; x < maxX + 2; x += 0.5f)
        {
            for (var y = minY - 1; y < maxY + 2; y += 0.5f)
            {
                backgroundPositions.Add(new Vector2(x, y));
            }
        }

        return backgroundPositions;
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
    }
}
