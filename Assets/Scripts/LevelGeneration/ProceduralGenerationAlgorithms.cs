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
        var randomDirection = Direction2D.GetRandomCardinalDirection();
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

    /*public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minWidth, int minHeight)
    {
        var roomsQueue = new Queue<BoundsInt>();
        var roomsList = new List<BoundsInt>();
        roomsQueue.Enqueue(spaceToSplit);

        while (roomsQueue.Count > 0)
        {
            var room = roomsQueue.Dequeue();

            if (room.size.y >= minHeight && room.size.x >= minWidth)
            {
                if (room.size.y >= minHeight * 2)
                {
                    SplitHorizontally(minHeight, roomsQueue, room);
                }
                else if (room.size.x >= minWidth * 2)
                {
                    SplitVertically(minWidth, roomsQueue, room);
                }
                else
                {
                    roomsList.Add(room);
                }
            }
        }

        return roomsList;
    }

    private static void SplitVertically(int minWidth, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        var xSplit = Random.Range(1, room.size.x);
        var room1 = new BoundsInt(room.min, new Vector3Int(xSplit, room.size.y, room.size.z));
        var room2 = new BoundsInt(
            new Vector3Int(room.min.x + xSplit, room.min.y, room.min.z),
            new Vector3Int(room.size.x - xSplit, room.size.y, room.size.z)
            );

        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }

    private static void SplitHorizontally(int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        var ySplit = Random.Range(1, room.size.y);
        var room1 = new BoundsInt(room.min, new Vector3Int(room.size.x, ySplit, room.size.z));
        var room2 = new BoundsInt(
            new Vector3Int(room.min.x, room.min.y + ySplit, room.min.z),
            new Vector3Int(room.size.x, room.size.y - ySplit, room.size.z)
            );

        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }*/
}

public static class Direction2D
{
    public static List<Vector2> cardinalDirectionList = new List<Vector2>()
    {
        new Vector2(0, 0.5f),  // UP
        new Vector2(0.5f, 0),  // RIGHT
        new Vector2(0, -0.5f),  // DOWN
        new Vector2(-0.5f, 0)  // LEFT
    };

    public static Vector2 GetRandomCardinalDirection()
    {
        return cardinalDirectionList[Random.Range(0, cardinalDirectionList.Count)];
    }
}
