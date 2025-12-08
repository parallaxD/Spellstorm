using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ColliderTilesGenerator : SimpleRandomWalkDungeonGenerator
{
    public void GenerateColliderTiles(List<HashSet<Vector2>> roomsPositionsList)
    {
        var positions = GetColliderTilesPositions(roomsPositionsList);
        pathVizualizer.PaintColliderTiles(positions);
    }

    private HashSet<Vector2> GetColliderTilesPositions(List<HashSet<Vector2>> roomsPositionsList)
    {
        var result = new HashSet<Vector2>();
        var sizeOffset = 3f;

        foreach (var room in roomsPositionsList)
        {
            var xStart = room.Min(p => p.x) - sizeOffset;
            var xEnd = room.Max(p => p.x) + sizeOffset;
            var yStart = room.Min(p => p.y) - sizeOffset;
            var yEnd = room.Max(p => p.y) + sizeOffset;

            for (float x = xStart; x < xEnd; x += 0.5f)
            {
                for (float y = yStart; y < yEnd; y += 0.5f)
                {
                    if (x == xStart || x == xEnd - 1 ||  y == yStart || y == yEnd - 1)
                    {
                        result.Add(new Vector2(x, y));
                    }
                }
            }
        }

        return result;
    }
}