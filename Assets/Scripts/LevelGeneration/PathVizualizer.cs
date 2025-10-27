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
        var xMinFloorPosition = floorPositions.Min(position => position.x);
        var xMaxFloorPosition = floorPositions.Max(position => position.x);
        var yMinFloorPosition = floorPositions.Min(position => position.y);
        var yMaxFloorPosition = floorPositions.Max(position => position.y);

        var widthFloor = Math.Abs(xMaxFloorPosition - xMinFloorPosition);
        var heightFloor = Math.Abs(yMaxFloorPosition - yMinFloorPosition);

        var center = new Vector2(xMinFloorPosition + widthFloor / 2, yMinFloorPosition + heightFloor / 2);
        var xMin = xMinFloorPosition - widthFloor;
        var xMax = xMaxFloorPosition + widthFloor;
        var yMin = yMinFloorPosition - heightFloor / 4;
        var yMax = yMaxFloorPosition + heightFloor / 4;

        var halfHorizontalDiagonal = (widthFloor * 3) / 2;
        var halfVerticalDiagonal = (heightFloor + heightFloor / 2) / 2;

        var backgroundPositions = new HashSet<Vector2>();

        for (var x = xMin; x <= xMax; x += 0.5f)
        {
            for (var y = yMin; y <= yMax; y += 0.5f)
            {
                var sumOfRatios = Math.Abs((x - center.x) / halfHorizontalDiagonal) + Math.Abs((y - center.y) / halfVerticalDiagonal);

                if (sumOfRatios <= 1.0f)
                {
                    backgroundPositions.Add(new Vector2(x, y));
                }
            }
        }

        return backgroundPositions;
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
    }
}
