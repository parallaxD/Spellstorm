using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class DecorationGenerator : MonoBehaviour
{
    [SerializeField] private DecorationManager decorationManager;
    [SerializeField] private DecorationVizualizer decorationVizualizer;

    public static List<HashSet<Vector2>> roomPositionsList;

    public void GenerateDecorations()
    {
        var decorationPositions = GetDecorationPositions();
        decorationVizualizer.PaintDecorations(decorationPositions);
    }

    private List<HashSet<Vector2>> GetDecorationPositions()
    {
        if (roomPositionsList == null || roomPositionsList.Count == 0)
        {
            Debug.LogError("roomPositionsList not found!");
            return new List<HashSet<Vector2>>();
        }

        var result = new List<HashSet<Vector2>>();

        foreach (var room in roomPositionsList)
        {
            var potentialPosition = GetPotentialDecorationPositionInRoom(room);
            var positionInRoom = GetDecorationPositionInRoom(potentialPosition);
            result.Add(positionInRoom);
        }

        return result;
    }

    private HashSet<Vector2> GetDecorationPositionInRoom(HashSet<Vector2> potentialPosition)
    {
        var result = new HashSet<Vector2>();
        var decorations = decorationManager.decorations;

        if (potentialPosition.Count < decorations.Count)
        {
            Debug.LogError("The number of decorations exceeds the number of available points!");
            return new HashSet<Vector2>();
        }

        for (var i = 0; i < decorations.Count; i++)
        {
            var randomPoint = potentialPosition.ElementAt(Random.Range(0, potentialPosition.Count));
            result.Add(randomPoint);
        }

        return result;
    }

    private HashSet<Vector2> GetPotentialDecorationPositionInRoom(HashSet<Vector2> room)
    {
        var result = new HashSet<Vector2>();

        var xMin = room.Min(p => p.x);
        var xMax = room.Max(p => p.x);
        var yMin = room.Min(p => p.y);
        var yMax = room.Max(p => p.y);

        var sizeOffset = 3f;

        for (float x = xMin - sizeOffset; x < xMax + sizeOffset; x += 0.5f)
        {
            for (float y = yMin - sizeOffset; y < yMax + sizeOffset; y += 0.5f)
            {
                var currentPosition = new Vector2(x, y);

                if (!room.Contains(currentPosition))
                    result.Add(currentPosition);
            }
        }

        return result;
    }
}