using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimpleRandomWalkDungeonGenerator : AbstractDungeonGenerator
{
    protected override void RunProceduralGeneration()
    {
        var floorPositions = RunRandomWalk(randomWalkParameters, startPosition);
        pathVizualizer.Clear();
        pathVizualizer.PaintFloorTiles(floorPositions);
    }

    protected HashSet<Vector2> RunRandomWalk(SimpleRandomWalkData parameters, Vector2 position)
    {
        var currentPosition = position;
        var floorPositions = new HashSet<Vector2>();

        for (int i = 0; i < parameters.iterations; i++)
        {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, parameters.walkLength);
            floorPositions.UnionWith(path);

            if (parameters.startRandomlyEachIteration)
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
        }

        return floorPositions;
    }
}
