using UnityEngine;

public abstract class AbstractDungeonGenerator : MonoBehaviour
{
    [SerializeField]
    protected SimpleRandomWalkData randomWalkParameters;
    [SerializeField]
    protected TileVizualizer pathVizualizer = null;
    [SerializeField]
    protected Vector2 startPosition = Vector2.zero;

    public void GenerateDungeon()
    {
        pathVizualizer.Clear();
        RunProceduralGeneration();
    }

    protected abstract void RunProceduralGeneration();
}
