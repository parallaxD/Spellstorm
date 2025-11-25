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
        var xMin = floorPositions.Min(p => p.x);
        var xMax = floorPositions.Max(p => p.x);
        var yMin = floorPositions.Min(p => p.y);
        var yMax = floorPositions.Max(p => p.y);

        var cx = (xMin + xMax) / 2f;
        var cy = (yMin + yMax) / 2f;

        Vector2[] corners =
        {
            new Vector2(xMin, yMin),
            new Vector2(xMin, yMax),
            new Vector2(xMax, yMin),
            new Vector2(xMax, yMax)
        };

        var V = 0f;
        foreach (var c in corners)
        {
            float val = Mathf.Abs(c.x - cx) / 2f + Mathf.Abs(c.y - cy);
            if (val > V) V = val;
        }

        var H = 2f * V;
        var backgroundPositions = new HashSet<Vector2>();

        for (float x = cx - H; x <= cx + H; x += 0.5f)
        {
            for (float y = cy - V; y <= cy + V; y += 0.5f)
            {
                float sumOfRatios =
                    Mathf.Abs((x - cx) / H) +
                    Mathf.Abs((y - cy) / V);

                if (sumOfRatios <= 1f)
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
