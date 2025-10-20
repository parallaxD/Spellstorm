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

    private List<MyTile> tiles;

    private void InitializeTiles()
    {
        tiles = new List<MyTile>
        {
            new MyTile { Sprite = defaultFloorTile, Weight = 90f },
            new MyTile { Sprite = flowersFloorTile, Weight = 5f },
            new MyTile { Sprite = decorationFloorTile1, Weight = 2.5f },
            new MyTile { Sprite = decorationFloorTile2, Weight = 2.5f }
        };
    }

    public TileBase GetRandomTile()
    {
        InitializeTiles();

        if (tiles == null || tiles.Count == 0)
        {
            Debug.LogError("Weighted tiles list is not initialized or empty!");
            return null;
        }

        float totalWeight = tiles.Sum(item => item.Weight);
        float randomValue = Random.Range(0f, totalWeight);

        foreach (var item in tiles)
        {
            randomValue -= item.Weight;

            if (randomValue <= 0)
            {
                return item.Sprite;
            }
        }

        return tiles.Last().Sprite;
    }
}
