using System.Collections.Generic;
using UnityEngine;

public class DecorationManager : MonoBehaviour
{
    [SerializeField] private DecorationSet decorationSet;

    private List<MyDecoration> decorations;

    public void Initialize()
    {
        decorations = decorationSet.decorations;
    }

    public List<MyDecoration> GetDecorations() => decorations;
}