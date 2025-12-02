using Newtonsoft.Json.Bson;
using UnityEngine;

public abstract class AbstractDungeonGenerator : MonoBehaviour
{
    [SerializeField]
    protected SimpleRandomWalkData randomWalkParameters;
    [SerializeField]
    protected PathVizualizer pathVizualizer = null;
    [SerializeField]
    protected Vector2 startPosition = Vector2.zero;
    [SerializeField]
    protected DecorationGenerator decorationGenerator;
    [SerializeField]
    protected DecorationVizualizer decorationVizualizer;

    private void Start()
    {
        GenerateDungeon();
        GenerateDecoration();
    }

    public void GenerateDungeon()
    {
        pathVizualizer.Clear();
        RunProceduralGeneration();
    }

    private void GenerateDecoration()
    {
        decorationVizualizer.Clear();
        decorationGenerator.GenerateDecoration();
    }

    protected abstract void RunProceduralGeneration();
}
