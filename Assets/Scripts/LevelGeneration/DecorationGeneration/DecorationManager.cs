using System.Collections.Generic;
using UnityEngine;

public class DecorationManager : MonoBehaviour
{
    [SerializeField] public DecorationSet decorationSet;

    public List<MyDecoration> decorations;

    public void Initialize()
    {
        InitializeDecorationList();
    }

    public void InitializeDecorationList()
    {
        decorations.Clear();

        foreach (var decor in decorationSet.decorations)
        {
            for (int i = 0; i < decor.Count; i++)
            {
                decorations.Add(decor);
            }
        }
    }
}