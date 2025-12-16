using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpellSystem : MonoBehaviour
{
    private List<ElementType> Elements = new();
    private SpellStash _spellStash = new();

    private const int MAX_ELEMENTS = 4;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AddElement(ElementType.Fire);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            AddElement(ElementType.Water);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            AddElement(ElementType.Earth);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            AddElement(ElementType.Wind);
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

        if (Input.GetKeyDown(KeyCode.C))
        {
            _spellStash.ClearStash();
            Elements.Clear();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            _spellStash.CastSpell();
            _spellStash.ClearStash();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log($"Текущее количество элементов: {Elements.Count}/{MAX_ELEMENTS}");
        }
    }

    private void AddElement(ElementType element)
    {
        if (Elements.Count < MAX_ELEMENTS)
        {
            Elements.Add(element);
            Debug.Log($"Добавлен элемент: {element}. Всего: {Elements.Count}/{MAX_ELEMENTS}");
        }
        else
        {
            Debug.LogWarning($"Достигнут лимит элементов ({MAX_ELEMENTS})! Элемент {element} не добавлен.");
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