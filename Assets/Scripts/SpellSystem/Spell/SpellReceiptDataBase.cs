using System;
using System.Collections.Generic;
using UnityEngine;


    public class SpellReceiptDataBase : MonoBehaviour
    {
        public static Dictionary<ElementSequence, string> SpellReceiptDB = new();

        private void Awake()
        {
            var spellData = (SpellsDB) Resources.Load("SpellsDataBase/SpellsDataBaseSO");
            foreach (var item in spellData.spellsDB)
            {
                SpellReceiptDB.Add(new ElementSequence(item.Receipt), item.ID);
            }
        }

        public static string GetIDBySequence(ElementSequence sequence)
        {
            if (SpellReceiptDB.ContainsKey(sequence))
            {
                return SpellReceiptDB[sequence];
            }
            else
            {
                Debug.Log("Element does not exist");
                return null;
            }
        }
    }

