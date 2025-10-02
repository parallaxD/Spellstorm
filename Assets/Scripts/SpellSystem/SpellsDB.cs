using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "a", menuName = "b")]
public class SpellsDB : ScriptableObject
{
    public List<SpellData> spellsDB;
}
