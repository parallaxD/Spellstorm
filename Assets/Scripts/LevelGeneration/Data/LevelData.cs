using System.Collections.Generic;
using UnityEngine;
using static LevelGenerationConstants;

[CreateAssetMenu(fileName = "LevelData", menuName = "Generation/Level Data")]
public class LevelData : ScriptableObject
{
    public LocationType locationType;
    public string locationName;

    [Header("Tiles")]
    public TileSet floorTileset;
    public TileSet backgroundTileset;
    public TileSet decorationTileset;

    [Header("Decoration")]
    public DecorationSet decorationSet;

    [Header("Enemy Spawn Settings")]
    public List<EnemySpawnData> enemySpawnData = new List<EnemySpawnData>();
    public int maxTotalEnemies = 15;
    public float spawnCheckRadius = 1.5f;

    [Header("Spawn Zone Settings")]
    public Vector2 spawnZoneSize = new Vector2(5, 5);
    public int spawnZonesCount = 3;
}

[System.Serializable]
public class EnemySpawnData
{
    public GameObject enemyPrefab;
    [Range(0, 10)] public int minCount = 1;
    [Range(1, 20)] public int maxCount = 5;
    [Range(0f, 1f)] public float spawnChance = 0.8f;
}



