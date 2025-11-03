using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField]
    private TileBase defaultFloorTile;
    [SerializeField]
    private TileBase flowersFloorTile;
    [SerializeField]
    private TileBase decorationFloorTile1;
    [SerializeField]
    private TileBase decorationFloorTile2;

    [SerializeField]
    private TileBase defaultBackgroundTile;
    [SerializeField]
    private TileBase decorationBackgroundTile1;
    [SerializeField]
    private TileBase decorationBackgroundTile2;
    [SerializeField]
    private TileBase decorationBackgroundTile3;

    private List<MyTile> floorTiles;
    private List<MyTile> backgroundTiles;

    private void InitializeTiles()
    {
        floorTiles = new List<MyTile>
        {
            new MyTile { Sprite = defaultFloorTile, Weight = 90f },
            new MyTile { Sprite = flowersFloorTile, Weight = 5f },
            new MyTile { Sprite = decorationFloorTile1, Weight = 2.5f },
            new MyTile { Sprite = decorationFloorTile2, Weight = 2.5f }
        };

        backgroundTiles = new List<MyTile>
        {
            new MyTile { Sprite = defaultBackgroundTile, Weight = 97f },
            new MyTile { Sprite = decorationBackgroundTile1, Weight = 1f },
            new MyTile { Sprite = decorationBackgroundTile2, Weight = 1f },
            new MyTile { Sprite = decorationBackgroundTile3, Weight = 1f }
        };
    }

    public TileBase GetRandomFloorTile()
    {
        InitializeTiles();
        return GetRandomTile(floorTiles);
    }

    public TileBase GetRandomBackgroundTile()
    {
        InitializeTiles();
        return GetRandomTile(backgroundTiles);
    }

    public TileBase GetRandomTile(List<MyTile> spriteTiles)
    {
        if (spriteTiles == null || spriteTiles.Count == 0)
        {
            Debug.LogError("Weighted tiles list is not initialized or empty!");
            return null;
        }

        float totalWeight = spriteTiles.Sum(item => item.Weight);
        float randomValue = Random.Range(0f, totalWeight);

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
