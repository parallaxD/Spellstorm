using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static LevelGenerationConstants;

public class GenerationManager : MonoBehaviour
{
    [SerializeField] private TileManager tileManager;
    [SerializeField] private DecorationManager decorationManager;
    [SerializeField] private EnemySpawner enemySpawnManager;

    [SerializeField] AbstractDungeonGenerator tileGenerator;
    [SerializeField] DecorationGenerator decorationGenerator;

    [SerializeField] private LevelData BurningWastelandsData;
    [SerializeField] private LevelData FloodedTemplesData;
    [SerializeField] private LevelData CrystalSteppesData;
    [SerializeField] private LevelData AbyssOfReflections;

    [SerializeField] private PlayerMovement player;
    [SerializeField] private Portal portal;

    [SerializeField] private GameObject hubTilemapObject;
    [SerializeField] private GameObject basicTilemapObject;
    [SerializeField] private GameObject collidersTilemapObject;

    private List<LevelData> levelsData;
    private LocationType currentLocation = LocationType.Hub; 

    public LocationType CurrentLocation => currentLocation;

    public void Awake()
    {
        Initialize();
        HubGeneration();
    }


    private void Initialize()
    {
        levelsData = new List<LevelData>()
        {
            BurningWastelandsData,
            FloodedTemplesData,
            CrystalSteppesData,
            AbyssOfReflections
        };
    }

    public void GenerateNextLocation()
    {
        var totalLocations = System.Enum.GetValues(typeof(LocationType)).Length;
        var nextLocation = (LocationType)(((int)currentLocation + 1) % totalLocations);
        if ((int)nextLocation > 4)
        {
            nextLocation = LocationType.Hub;
        }
        currentLocation = nextLocation;


        if (nextLocation == LocationType.Hub)
        {
            HubGeneration();
        }
        else
        {
            LevelGeneration();
            portal.UpdatePosition();
        }

        player.ResetPosition();
    }

    private void LevelGeneration()
    {
        var level = levelsData.FirstOrDefault(l => l.locationType == currentLocation);

        if (level == null)
        {
            Debug.LogError($"Нет данных для локации: {currentLocation}");
            return;
        }

        TilesGeneration(level);
        DecorationGenerator(level);

        if (enemySpawnManager != null)
        {
            enemySpawnManager.StartSpawningForLocation(currentLocation, level);
        }

        hubTilemapObject.SetActive(false);
        basicTilemapObject.SetActive(true);
        collidersTilemapObject.SetActive(true);
    }

    public void HubGeneration()
    {
        currentLocation = LocationType.Hub;

        if (enemySpawnManager != null)
        {
            enemySpawnManager.StartSpawningForLocation(LocationType.Hub, null);
        }

        hubTilemapObject.SetActive(true);
        basicTilemapObject.SetActive(false);
        collidersTilemapObject.SetActive(false);

        player.ResetPosition();
        portal.ResetPosition();
    }

    private void TilesGeneration(LevelData level)
    {
        tileManager.floorTileSet = level.floorTileset;
        tileManager.backgroundTileSet = level.backgroundTileset;
        tileManager.decorationsTileSet = level.decorationTileset;
        tileManager.Initialize();
        tileGenerator.GenerateDungeon();
    }

    private void DecorationGenerator(LevelData level)
    {
        decorationManager.decorationSet = level.decorationSet;
        decorationManager.Initialize();
        decorationGenerator.GenerateDecoration();
    }
}