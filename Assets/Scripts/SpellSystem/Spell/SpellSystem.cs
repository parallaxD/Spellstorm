using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpellSystem : MonoBehaviour
{
    private List<ElementType> Elements = new();
    private SpellStash _spellStash = new();

    private const int MAX_ELEMENTS = 4;
    private bool _isCollectingMode = false;

    private void Update()
    {
        if (_isCollectingMode)
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
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (Elements.Count > 0)
            {
                var id = SpellReceiptDataBase.GetIDBySequence(CreateSequence());
                if (id != null)
                {
                    var spell = CreateSpell(id);
                    _spellStash.ChangeCurrentSpell(spell);
                    Debug.Log("Заклинание создано и сохранено в сташ!");

                    _isCollectingMode = false;
                }
                else
                {
                    Debug.LogWarning("Эта комбинация не является валидным заклинанием!");
                }
            }
            else
            {
                Debug.LogWarning("Нет элементов для создания заклинания!");
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            _spellStash.ClearStash();
            Elements.Clear();
            _isCollectingMode = false;
            Debug.Log("Все очищено! Режим сбора деактивирован.");
        }


        if (Input.GetKeyDown(KeyCode.Z))
        {
            _spellStash.CastSpell();
            _spellStash.ClearStash();

        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            ToggleSpellCollectionMode();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log($"Текущее количество элементов: {Elements.Count}/{MAX_ELEMENTS}");
            Debug.Log($"Режим сбора: {(_isCollectingMode ? "АКТИВЕН" : "не активен")}");
            if (_isCollectingMode)
            {
                Debug.Log($"Собираем заклинание: {string.Join(", ", Elements)}");
            }
        }
    }

    private void ToggleSpellCollectionMode()
    {
        if (!_isCollectingMode)
        {
            _isCollectingMode = true;
            Elements.Clear();
            Debug.Log("=== РЕЖИМ СБОРА ЗАКЛИНАНИЯ АКТИВИРОВАН ===");
            Debug.Log("Очищены все ячейки. Введите комбинацию элементов:");
            Debug.Log("Q - Огонь | E - Вода | R - Земля | T - Ветер");
            Debug.Log("G - Сохранить в сташ и выйти из режима");
            Debug.Log("V - Отмена сбора (очистить и выйти)");
        }
        else
        {

            _isCollectingMode = false;
            Elements.Clear();
            Debug.Log("Режим сбора отменен. Ячейки очищены.");
        }
    }

    private void AddElement(ElementType element)
    {
        if (!_isCollectingMode)
        {
            Debug.LogWarning("Не в режиме сбора! Нажмите V для активации.");
            return;
        }

        if (Elements.Count < MAX_ELEMENTS)
        {
            Elements.Add(element);
            Debug.Log($"Добавлен элемент: {element}. Всего: {Elements.Count}/{MAX_ELEMENTS}");

            if (Elements.Count == MAX_ELEMENTS)
            {
                Debug.Log("Достигнут максимум элементов! Автоматически создаем заклинание...");

                var id = SpellReceiptDataBase.GetIDBySequence(CreateSequence());
                if (id != null)
                {
                    var spell = CreateSpell(id);
                    _spellStash.ChangeCurrentSpell(spell);
                    Debug.Log("Заклинание создано и сохранено в сташ!");
                    _isCollectingMode = false;
                }
                else
                {
                    Debug.LogWarning("Эта комбинация не является валидным заклинанием!");
                    Debug.Log("Продолжайте ввод или нажмите V для отмены.");
                }
            }
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
        Debug.Log("Создана последовательность:");
        foreach (var item in sequence.elementTuplesList)
        {
            if (item.count > 0)
            {
                Debug.Log($"{item.elementType} : {item.count}");
            }
        }
    }

    private Spell CreateSpell(string spellID)
    {
        return SpellIDDataBase.GetSpellByID(spellID);
    }
}