using System.Collections.Generic;
using UnityEngine;


public class SpellIDDataBase : MonoBehaviour
{
    private static Dictionary<string, Spell> SpellIDDB = new();

    private void Start()
    {
        SpellIDDB.Add("1", new Fireball());
        SpellIDDB.Add("2", new Waterball());
    }

    public static Spell GetSpellByID(string id)
    {
        return SpellIDDB[id];
    }
}
