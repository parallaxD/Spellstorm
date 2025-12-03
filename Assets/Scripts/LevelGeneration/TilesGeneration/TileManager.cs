using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private TileSet floorTileSet;
    [SerializeField] private TileSet backgroundTileSet;

    private List<MyTile> floorTiles;
    private List<MyTile> backgroundTiles;

    public void Initialize()
    {
        floorTiles = floorTileSet.tiles;
        backgroundTiles = backgroundTileSet.tiles;
    }

    public TileBase GetRandomFloorTile() => GetRandomTile(floorTiles);

    public TileBase GetRandomBackgroundTile() => GetRandomTile(backgroundTiles);

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
