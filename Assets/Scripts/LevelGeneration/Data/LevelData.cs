using UnityEngine;
using static LevelGenerationConstants;

[CreateAssetMenu(fileName = "LevelData", menuName = "Generation/Level Data")]
public class LevelData : ScriptableObject
{
    public LocationType locationType;

    [Header("Tiles")]
    public TileSet floorTileset;
    public TileSet backgroundTileset;
    public TileSet decorationTileset;

    [Header("Decoration")]
    public DecorationSet decorationSet;
}
