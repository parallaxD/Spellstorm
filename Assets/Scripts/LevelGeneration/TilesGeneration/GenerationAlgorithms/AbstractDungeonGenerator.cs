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

    private void Start()
    {
        GenerateDungeon();
    }

    public void GenerateDungeon()
    {
        pathVizualizer.Clear();
        RunProceduralGeneration();
    }

    protected abstract void RunProceduralGeneration();
}
