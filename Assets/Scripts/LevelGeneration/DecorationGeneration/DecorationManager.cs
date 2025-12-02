using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DecorationManager : MonoBehaviour
{
    [SerializeField] private DecorationSet decorationSet;

    private List<MyDecoration> decorations;

    public void Initialize()
    {
        decorations = decorationSet.decorations;
    }

    public List<MyDecoration> GetDecorations()
    {
        var result = new List<MyDecoration>();

        foreach (var decor in decorations)
        {
            for (int i = 0; i < decor.Count; i++)
            {
                result.Add(decor);
            }
        }

        return result;
    }
}