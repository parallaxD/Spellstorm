using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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

    public static List<Vector2> RandomWalkCorridor(Vector2 startPosition, int corridorLength)
    {
        var corridor = new List<Vector2>();
        var randomDirection = Direction2D.GetRandomCorridorDirection();
        var currentPosition = startPosition;

        corridor.Add(currentPosition);

        for (int i = 0; i < corridorLength; i++)
        {
            currentPosition += randomDirection;
            corridor.Add(currentPosition);

            IncreaseCorridorSizeByOne(corridor, currentPosition);
        }

        return corridor;
    }

    private static void IncreaseCorridorSizeByOne(List<Vector2> corridor, Vector2 position)
    {
        foreach (var direction in Direction2D.cardinalDirectionList)
        {
            position += direction;
            corridor.Add(position);
        }
    }
}

public static class Direction2D
{
    public static Vector2 Right = new Vector2(0.5f, 0);
    public static Vector2 Left = new Vector2(-0.5f, 0);
    public static Vector2 Up = new Vector2(0, 0.5f);
    public static Vector2 Down = new Vector2(0, -0.5f);

    private static Vector2 lastDirection;

    public static List<Vector2> cardinalDirectionList = new List<Vector2>() { Right, Left, Up, Down };

    public static Vector2 GetRandomCardinalDirection()
    {
        return cardinalDirectionList[Random.Range(0, cardinalDirectionList.Count)];
    }

    public static Vector2 GetRandomCorridorDirection()
    {
        var start = 0;
        var end = cardinalDirectionList.Count;

        if (lastDirection == Right || lastDirection == Left) start = 2;
        else end = cardinalDirectionList.Count - 2;

        var randomPosition = cardinalDirectionList[Random.Range(start, end)];
        lastDirection = randomPosition;

        return randomPosition;
    }
}
