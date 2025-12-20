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
        SpellIDDB.Add("1", new Fire(spellsData.spellsDB[0]));
        SpellIDDB.Add("2", new Water(spellsData.spellsDB[1]));
        SpellIDDB.Add("3", new Earth(spellsData.spellsDB[2]));
        SpellIDDB.Add("4", new Wind(spellsData.spellsDB[3]));
        SpellIDDB.Add("5", new FireFire(spellsData.spellsDB[4]));
        SpellIDDB.Add("6", new FireWater(spellsData.spellsDB[5]));
        SpellIDDB.Add("7", new FireEarth(spellsData.spellsDB[6]));
        SpellIDDB.Add("8", new FireWind(spellsData.spellsDB[7]));
        SpellIDDB.Add("9", new WaterWater(spellsData.spellsDB[8]));
        SpellIDDB.Add("10", new WaterEarth(spellsData.spellsDB[9]));
        SpellIDDB.Add("11", new WaterWind(spellsData.spellsDB[10]));
        SpellIDDB.Add("12", new EarthEarth(spellsData.spellsDB[11]));
        SpellIDDB.Add("13", new EarthWind(spellsData.spellsDB[12]));
        SpellIDDB.Add("14", new WindWind(spellsData.spellsDB[13]));
        SpellIDDB.Add("15", new FireFireFire(spellsData.spellsDB[14]));
        SpellIDDB.Add("16", new FireFireWater(spellsData.spellsDB[15]));
        SpellIDDB.Add("17", new FireFireEarth(spellsData.spellsDB[16]));
        SpellIDDB.Add("18", new FireFireWind(spellsData.spellsDB[17]));
        SpellIDDB.Add("19", new FireWaterWater(spellsData.spellsDB[18]));
        SpellIDDB.Add("20", new FireWaterEarth(spellsData.spellsDB[19]));
        SpellIDDB.Add("21", new FireWaterWind(spellsData.spellsDB[20]));
        SpellIDDB.Add("22", new FireEarthEarth(spellsData.spellsDB[21]));
        SpellIDDB.Add("23", new FireEarthWind(spellsData.spellsDB[22]));
        SpellIDDB.Add("24", new FireWindWind(spellsData.spellsDB[23]));
        SpellIDDB.Add("25", new WaterWaterWater(spellsData.spellsDB[24]));
        SpellIDDB.Add("26", new WaterWaterEarth(spellsData.spellsDB[25]));
        SpellIDDB.Add("27", new WaterWaterWind(spellsData.spellsDB[26]));
        SpellIDDB.Add("28", new WaterEarthEarth(spellsData.spellsDB[27]));
        SpellIDDB.Add("29", new WaterEarthWind(spellsData.spellsDB[28]));
        SpellIDDB.Add("30", new WaterWindWind(spellsData.spellsDB[29]));
        SpellIDDB.Add("31", new EarthEarthEarth(spellsData.spellsDB[30]));
        SpellIDDB.Add("32", new EarthEarthWind(spellsData.spellsDB[31]));
        SpellIDDB.Add("33", new EarthWindWind(spellsData.spellsDB[32]));
        SpellIDDB.Add("34", new WindWindWind(spellsData.spellsDB[33]));
    }

    public static Spell GetSpellByID(string id)
    {
        return SpellIDDB[id];
    }
}
