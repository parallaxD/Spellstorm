using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ColliderTilesGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField] private float sizeOffset = 3f;
    public static List<HashSet<Vector2>> roomPositionsList;
    public static List<HashSet<Vector2>> corridorPositionsList;

    public void GenerateColliderTiles()
    {
        var positions = GetColliderTilesPositions();
        pathVizualizer.PaintColliderTiles(positions);
    }

    private HashSet<Vector2> GetColliderTilesPositions()
    {
        var result = new HashSet<Vector2>();

        foreach (var room in roomPositionsList)
        {
            var xStart = room.Min(p => p.x) - sizeOffset;
            var xEnd = room.Max(p => p.x) + sizeOffset;
            var yStart = room.Min(p => p.y) - sizeOffset;
            var yEnd = room.Max(p => p.y) + sizeOffset;

            for (float x = xStart; x < xEnd; x += 0.5f)
            {
                for (float y = yStart; y < yEnd; y += 0.5f)
                {
                    var position = new Vector2(x, y);

                    if ((x == xStart || x == xEnd - 1 ||  y == yStart || y == yEnd - 1) && F(position))
                    {
                        result.Add(position);
                    }
                }
            }
        }

        return result;
    }

    private bool F(Vector2 p)
    {
        return corridorPositionsList.All(
            x =>
            !x.Contains(p) && 
            !x.Contains(p + Direction2D.Up) &&
            !x.Contains(p + Direction2D.Left) &&
            !x.Contains(p + Direction2D.Right) &&
            !x.Contains(p + Direction2D.Down)
        );
    }
}