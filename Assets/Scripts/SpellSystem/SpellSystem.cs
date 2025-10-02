using System.Collections.Generic;
using UnityEngine;

public class SpellSystem : MonoBehaviour
{

    public List<(ElementType, int)> inputTuplesList = new();
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inputTuplesList.Add((ElementType.Fire, 1));
            Debug.Log(inputTuplesList.Count);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            inputTuplesList.Add((ElementType.Water, 1));
            Debug.Log(inputTuplesList.Count);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            var id = SpellReceiptDataBase.GetIDBySequence(GetSequence());
            if (id != null)
            {
                var spell = CreateSpell(id);
                spell.Action();
            }
        }
    }
    
    public ElementSequence GetSequence()
    {
        ElementSequence sequence = new ElementSequence();

        foreach (var (elementType, count) in inputTuplesList)
        {
            sequence.AddElement(elementType, count);
        }

        inputTuplesList.Clear();

        return sequence;
    }

    private Spell CreateSpell(string spellID)
    {
        return SpellIDDataBase.GetSpellByID(spellID);
    }
}
