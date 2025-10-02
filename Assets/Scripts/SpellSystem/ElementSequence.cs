using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ElementSequence
{
    private List<(ElementType, int)> elementTuplesList = new();

    public ElementSequence AddElement(ElementType elementType, int count)
    {
        elementTuplesList.Add((elementType, count));
        return this;
    }

    private Dictionary<ElementType, int> GetGroupedElements()
    {
        var grouped = new Dictionary<ElementType, int>();
        foreach (var (type, count) in elementTuplesList)
        {
            if (grouped.ContainsKey(type))
                grouped[type] += count;
            else
                grouped[type] = count;
        }
        return grouped;
    }

    public override bool Equals(object obj)
    {
        if (obj is ElementSequence other)
        {
            var thisGrouped = GetGroupedElements();
            var otherGrouped = other.GetGroupedElements();

            if (thisGrouped.Count != otherGrouped.Count) return false;

            foreach (var kvp in thisGrouped)
            {
                if (!otherGrouped.TryGetValue(kvp.Key, out int otherCount) ||
                    otherCount != kvp.Value)
                    return false;
            }
            return true;
        }
        return false;
    }

    public override int GetHashCode()
    {
        var grouped = GetGroupedElements();
        int hash = 17;
        foreach (var kvp in grouped.OrderBy(x => x.Key))
        {
            hash = hash * 31 + kvp.Key.GetHashCode();
            hash = hash * 31 + kvp.Value.GetHashCode();
        }
        return hash;
    }

    public override string ToString()
    {
        var grouped = GetGroupedElements();
        return string.Join(", ", grouped.Select(kvp => $"{kvp.Key}:{kvp.Value}"));
    }
}