using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    protected DecorationGenerator decorationGenerator;
    [SerializeField]
    protected ColliderTilesGenerator colliderGenerator;
    [SerializeField]
    protected DecorationVizualizer decorationVizualizer;
    [SerializeField]
    protected ColliderTilesGenerator colliderTilesGenerator;

    protected override void RunProceduralGeneration()
    {
        CorridorDungeonGeneration();
        decorationGenerator.GenerateDecoration();
        colliderGenerator.GenerateColliderTiles();
    }

    private void CorridorDungeonGeneration()
    {
        var floorPositions = new HashSet<Vector2>();
        var potentialRoomPositions = new HashSet<Vector2>();

        CreateCorridors(floorPositions, potentialRoomPositions);

        var roomPositions = CreateRooms(potentialRoomPositions);

        floorPositions.UnionWith(roomPositions);

        pathVizualizer.PaintFloor(floorPositions);
    }

    private HashSet<Vector2> CreateRooms(HashSet<Vector2> potentialRoomPositions)
    {
        var roomPositions = new HashSet<Vector2>();
        var roomPositionsSet = new List<HashSet<Vector2>>();

        var roomToCreate = potentialRoomPositions
            .OrderBy(x => Guid.NewGuid())
            .Take(randomWalkParameters.roomCount)
            .ToList();

        foreach (var roomPosition in roomToCreate)
        {
            var roomRandomWalkPosition = RunRandomWalk(randomWalkParameters, roomPosition);
            roomPositions.UnionWith(roomRandomWalkPosition);
            roomPositionsSet.Add(roomRandomWalkPosition);
        }

        // Передача в декорации и коллайдеры
        DecorationGenerator.roomPositionsList = roomPositionsSet;
        ColliderTilesGenerator.roomPositionsList = roomPositionsSet;

        return roomPositions;
    }

    private void CreateCorridors(HashSet<Vector2> floorPositions, HashSet<Vector2> potentialRoomPositions)
    {
        var currentPosition = startPosition;
        var corridors = new List<HashSet<Vector2>>();
        potentialRoomPositions.Add(currentPosition);

        for (int i = 0; i < randomWalkParameters.corridorCount; i++)
        {
            var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(
                currentPosition,
                randomWalkParameters.corridorLength,
                randomWalkParameters.corridorWidth
            );
            currentPosition = corridor[corridor.Count - 1];
            potentialRoomPositions.Add(currentPosition);
            floorPositions.UnionWith(corridor);

            corridors.Add(new HashSet<Vector2>(corridor));
        }

        // Передача в коллайдеры
        ColliderTilesGenerator.corridorPositionsList = corridors;
    }
}
