using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SpellIDDataBase : MonoBehaviour
{
    private static Dictionary<string, Spell> SpellIDDB = new();

    private void Awake()
    {      
        var spellsData = (SpellsDB) Resources.Load("SpellsDataBase/SpellsDataBaseSO");
        SpellIDDB.Add("1", new Fireball(spellsData.spellsDB[0]));
        SpellIDDB.Add("2", new Waterball(spellsData.spellsDB[1]));
        SpellIDDB.Add("3", new Fire(spellsData.spellsDB[2]));
        SpellIDDB.Add("4", new FireEarth(spellsData.spellsDB[3]));
    }

    public static Spell GetSpellByID(string id)
    {
        return SpellIDDB[id];
    }
}
