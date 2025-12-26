using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] public TileSet floorTileSet;
    [SerializeField] public TileSet backgroundTileSet;
    [SerializeField] public TileSet decorationsTileSet;
    [SerializeField] private TileSet collidersTileSet;

    private List<MyTile> floorTiles;
    private List<MyTile> backgroundTiles;
    private List<MyTile> decorationsTiles;
    private List<MyTile> collidersTiles;

    public void Initialize()
    {
        floorTiles = floorTileSet.tiles;
        backgroundTiles = backgroundTileSet.tiles;
        decorationsTiles = decorationsTileSet.tiles;
        collidersTiles = collidersTileSet.tiles;
    }

    public TileBase GetRandomFloorTile() => GetRandomTile(floorTiles);

    public TileBase GetRandomBackgroundTile() => GetRandomTile(backgroundTiles);

    public TileBase GetRandomDecorationTile() => GetRandomTile(decorationsTiles);

    public TileBase GetRandomColliderTile() => GetRandomTile(collidersTiles);

    private TileBase GetRandomTile(List<MyTile> spriteTiles)
    {
        if (spriteTiles == null || spriteTiles.Count == 0)
        {
            Debug.LogError("Weighted tiles list is not initialized or empty!");
            return null;
        }

        var totalWeight = spriteTiles.Sum(item => item.Weight);
        var randomValue = Random.Range(0f, totalWeight);

        foreach (var item in spriteTiles)
        {
            randomValue -= item.Weight;

            if (randomValue <= 0)
            {
                return item.Sprite;
            }
        }

        return spriteTiles.Last().Sprite;
    }
}
