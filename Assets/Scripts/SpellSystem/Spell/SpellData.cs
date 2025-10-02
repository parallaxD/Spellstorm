using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "SpellSystem/Spell")]
public class SpellData : ScriptableObject
{
    public string ID;
    public string Name;
    public string Cooldown;
    public Sprite Sprite;
    public List<ElementTuple> Receipt;
}

public static class SpellDataManager <T>
{
    
}
