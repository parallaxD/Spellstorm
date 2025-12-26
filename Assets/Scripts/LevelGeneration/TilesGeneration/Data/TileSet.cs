using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Generation/TileSet")]
public class TileSet : ScriptableObject
{
    public List<MyTile> tiles;
}
