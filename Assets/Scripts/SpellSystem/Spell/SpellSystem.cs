using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpellSystem : MonoBehaviour
{
    public List<ElementType> Elements = new ();
    private SpellStash _spellStash = new ();
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Elements.Add(ElementType.Fire);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Elements.Add(ElementType.Water);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Elements.Add(ElementType.Earth);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            Elements.Add(ElementType.Wind);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            var id = SpellReceiptDataBase.GetIDBySequence(CreateSequence());
            if (id != null)
            {
                var spell = CreateSpell(id);
                _spellStash.ChangeCurrentSpell(spell);
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            _spellStash.CastSpell();
            _spellStash.ClearStash();
        }
    }

    private ElementSequence CreateSequence()
    {
        ElementSequence sequence = new ElementSequence();
  
        foreach (ElementType elementType in Enum.GetValues(typeof(ElementType)))
        {
            
            int count = Elements.Count(e => e == elementType);
            sequence.AddElement(elementType, count);
        }

        DebugSequence(sequence);
        Elements.Clear();

        return sequence;
    }

    private void DebugSequence(ElementSequence sequence)
    {
        foreach (var item in sequence.elementTuplesList)
        {
            Debug.Log($"{item.elementType} : {item.count}");
        }
    }

    private Spell CreateSpell(string spellID)
    {
        return SpellIDDataBase.GetSpellByID(spellID);
    }
}
