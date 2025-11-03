using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    private int corridorLength = 14;
    [SerializeField]
    private int corridorCount = 5;
    [SerializeField]
    private int roomCount = 4;

    protected override void RunProceduralGeneration()
    {
        CorridorDungeonGeneration();
    }

    private void CorridorDungeonGeneration()
    {
        var floorPositions = new HashSet<Vector2>();
        var potentialRoomPositions = new HashSet<Vector2>();

        CreateCorridors(floorPositions, potentialRoomPositions);

        var roomPositions = CreateRooms(potentialRoomPositions);

        floorPositions.UnionWith(roomPositions);

        pathVizualizer.PaintFloorTiles(floorPositions);
    }

    private HashSet<Vector2> CreateRooms(HashSet<Vector2> potentialRoomPositions)
    {
        var roomPositions = new HashSet<Vector2>();

        var roomToCreate = potentialRoomPositions
            .OrderBy(x => Guid.NewGuid())
            .Take(roomCount)
            .ToList();

        foreach (var roomPosition in roomToCreate)
        {
            var roomFloor = RunRandomWalk(randomWalkParameters, roomPosition);
            roomPositions.UnionWith(roomFloor);
        }

        return roomPositions;
    }

    private void CreateCorridors(HashSet<Vector2> floorPositions, HashSet<Vector2> potentialRoomPositions)
    {
        var currentPosition = startPosition;
        potentialRoomPositions.Add(currentPosition);

        for (int i = 0; i < corridorCount; i++)
        {
            var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, corridorLength);
            currentPosition = corridor[corridor.Count - 1];
            potentialRoomPositions.Add(currentPosition);
            floorPositions.UnionWith(corridor);
        }
    }
}
