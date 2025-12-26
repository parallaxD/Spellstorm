using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColliderTilesGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField] private float sizeOffset = LevelGenerationConstants.roomCollidersSizeOffset;

    public static List<HashSet<Vector2>> roomPositionsList;
    public static List<HashSet<Vector2>> corridorPositionsList;

    public static HashSet<Vector2> roomAvialablePositions;
    public static HashSet<Vector2> corridorAvialablePositions;

    public void GenerateColliderTiles()
    {
        roomAvialablePositions = new HashSet<Vector2>();
        corridorAvialablePositions = new HashSet<Vector2>();

        var roomPotentialColliderPositions = GetPotentialColliderPositions(roomPositionsList, roomAvialablePositions);
        var corridorPotentialColliderPositions = GetPotentialColliderPositions(corridorPositionsList, corridorAvialablePositions);

        var roomColliders = CleanupColliderPositions(roomPotentialColliderPositions);
        var corridorColliders = CleanupColliderPositions(corridorPotentialColliderPositions);

        pathVizualizer.PaintColliderTiles(roomColliders);
        pathVizualizer.PaintColliderTiles(corridorColliders);
    }

    private HashSet<Vector2> GetPotentialColliderPositions(List<HashSet<Vector2>> positionsList, HashSet<Vector2> avialablePositions)
    {
        var result = new HashSet<Vector2>();

        foreach (var positions in positionsList)
        {
            var xStart = positions.Min(p => p.x) - sizeOffset;
            var xEnd = positions.Max(p => p.x) + sizeOffset;
            var yStart = positions.Min(p => p.y) - sizeOffset;
            var yEnd = positions.Max(p => p.y) + sizeOffset;

            for (var x = xStart; x < xEnd; x += 0.5f)
            {
                for (var y = yStart; y < yEnd; y += 0.5f)
                {
                    var position = new Vector2(x, y);

                    if (IsApproximately(x, xStart) || IsApproximately(x, xEnd - 0.5f) || IsApproximately(y, yStart) || IsApproximately(y, yEnd - 0.5f))
                    {
                        result.Add(position);
                    }
                    else
                    {
                        avialablePositions.Add(position);
                    }
                }
            }
        }

        return result;
    }

    private HashSet<Vector2> CleanupColliderPositions(HashSet<Vector2> positions)
    {
        var result = new HashSet<Vector2>();

        foreach (var position in positions)
        {
            if (!roomAvialablePositions.Contains(position) && !corridorAvialablePositions.Contains(position))
            {
                result.Add(position);
            }
        }

        return result;
    }

    private bool IsApproximately(float a, float b, float tolerance = 0.01f)
    {
        return Mathf.Abs(a - b) < tolerance;
    }
}