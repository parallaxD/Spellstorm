using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Generation/DecorationSet")]
public class DecorationSet : ScriptableObject
{
    public List<MyDecoration> decorations;
}