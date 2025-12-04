using System.Collections.Generic;
using UnityEngine;

public class DecorationTilesGenerator : SimpleRandomWalkDungeonGenerator
{
    public void GenerateDecorationTiles(HashSet<Vector2> decorationPositions)
    {
        var tilesPositions = GetDecorationTilesPositions(decorationPositions);
        pathVizualizer.PaintDecorationTiles(tilesPositions);
    }

    private HashSet<Vector2> GetDecorationTilesPositions(HashSet<Vector2> positions)
    {
        var result = new HashSet<Vector2>();

        foreach (var position in positions)
        {
            var path = RunRandomWalk(randomWalkParameters, position);
            result.UnionWith(path);
        }
        
        return result;
    }
}