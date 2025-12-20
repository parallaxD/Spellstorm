using UnityEngine;

[System.Serializable]
public class MyDecoration
{
    public Sprite Sprite;
    public int Count;
    public bool isDecorNearPaths;

    private static readonly float[] StandardSizes = { 32f, 64f, 128f };

    public int GetSizeInTiles()
    {
        if (!Sprite) return 1;

        var maxSide = Mathf.Max(Sprite.rect.width, Sprite.rect.height);
        var closest = FindClosestStandardSize(maxSide);

        return closest switch
        {
            <= 32f => 1,
            <= 64f => 9,
            _ => 81
        };
    }

    private static float FindClosestStandardSize(float value)
    {
        var closest = StandardSizes[0];
        var minDiff = Mathf.Abs(value - closest);

        for (var i = 1; i < StandardSizes.Length; i++)
        {
            var diff = Mathf.Abs(value - StandardSizes[i]);
            if (diff < minDiff)
            {
                minDiff = diff;
                closest = StandardSizes[i];
            }
        }

        return closest;
    }
}
