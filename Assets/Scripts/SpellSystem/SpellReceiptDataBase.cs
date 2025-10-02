using System;
using System.Collections.Generic;
using UnityEngine;


    public class SpellReceiptDataBase : MonoBehaviour
    {
        public static Dictionary<ElementSequence, string> SpellReceiptDB = new();

        private void Awake()
        {
            SpellReceiptDB.Add(new ElementSequence().AddElement(ElementType.Fire, 1), "1");
            SpellReceiptDB.Add(new ElementSequence().AddElement(ElementType.Water, 2), "2");
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

