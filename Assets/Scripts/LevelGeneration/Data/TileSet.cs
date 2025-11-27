using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SpriteSets/TileSet")]
public class TileSet : ScriptableObject
{
    public List<MyTile> tiles;
}
