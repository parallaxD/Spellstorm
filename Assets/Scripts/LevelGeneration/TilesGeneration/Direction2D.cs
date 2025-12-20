using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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