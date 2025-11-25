using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tiles/TileSet")]
public class TileSet : ScriptableObject
{
    public List<MyTile> tiles;
}
