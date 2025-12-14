using System.Collections.Generic;
using UnityEngine;

public class SpellComposer : MonoBehaviour
{
    // Спрайты четвертинок для каждой стихии
    public Sprite fireQuarter;
    public Sprite waterQuarter;
    public Sprite earthQuarter;
    public Sprite airQuarter;

    [SerializeField] private  GameObject quarterPrefab; // Префаб с SpriteRenderer

    // Главный метод: создает шар из четвертинок по рецепту
    public void ComposeSpell(List<ElementTuple> recipe)
    {
        //// Очищаем старые четвертинки
        //for (int i = gameObject.transform.childCount - 1; i >= 0; i--)
        //{
        //    Destroy(transform.GetChild(i).gameObject);
        //}

        if (recipe == null || recipe.Count == 0) return;

        // 1. Превращаем рецепт в плоский список элементов
        // [Earth:1, Water:2] → [Earth, Water, Water]
        List<ElementType> allElements = new List<ElementType>();
        foreach (ElementTuple tuple in recipe)
        {
            for (int i = 0; i < tuple.count; i++)
            {
                allElements.Add(tuple.elementType);
            }
        }

        // 2. Распределяем 4 четвертинки пропорционально
        ElementType[] quarters = new ElementType[4]; // Массив на 4 четвертинки

        if (allElements.Count == 1)
        {
            // Если всего 1 элемент - все 4 четвертинки одинаковые
            for (int i = 0; i < 4; i++) quarters[i] = allElements[0];
        }
        else
        {
            // Считаем сколько раз встречается каждый элемент
            Dictionary<ElementType, int> counts = new Dictionary<ElementType, int>();
            foreach (ElementType element in allElements)
            {
                if (!counts.ContainsKey(element)) counts[element] = 0;
                counts[element]++;
            }

            // Распределяем 4 слота пропорционально
            int slotsAssigned = 0;

            // Сначала даем по целому слоту на пропорцию
            foreach (var pair in counts)
            {
                int slots = Mathf.RoundToInt((float)pair.Value / allElements.Count * 4);
                for (int i = 0; i < slots && slotsAssigned < 4; i++)
                {
                    quarters[slotsAssigned] = pair.Key;
                    slotsAssigned++;
                }
            }

            // Если остались пустые слоты, заполняем самым частым элементом
            while (slotsAssigned < 4)
            {
                // Находим самый частый элемент
                ElementType mostCommon = ElementType.Fire;
                int maxCount = 0;
                foreach (var pair in counts)
                {
                    if (pair.Value > maxCount)
                    {
                        maxCount = pair.Value;
                        mostCommon = pair.Key;
                    }
                }

                quarters[slotsAssigned] = mostCommon;
                slotsAssigned++;
            }
        }

        // 3. Создаем 4 четвертинки на сцене
        for (int i = 0; i < 4; i++)
        {
            CreateQuarter(quarters[i], i);
        }
    }

    // Создает одну четвертинку
    private void CreateQuarter(ElementType element, int position)
    {
        GameObject quarter = new GameObject();
        Instantiate(quarterPrefab, Constants.PlayerTransform);
        quarter.name = element.ToString() + "_Quarter_" + position;

        // Поворачиваем на нужный угол (0°, 90°, 180°, 270°)
        float angle = position * 90f;
        quarter.transform.localRotation = Quaternion.Euler(0, 0, angle);

        // Назначаем спрайт
        SpriteRenderer renderer = quarter.GetComponent<SpriteRenderer>();
        renderer.sprite = GetSpriteForElement(element);
    }

    // Возвращает спрайт для элемента
    private Sprite GetSpriteForElement(ElementType element)
    {
        switch (element)
        {
            case ElementType.Fire: return fireQuarter;
            case ElementType.Water: return waterQuarter;
            case ElementType.Earth: return earthQuarter;
            case ElementType.Wind: return airQuarter;
            default: return fireQuarter;
        }
    }
}