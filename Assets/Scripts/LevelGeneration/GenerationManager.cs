using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static LevelGenerationConstants;

public class GenerationManager : MonoBehaviour
{
    [SerializeField] private TileManager tileManager;
    [SerializeField] private DecorationManager decorationManager;

    [SerializeField] AbstractDungeonGenerator tileGenerator;
    [SerializeField] DecorationGenerator decorationGenerator;

    [SerializeField] private LevelData BurningWastelandsData;
    [SerializeField] private LevelData FloodedTemplesData;
    [SerializeField] private LevelData CrystalSteppesData;

    [SerializeField] private PlayerMovement player;
    [SerializeField] private Portal portal;

    [SerializeField] private GameObject hubTilemapObject;
    [SerializeField] private GameObject basicTilemapObject;
    [SerializeField] private GameObject collidersTilemapObject;

    private List<LevelData> levelsData;

    private LocationType currentLocation;

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
            CrystalSteppesData
        };
    }

    public void GenerateNextLocation()
    {
        var totalLocations = System.Enum.GetValues(typeof(LocationType)).Length;

        currentLocation = (LocationType)(((int)currentLocation + 1) % totalLocations);

        if (currentLocation == LocationType.Hub)
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

        TilesGeneration(level);
        DecorationGenerator(level);

        hubTilemapObject.SetActive(false);
        basicTilemapObject.SetActive(true);
        collidersTilemapObject.SetActive(true);
    }

    private void HubGeneration()
    {
        hubTilemapObject.SetActive(true);
        basicTilemapObject.SetActive(false);
        collidersTilemapObject.SetActive(false);

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
