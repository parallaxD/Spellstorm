using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathVizualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap;
    [SerializeField]
    private TileManager tileManager;

    public void PaintFloorTiles(HashSet<Vector2> floorPositions)
    {
        PaintTiles(floorPositions, floorTilemap);
    }

    private void PaintTiles(HashSet<Vector2> positions, Tilemap tilemap)
    {
        foreach (var position in positions)
        {
            var tile = tileManager.GetRandomTile();
            PaintSingleTile(tilemap, tile, position);
        }
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2 position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3)position);
        tilemap.SetTile(tilePosition, tile);
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
    }
}
