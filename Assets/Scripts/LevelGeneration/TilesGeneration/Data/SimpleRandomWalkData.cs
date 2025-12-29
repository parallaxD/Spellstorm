using UnityEngine;

[CreateAssetMenu(fileName = "SimpleRandomWalkParameters_", menuName = "PCG/SimpleRandomWalkData")]
public class SimpleRandomWalkData : ScriptableObject
{
    [Header("Rooms")]
    public int iterations = 10;
    public int walkLength = 10;
    public int roomCount = 4;
    public bool startRandomlyEachIteration = true;

    [Header("Corridors")]
    public int corridorLength = 14;
    public int corridorWidth = 3;
    public int corridorCount = 5;
}
