using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColliderTilesGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField] private float sizeOffset = LevelGenerationConstants.roomCollidersSizeOffset;

    public static List<HashSet<Vector2>> roomPositionsList;
    public static List<HashSet<Vector2>> corridorPositionsList;

    public void GenerateColliderTiles()
    {
        var corridorColliders = GetCorridorColliderTilesPositions();
        var roomColliders = GetRoomColliderTilesPositions();

        pathVizualizer.PaintColliderTiles(roomColliders);
        pathVizualizer.PaintColliderTiles(corridorColliders);
    }

    private HashSet<Vector2> GetRoomColliderTilesPositions()
    {
        var result = new HashSet<Vector2>();

        foreach (var room in roomPositionsList)
        {
            var xStart = room.Min(p => p.x) - sizeOffset;
            var xEnd = room.Max(p => p.x) + sizeOffset;
            var yStart = room.Min(p => p.y) - sizeOffset;
            var yEnd = room.Max(p => p.y) + sizeOffset;

            for (var x = xStart; x < xEnd; x += 0.5f)
            {
                for (var y = yStart; y < yEnd; y += 0.5f)
                {
                    var position = new Vector2(x, y);

                    if ((x == xStart || x == xEnd - 1 ||  y == yStart || y == yEnd - 1) && IsPointOutsideAllCorridors(position))
                    {
                        result.Add(position);
                    }
                }
            }
        }

        return result;
    }

    private HashSet<Vector2> GetCorridorColliderTilesPositions()
    {
        var result = new HashSet<Vector2>();

        foreach (var corridor in corridorPositionsList)
        {
            var xStart = corridor.Min(p => p.x) - sizeOffset;
            var xEnd = corridor.Max(p => p.x) + sizeOffset;
            var yStart = corridor.Min(p => p.y) - sizeOffset;
            var yEnd = corridor.Max(p => p.y) + sizeOffset;

            for (var x = xStart; x < xEnd; x += 0.5f)
            {
                for (var y = yStart; y < yEnd; y += 0.5f)
                {
                    var position = new Vector2(x, y);

                    if (x == xStart || x == xEnd - 1 || y == yStart || y == yEnd - 1)
                    {
                        result.Add(position);
                    }
                }
            }
        }

        return result;
        // Стереть все, которые входят в область друг друга
    }

    private bool IsPointOutsideAllCorridors(Vector2 position)
    {
        return corridorPositionsList.All(corridorPositions =>
            !corridorPositions.Contains(position) &&
                Direction2D.cardinalDirectionList.All(
                    direction =>
                    !corridorPositions.Contains(position + direction)
                )
        );
    }
}