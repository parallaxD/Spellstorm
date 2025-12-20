using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ProceduralGenerationAlgorithms
{
    public static HashSet<Vector2> SimpleRandomWalk(Vector2 startPosition, int walkLength)
    {
        var path = new HashSet<Vector2>();

        path.Add(startPosition);

        var previousPosition = startPosition;

        for (int i = 0; i < walkLength; i++)
        {
            var newPosition = previousPosition + Direction2D.GetRandomCardinalDirection();
            path.Add(newPosition);
            previousPosition = newPosition;
        }

        return path;
    }

    public static List<Vector2> RandomWalkCorridor(Vector2 startPosition, int corridorLength, int corridorWidth)
    {
        var corridor = new HashSet<Vector2>();
        var randomDirection = Direction2D.GetRandomCorridorDirection();
        var currentPosition = startPosition;

        corridor.Add(currentPosition);

        for (int i = 0; i < corridorLength; i++)
        {
            currentPosition += randomDirection;
            var corridorRandomWalk = SimpleRandomWalk(currentPosition, corridorWidth);
            corridor.UnionWith(corridorRandomWalk);
        }

        return corridor.ToList();
    }
}
