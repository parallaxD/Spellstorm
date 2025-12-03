using System.Collections.Generic;
using UnityEngine;

public class DecorationTilesGenerator : MonoBehaviour
{
    [SerializeField] TileVizualizer tileVizualizer;

    public void GenerateDecorationTiles(HashSet<Vector2> decorationPositions)
    {
        var tilesPositions = GetDecorationTilesPositions(decorationPositions);
        tileVizualizer.PaintDecorationTiles(tilesPositions);
    }

    private HashSet<Vector2> GetDecorationTilesPositions(HashSet<Vector2> positions)
    {
        var result = new HashSet<Vector2>();

        foreach (var position in positions)
        {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(position, 10); // ИЗМЕНИТЬ
            result.UnionWith(path);
        }

        return result;
    }
}