using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SpriteSets/DecorationSet")]
public class DecorationSet : ScriptableObject
{
    public List<Decoration> decorations;
}