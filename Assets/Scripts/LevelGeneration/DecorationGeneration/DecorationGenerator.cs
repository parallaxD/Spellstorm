using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class DecorationGenerator : MonoBehaviour
{
    [SerializeField] private DecorationManager decorationManager;
    [SerializeField] private DecorationVizualizer decorationVizualizer;

    public static List<HashSet<Vector2>> roomPositionsList;
    private List<DecorationObject> decorationsObjects = new List<DecorationObject>();

    public void GenerateDecoration()
    {
        decorationVizualizer.Clear();
        GenerateDecorationObjectsInRooms();
        decorationVizualizer.PaintDecorations(decorationsObjects);
    }

    private void GenerateDecorationObjectsInRooms()
    {
        decorationsObjects.Clear();

        if (roomPositionsList == null || roomPositionsList.Count == 0)
        {
            Debug.LogError("roomPositionsList not found!");
            return;
        }

        foreach (var room in roomPositionsList)
        {
            var potentialPosition = GetPotentialDecorationPositionInRoom(room);
            CreateDecorationObjects(potentialPosition);
        }
    }

    private void CreateDecorationObjects(HashSet<Vector2> potentialPosition)
    {
        var decorations = decorationManager.decorations;

        for (var i = 0; i < decorations.Count; i++)
        {
            if (potentialPosition.Count == 0)
            {
                Debug.LogWarning("No more points to place decoration");
                break;
            }

            var randomPoint = GetRandomElement(potentialPosition);
            var occupiedPositions = GetOccupiedTiles(randomPoint, decorations[i].GetSizeInTiles());

            var decoration = new DecorationObject
            {
                Center = randomPoint,
                Sprite = decorations[i].Sprite,
                Positions = occupiedPositions
            };

            RemoveOccupiedTiles(decoration.Positions, potentialPosition);

            decorationsObjects.Add(decoration);
        }
    }

    private Vector2 GetRandomElement(HashSet<Vector2> set)
    {
        var index = Random.Range(0, set.Count);
        var i = 0;
        foreach (var p in set)
        {
            if (i == index) return p;
            i++;
        }
        throw new System.Exception("Random element failed");
    }

    private void RemoveOccupiedTiles(HashSet<Vector2> occupiedTiles, HashSet<Vector2> availableTiles)
    {
        foreach (var tile in occupiedTiles)
        {
            availableTiles.Remove(tile);
        }
    }

    private HashSet<Vector2> GetOccupiedTiles(Vector2 center, int tileCount)
    {
        var result = new HashSet<Vector2>();
        var tilesPerSide = Mathf.RoundToInt(Mathf.Sqrt(tileCount));
        var half = (tilesPerSide - 1) * 0.5f * 0.5f;

        var startX = center.x - half;
        var startY = center.y - half;

        for (var x = 0; x < tilesPerSide; x++)
        {
            for (var y = 0; y < tilesPerSide; y++)
            {
                var px = startX + x * 0.5f;
                var py = startY + y * 0.5f;

                result.Add(new Vector2(px, py));
            }
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